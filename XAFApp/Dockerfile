FROM mcr.microsoft.com/dotnet/sdk:7.0

RUN apt-get update
RUN apt-get install -y wait-for-it libc6 libicu-dev libfontconfig1 libgdiplus

WORKDIR /src

COPY . ./

ARG DXNUGETKEY
RUN dotnet nuget add source https://nuget.devexpress.com/api -n DXFeed --store-password-in-clear-text -u DevExpress -p $DXNUGETKEY

ARG STARTSCRIPT
ENV STARTSCRIPT $STARTSCRIPT

CMD wait-for-it -t 0 sql:1433 -- $STARTSCRIPT
