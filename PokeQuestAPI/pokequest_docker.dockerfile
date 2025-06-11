# Build stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["PokeQuestAPI/PokeQuestAPI.csproj", "PokeQuestAPI/"]
RUN dotnet restore "PokeQuestAPI/PokeQuestAPI.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "PokeQuestAPI/PokeQuestAPI.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copy build output
COPY --from=build /app/publish .

# Entry point (use dotnet command for better signal handling)
ENTRYPOINT ["dotnet", "PokeQuestAPI.dll"]