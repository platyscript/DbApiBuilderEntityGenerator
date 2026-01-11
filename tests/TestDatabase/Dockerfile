FROM mcr.microsoft.com/mssql/server:2022-latest
ARG BAK_FILE
ARG SQL_FILE
ENV MSSQL_SA_PASSWORD=Test_123_Test
EXPOSE 1433
WORKDIR /
COPY $SQL_FILE ./Tracker.sql
COPY ./install.sh .
#COPY ./startup.sh .
#CMD /bin/bash ./startup.sh