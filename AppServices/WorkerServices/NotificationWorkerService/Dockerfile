# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

RUN mkdir -p /source
WORKDIR /source

# Copy the main source project files
COPY ./AppServices/WorkerServices/NotificationWorkerService .
RUN dotnet restore

RUN dotnet publish -c debug -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "NotificationWorkerService.dll"]