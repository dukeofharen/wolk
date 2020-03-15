FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102
WORKDIR /app
COPY dist/wolk .
ENTRYPOINT ["dotnet", "Ducode.Api.Web.dll"]