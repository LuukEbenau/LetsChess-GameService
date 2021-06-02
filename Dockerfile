#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
ARG PROJ_NAME=LetsChess-GameService

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
expose 80
expose 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG PROJ_NAME

WORKDIR /src
COPY ["./$PROJ_NAME/$PROJ_NAME.csproj", "./$PROJ_NAME/$PROJ_NAME.csproj"]

COPY ["shared", "./shared"]

RUN ls 
RUN dotnet restore "./$PROJ_NAME/$PROJ_NAME.csproj"
COPY ./$PROJ_NAME ./$PROJ_NAME

WORKDIR "/src/."
RUN dotnet build "./$PROJ_NAME/$PROJ_NAME.csproj" -c Release -o /app/build

FROM build AS publish
ARG PROJ_NAME

RUN dotnet publish "./$PROJ_NAME/$PROJ_NAME.csproj" -c Release -o /app/publish

FROM base AS final


WORKDIR /app
COPY --from=publish /app/publish .
ARG PROJ_NAME
ENV PROJ=${PROJ_NAME}
ENTRYPOINT ["sh","-c","dotnet ./${PROJ}.dll"]
#ENTRYPOINT ["dotnet" ,"./LetsChess-Backend.dll"]