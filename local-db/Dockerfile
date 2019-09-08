FROM mcr.microsoft.com/mssql/server:2017-CU14-ubuntu

# Create a config directory
RUN mkdir -p /usr/config
WORKDIR /usr/config

# Bundle config source
COPY . /usr/config

# Grant permissions for to our scripts to be executable
RUN chmod +x /usr/config/Linnworks.TestDb.bak
RUN chmod +x /usr/config/setup.sql
RUN chmod +x /usr/config/configure-db.sh
RUN chmod +x /usr/config/entrypoint.sh

CMD ["./entrypoint.sh"]
