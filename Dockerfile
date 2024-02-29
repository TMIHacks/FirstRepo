FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 2222

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ConeConnect.Calendar.API/nuget.config", "src/ConeConnect.Calendar.API/"]
COPY ["nuget.config", "."]
COPY ["src/ConeConnect.Calendar.API/ConeConnect.Calendar.API.csproj", "src/ConeConnect.Calendar.API/"]
COPY ["src/ConeConnect.Calendar.Core/ConeConnect.Calendar.Core.csproj", "src/ConeConnect.Calendar.Core/"]
COPY ["src/ConeConnect.Calendar.SharedKernel/ConeConnect.Calendar.SharedKernel.csproj", "src/ConeConnect.Calendar.SharedKernel/"]
COPY ["src/ConeConnect.Calendar.Infrastructure/ConeConnect.Calendar.Infrastructure.csproj", "src/ConeConnect.Calendar.Infrastructure/"]
RUN dotnet restore "src/ConeConnect.Calendar.API/ConeConnect.Calendar.API.csproj"
COPY . .
WORKDIR "/src/src/ConeConnect.Calendar.API"
RUN dotnet build "ConeConnect.Calendar.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
 
FROM build AS publish
RUN dotnet publish "ConeConnect.Calendar.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
 
FROM base AS final
WORKDIR /app
 
COPY init_container.sh .
COPY sshd_config /etc/ssh/
# Start and enable SSH
RUN apt-get update \
    && apt-get install -y --no-install-recommends dialog openssh-server \
    && echo "root:Docker!" | chpasswd \
    && chmod u+x init_container.sh \
    && service ssh start \
    && update-rc.d ssh enable \
    && apt-get clean && rm -rf /var/lib/apt/lists/*
 
COPY --from=publish /app/publish .
ENTRYPOINT ["/app/init_container.sh"]
