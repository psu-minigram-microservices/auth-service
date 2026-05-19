FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH
WORKDIR /src

COPY Minigram.Auth/Minigram.Auth.csproj Minigram.Auth/
COPY Minigram.Core/Minigram.Core.csproj Minigram.Core/

RUN arch=$([ "$TARGETARCH" = "amd64" ] && echo "x64" || echo "$TARGETARCH") && \
    dotnet restore Minigram.Auth/Minigram.Auth.csproj -a $arch

COPY Minigram.Core/ Minigram.Core/
COPY Minigram.Auth/ Minigram.Auth/

WORKDIR /src/Minigram.Auth
RUN arch=$([ "$TARGETARCH" = "amd64" ] && echo "x64" || echo "$TARGETARCH") && \
    dotnet publish -c Release -o /app -a $arch

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Minigram.Auth.dll"]
