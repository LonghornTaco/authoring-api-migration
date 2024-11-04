# Getting Started

After cloning the repo, perform these steps:

1. Publish the VS solution in `/src/authoring-api-xm` (this is really only necessary to fix images locally, so they don't have to be published if you're looking at `master`).
2. Publish the VS solution in `/src/authoring-api-webhook`.
3. Run `.\init-starthere.ps1`.  This will do all the init container stuff.
4. Once all of the init containers have stopped, run:
      `.\start.ps1 -IncludeIndexRebuild`
5. Once the site comes up, do a full publish
6. Hit `https://local.authoring-api.com` and make sure the site comes up