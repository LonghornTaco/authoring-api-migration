# escape=`

ARG BASE_IMAGE
ARG SXA_IMAGE

FROM ${SXA_IMAGE} as sxa
FROM ${BASE_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Add SXA module
COPY --from=sxa C:\module\solr\cores-sxa.json C:\data\cores-sxa.json
COPY "scripts\CustomStart.ps1" c:\

ENTRYPOINT ["powershell", "C:/CustomStart.ps1"]