#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Guet.Blog/Guet.Blog.csproj", "Guet.Blog/"]
RUN dotnet restore "Guet.Blog/Guet.Blog.csproj"
COPY . .
WORKDIR "/src/Guet.Blog"
RUN dotnet build "Guet.Blog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Guet.Blog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Guet.Blog.dll"]