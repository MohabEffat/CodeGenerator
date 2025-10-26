#!/usr/bin/env bash
set -euo pipefail

COLLECTION=${1:-postman/CodeGenerator.postman_collection.json}
ENVIRONMENT=${2:-postman/Local.postman_environment.json}
REPORT=${3:-postman/report.html}

if ! command -v newman >/dev/null 2>&1; then
  echo "Newman CLI not found. Install with: npm install -g newman newman-reporter-htmlextra" >&2
  exit 1
fi

if newman -h 2>&1 | grep -q htmlextra; then
  newman run "$COLLECTION" -e "$ENVIRONMENT" --reporters cli,htmlextra --reporter-htmlextra-export "$REPORT"
else
  newman run "$COLLECTION" -e "$ENVIRONMENT" --reporters cli
fi

echo "Collection run complete. Report: $REPORT"

