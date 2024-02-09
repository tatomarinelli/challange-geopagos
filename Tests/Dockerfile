
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
#COPY ["Tests/Tests.csproj", "Tests/"]
#
#COPY ["PaymentProcessor/PaymentProcessor.csproj", "PaymentProcessor/"]
#COPY ["ClientAuthorization/ClientAuthorization.csproj", "ClientAuthorization/"]
#COPY ["ServiceMiddlewares/ServiceMiddlewares.csproj", "ServiceMiddlewares/"]
#COPY /PaymentProcessor/ .
COPY . .
WORKDIR /src/Tests
#COPY ["Tests/PaymentProcessor/Dataset/", "bin/Debug/net6.0/PaymentProcessor/Dataset/"]
RUN dotnet test Tests.csproj