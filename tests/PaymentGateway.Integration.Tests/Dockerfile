#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk-base
ARG BUILD_CONFIGURATION=Release
ARG BUILD_VERSION

FROM sdk-base AS build
WORKDIR /src
COPY ["tests/PaymentGateway.Integration.Tests/PaymentGateway.Integration.Tests.csproj", "tests/PaymentGateway.Integration.Tests/"]
COPY ["src/PaymentGateway.Clients/PaymentGateway.Clients.csproj", "src/PaymentGateway.Clients/"]
COPY ["src/PaymentGateway.Clients.Contract/PaymentGateway.Clients.Contract.csproj", "src/PaymentGateway.Clients.Contract/"]
COPY ["src/PaymentGateway.Core/PaymentGateway.Core.csproj", "src/PaymentGateway.Core/"]
RUN dotnet restore "./tests/PaymentGateway.Integration.Tests/PaymentGateway.Integration.Tests.csproj"
COPY . .
WORKDIR "/src/tests/PaymentGateway.Integration.Tests"
RUN dotnet build "./PaymentGateway.Integration.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaymentGateway.Integration.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM sdk-base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "test", "PaymentGateway.Integration.Tests.dll"]