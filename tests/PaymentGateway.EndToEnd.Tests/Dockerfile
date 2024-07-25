#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk-base
ARG BUILD_CONFIGURATION=Release
ARG BUILD_VERSION

FROM sdk-base AS build
WORKDIR /src
COPY ["tests/PaymentGateway.EndToEnd.Tests/PaymentGateway.EndToEnd.Tests.csproj", "tests/PaymentGateway.EndToEnd.Tests/"]
COPY ["src/PaymentGateway.Api/PaymentGateway.Api.csproj", "src/PaymentGateway.Api/"]
COPY ["src/PaymentGateway.Clients.Contract/PaymentGateway.Clients.Contract.csproj", "src/PaymentGateway.Clients.Contract/"]
COPY ["src/PaymentGateway.Core/PaymentGateway.Core.csproj", "src/PaymentGateway.Core/"]
COPY ["src/PaymentGateway.Clients/PaymentGateway.Clients.csproj", "src/PaymentGateway.Clients/"]
COPY ["src/PaymentGateway.Persistance.Contract/PaymentGateway.Persistance.Contract.csproj", "src/PaymentGateway.Persistance.Contract/"]
COPY ["src/PaymentGateway.Persistance/PaymentGateway.Persistance.csproj", "src/PaymentGateway.Persistance/"]
RUN dotnet restore "./tests/PaymentGateway.EndToEnd.Tests/PaymentGateway.EndToEnd.Tests.csproj"
COPY . .
WORKDIR "/src/tests/PaymentGateway.EndToEnd.Tests"
RUN dotnet build "./PaymentGateway.EndToEnd.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaymentGateway.EndToEnd.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM sdk-base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "test", "PaymentGateway.EndToEnd.Tests.dll"]