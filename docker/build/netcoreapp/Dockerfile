# escape=`

ARG BASE_IMAGE

FROM ${BASE_IMAGE}

RUN mkdir c:\app\publish

WORKDIR .\app\publish

ENTRYPOINT ["dotnet", "AuthoringApi.NetCoreApp.dll"]