# Build API
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY . ./
RUN cd src/Ducode.Wolk.Api && dotnet publish -c Release -o ../../out

# Build UI
FROM node AS gui-build-env
WORKDIR /app

COPY . ./
RUN cd ui && npm install && npm run build

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=gui-build-env /app/ui/dist ./gui
ENTRYPOINT ["dotnet", "Ducode.Wolk.Api.dll"]