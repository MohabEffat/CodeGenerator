using CodeGenerator;

var builder = WebApplication.CreateBuilder(args);

// CORS for frontend integration (adjust origins as needed)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

// Quick parameter validation for forms
// Usage: GET /validate?prefix=TJ&count=1000000&year=25
app.MapGet("/validate", (string? prefix, int? count, string? year) =>
{
    var errors = new List<string>();

    // prefix validation
    if (string.IsNullOrWhiteSpace(prefix))
        errors.Add("prefix is required");
    else if (prefix!.Length != 2)
        errors.Add("prefix must be exactly 2 characters");
    else if (!prefix.All(c => uCodeGenerator.AllowedPrefixChars.Contains(char.ToUpperInvariant(c))))
        errors.Add("prefix contains invalid characters");

    // count validation
    if (count is null)
        errors.Add("count is required");
    else if (count <= 0 || count > 1_000_000)
        errors.Add("count must be between 1 and 1,000,000");

    // year validation (optional)
    if (year is not null)
    {
        if (string.IsNullOrWhiteSpace(year) || year!.Length != 2 || !year.All(char.IsDigit))
            errors.Add("year must be a 2-digit string (e.g., '25')");
    }

    var ok = errors.Count == 0;
    var normalized = new
    {
        prefix = prefix?.ToUpperInvariant(),
        count,
        year = (year ?? "25")
    };

    return Results.Ok(new { ok, errors, normalized, limits = new { maxCsv = 1_000_000, maxJson = 10_000 } });
});

// Compute/verify checksum for a code
// Usage: GET /checksum?code=TJ25000000HV9
app.MapGet("/checksum", (string? code) =>
{
    if (string.IsNullOrWhiteSpace(code))
        return Results.BadRequest("code is required");

    code = code!.Trim().ToUpperInvariant();

    if (code.Length == 12)
    {
        var computed = Helper.CalculateChecksum(code);
        return Results.Ok(new { inputLength = 12, checksum = computed, fullCode = code + computed });
    }
    else if (code.Length == 13)
    {
        var body = code.Substring(0, 12);
        var providedChar = code[12];
        if (!char.IsDigit(providedChar))
            return Results.BadRequest("last character must be a digit checksum");
        var provided = providedChar - '0';
        var computed = Helper.CalculateChecksum(body);
        var valid = provided == computed;
        return Results.Ok(new { inputLength = 13, provided, computed, valid });
    }
    else
    {
        return Results.BadRequest("code must be 12 (no checksum) or 13 (with checksum) characters long");
    }
});

// Stream a CSV file with generated codes
// Usage: GET /generate-csv?prefix=TJ&count=1000000&year=25
app.MapGet("/generate-csv", async (string prefix, int count, string? year, HttpResponse response) =>
{
    // --- validation ---
    if (string.IsNullOrWhiteSpace(prefix) || prefix.Length != 2)
        return Results.BadRequest("prefix must be exactly 2 characters.");
    if (count <= 0 || count > 1_000_000)
        return Results.BadRequest("count must be between 1 and 1,000,000.");

    var y = year ?? "25";
    var generator = new uCodeGenerator(y);

    var fileName = $"codes_{prefix}_{y}_{count}.csv";

    // --- set headers ONCE and don't return Results.Text after ---
    response.ContentType = "text/csv";
    response.Headers["Content-Disposition"] = $"attachment; filename=\"{fileName}\"";

    // We'll always stream, even for small counts. Simpler + consistent.
    await using var writer = new StreamWriter(response.Body);

    for (int i = 0; i < count; i++)
    {
        var code = generator.GenerateCode(prefix);
        await writer.WriteLineAsync(code);
    }

    await writer.FlushAsync();

    // Important: return Results.Empty so we don't overwrite headers/body
    return Results.Empty;
});


// Optional: return small batches as JSON for previews
// Usage: GET /generate?prefix=TJ&count=100&year=25
app.MapGet("/generate", (string prefix, int count, string? year) =>
{
    if (string.IsNullOrWhiteSpace(prefix) || prefix.Length != 2)
        return Results.BadRequest("prefix must be exactly 2 characters.");
    if (count <= 0 || count > 10_000)
        return Results.BadRequest("count must be between 1 and 10,000.");

    var generator = new uCodeGenerator(year ?? "25");
    var codes = new List<string>(capacity: count);
    for (int i = 0; i < count; i++)
        codes.Add(generator.GenerateCode(prefix));

    return Results.Ok(new { prefix, year = year ?? "25", count, codes });
});

app.Run();
