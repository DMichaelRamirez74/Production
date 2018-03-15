var OfflineConfiguration = {
    Db_VERSION: 1,
    DB_NAME: "FingerPrintsDb",
    WEBSERVICE_BASE_URL: "SERVICE URL",
    SCHEMA: {
        ClientAttendance: { key: { keyPath: 'UserID', autoIncrement: false, Unique: false } },
        DailyMealsAttendance:{ key: { keyPath: 'DailyID', autoIncrement: false, Unique: false } }
    }

};