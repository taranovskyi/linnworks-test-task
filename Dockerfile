FROM mcr.microsoft.com/dotnet/core/sdk:2.1 as build-env
WORKDIR /app
COPY . ./
# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
    apt-get install -y build-essential nodejs
RUN dotnet build LinnworksTest

FROM markadams/chromium-xvfb-js:8 AS frontend-unit-tests
WORKDIR /app
COPY --from=build-env /app/LinnworksTest .
WORKDIR /app/ClientApp
RUN ./node_modules/.bin/karma start

FROM build-env
RUN dotnet test ./Linnworks.UnitTests
CMD dotnet test ./Linnworks.IntegrationTests