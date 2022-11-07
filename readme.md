# Playwright in GitHub Codespaces then Azure Container Apps

## Set up

* Create new repo from this template
* Then run:

```shell
az containerapp up \
--name <mynewapp> \
--resource-group <mynewapp> \
--location <myLocation e.g. uksouth> \
--repo https://github.com/<myrepo>/<mynewapp> \
--ingress external \
--target-port 5000
```

To make use of `ApiKeyMiddleware`:

* Add a github secret "API_KEY"
* Update the github workflow `.yml` file, before `az containerapp update` with:

```shell
az containerapp secret set -n myAppName -g myResourceGroup --secrets "api-key=${{ secrets.API_KEY }}"
az containerapp update -n myAppName -g myResourceGroup --set-env-vars "API_KEY=secretref:api-key"
```

## Description

The Codespace has:

* the main image based on the [Playwright for .NET
By Microsoft](https://hub.docker.com/_/microsoft-playwright-dotnet) docker image, allowing you headlessly run playwright
* docker in docker
* az cli
* a post startup command which will install a dev https certificate

The code will:

* start an api on port 5000 which when called runs playwright *(reminder: Azure Container Apps will talk to your app on port 5000, but externally expose this on port 443 (https))*
* use custom `ApiKeyMiddleware` on each endpoint
  * the calling entity must pass an "X-API-KEY" header
  * this header must match the contents of environment variable "API_KEY"