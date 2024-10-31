FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.sln ./
COPY Hospital.Console/*.csproj Hospital.Console/
COPY Hospital.Presentation/*.csproj Hospital.Presentation/
COPY Hospital.Application/*.csproj Hospital.Application/
COPY Hospital.Persistence/*.csproj Hospital.Persistence/
COPY Hospital.Domain/*.csproj Hospital.Domain/
COPY Hospital.Tests/*.csproj Hospital.Tests/

RUN dotnet restore

COPY . ./

RUN dotnet publish  Hospital.Console/Hospital.Console.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Hospital.Console.dll"]