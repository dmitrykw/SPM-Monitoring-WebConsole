FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

RUN powershell -NoProfile -Command Remove-Item -Recurse C:\inetpub\wwwroot\*

WORKDIR /inetpub/wwwroot

COPY SPM_WebClient/ .