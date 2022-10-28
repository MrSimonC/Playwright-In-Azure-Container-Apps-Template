# Playwright in GitHub Codespaces then Azure Container Apps

## Set up

Add `.devcontainer/devcontainer.json` containing:

```json
{
	"name": "C# (.NET)",
	"image": "mcr.microsoft.com/playwright/dotnet:v1.27.0-focal",
	"features": {
		"ghcr.io/devcontainers/features/docker-in-docker:1": {},
		"ghcr.io/devcontainers/features/azure-cli:1": {}
	}
}
```

* VS Code Command: `Docker: Docker Add Files to Workspace`
  * Then update `final` to use `mcr.microsoft.com/playwright/dotnet:v1.27.0-focal`
  * Comment out the `USER appuser` section (so that root is used, bypassing permission errors)
* Test with VS Commands `Docker Build` and `Docker Run Interractive`
* Then run `az container up`:

```shell
az containerapp up \
--name playwrightinacodespace \
--resource-group playwrightinacodespace \
--location uksouth \
--repo https://github.com/MrSimonC/PlaywrightInACodeSpace \
--ingress external \
--target-port 5000
```

* Which outputs our url:

<https://playwrightinacodespace.politeocean-cf0d7198.uksouth.azurecontainerapps.io/>

* After the `devcontainer.json` experimentation, the rest of it look literally 1h30 on an evening to get working for the first time ever!

## Learnings

To set up the codespace, you add `.devcontainer/devcontainer.json`.

* Inside this file, you can have either `"image":` (standard github image, other, or your own) or `"build":` (pointing to a Dockerfile can then specify an image and RUN commands).
* Putting `"image": "mcr.microsoft.com/playwright/dotnet:v1.27.0-focal"` means you get dotnet and all the playwright browsers installed by the Microsoft Playwright team.

I experimented first by:

* Try `"build":` with a dockerfile, then try to `dotnet build` to then do `playwright.sh install` except code isn't loaded at the time of container creation
* `"image":` with Microsoft's standard dotnet. Then add `"features":` of powershell, then with `"postCreateCommand":` install browsers via playwright's getting started guide. Works super well, but takes 3 mins after running all the commands.
* Settled on using simply `"image": "mcr.microsoft.com/playwright/dotnet:v1.27.0-focal"`