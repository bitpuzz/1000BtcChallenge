# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj file and restore any dependencies
COPY BtcPuzzleSolver/BtcPuzzleSolver.csproj BtcPuzzleSolver/
RUN dotnet restore BtcPuzzleSolver/BtcPuzzleSolver.csproj

# Copy the entire project and build the app
COPY BtcPuzzleSolver/ BtcPuzzleSolver/
RUN dotnet publish BtcPuzzleSolver/BtcPuzzleSolver.csproj -c Release -o out

# Use the official .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .

# Command to run the application
ENTRYPOINT ["dotnet", "BtcPuzzleSolver.dll"]
