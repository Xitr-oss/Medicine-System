# Backend CI Workflow

This folder contains the GitHub Actions workflow for the ASP.NET Core backend API.

## File

- `backend-ci.yml`: Runs CI for the backend project.

## What it does

1. Restores NuGet dependencies.
2. Builds the ASP.NET Core Web API in `Release` mode.
3. Runs test validation using `dotnet test`.
4. Publishes the app to `backend/publish`.
5. Uploads published files as the `backend-publish` artifact.

## Trigger conditions

The workflow runs on:

- Push to `main`
- Pull request targeting `main`
- Manual run from the Actions tab (`workflow_dispatch`)

## Notes

- The project targets `.NET 10` (`net10.0`), so the workflow installs SDK `10.0.x`.
- If you create dedicated test projects later, the test step can be updated to target them directly.
