Postman Collection Runner
=========================

One‑click way to run and verify all API tests using Postman or Newman.

Option A — Postman Collection Runner (GUI)
- Import `postman/CodeGenerator.postman_collection.json` and `postman/Local.postman_environment.json`.
- Start the API: `dotnet run --urls http://localhost:5080`.
- In Postman, open the collection and click "Run" to execute all requests and tests.

Option B — Newman (CLI)
- Install Newman: `npm install -g newman newman-reporter-htmlextra`.
- Start the API: `dotnet run --urls http://localhost:5080`.
- Run the script:
  - Windows PowerShell: `./postman/run-collection.ps1`
  - macOS/Linux: `bash postman/run-collection.sh`
- If `htmlextra` reporter is installed, an HTML report is written to `postman/report.html`.

Parameters
- Both scripts accept optional args: `COLLECTION`, `ENVIRONMENT`, `REPORT` (shell) and `-Collection`, `-Environment`, `-Report` (PowerShell).

