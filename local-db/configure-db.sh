#!/usr/bin/env bash
STATUS=1
i=0

while [[ $STATUS -ne 0 ]] && [[ $i -lt 30 ]]; do
	i=$i+1
	/opt/mssql-tools/bin/sqlcmd -t 1 -U sa -P $SA_PASSWORD -Q "select 1" >> /dev/null
	STATUS=$?
done

if [ $STATUS -ne 0 ]; then
	echo "Error: MSSQL SERVER took more than thirty seconds to start up."
	exit 1
fi

echo "======= MSSQL SERVER STARTED ========" | tee -a ./config.log

# Run the setup script to create the DB
echo "setup required tables and sp"
/opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P $SA_PASSWORD -i setup.sql
