#!/usr/bin/env bash

echo "starting database setup"
/opt/mssql/bin/sqlservr & /usr/config/configure-db.sh & bash
