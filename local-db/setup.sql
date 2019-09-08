RESTORE DATABASE [Linnworks.TestDb] FROM DISK = "/usr/config/Linnworks.TestDb.bak"
WITH MOVE "Linnworks.TestDb" to "/usr/config/Linnworks.TestDb.mdf", 
MOVE "Linnworks.TestDb_log" to "/usr/config/Linnworks.TestDb_log.ldf"
