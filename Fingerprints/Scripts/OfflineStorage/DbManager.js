if (navigator.userAgent.indexOf("Safari") >= 0 && navigator.userAgent.indexOf("Chrome") < 0) {
    var DataBaseManager = {
        Offlinedb: openDatabase(OfflineConfiguration.DB_NAME, '1', 'FingerPrints Database', 100 * 1024 * 1024),
        initializeDataBase: function () {
            var self = this;
            self.Offlinedb.transaction(function (tx) {
               
                tx.executeSql('CREATE TABLE IF NOT EXISTS ClientAttendance (UserID, ClientID, AttendanceType,AttendanceDate,PSignatureIn,SignedInBy,TimeIn,TimeOut,PSignatureOut,SignedOutBy,TSignatureIn,TSignatureOut,BreakFast,Lunch,Snacks,AbsenceReasonId,CenterID,ClassroomID)');
                tx.executeSql('CREATE TABLE IF NOT EXISTS DailyMealsAttendance(DailyID,AdultBreakFast,AdultLunch,AdultSnacks,AttendanceDate,ChildBreakFast,ChildExcused,ChildLunch,ChildPresent,ChildSnacks,ChildUnExcused,CenterID,ClassroomID)');
               
            });
        },
        InsertIntoTable: function (data, callback) {
           
            this.initializeDataBase();
            var self = this;
          
            self.Offlinedb.transaction(function (tx) {
                var query = "INSERT INTO ClientAttendance(UserID, ClientID, AttendanceType,AttendanceDate,PSignatureIn,SignedInBy,TimeIn,TimeOut,PSignatureOut,SignedOutBy,TSignatureIn,TSignatureOut,BreakFast,Lunch,Snacks,AbsenceReasonId,CenterID,ClassroomID) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                tx.executeSql(query, [data.UserID, data.ClientID, data.AttendanceType, data.AttendanceDate, data.PSignatureIn, data.SignedInBy,data.TimeIn,data.TimeOut,data.PSignatureOut,data.SignedOutBy,data.TSignatureIn,data.TSignatureOut,data.BreakFast, data.Lunch, data.Snacks,data.AbsenceReasonId,data.CenterID,data.ClassroomID], function (tx, results) {
                    if (callback) callback("User Saved");
                });
            });


        },
        GetSingleClient: function (data, clientID, attendanceDate, callback, element) {
            try {
                var self = this;
                this.initializeDataBase();
                self.Offlinedb.transaction(function (tx) {
                    //var query1 = "SELECT * from ClientAttendance where UserID=" + data + " and ClientID=" + clientID + "  and AttendanceDate=" + attendanceDate + "";
                    var query1 = "SELECT * from ClientAttendance where UserID=? and ClientID=?  and AttendanceDate=?";
                    tx.executeSql(query1, [data,clientID,attendanceDate], function (tx, results) {
                       
                        if (results.rows.length > 0) {
                            var arr = [];
                            var v = results.rows.item(0);
                           
                            arr.push(v);
                            if (callback) callback(arr, element);
                        } else {
                          
                            if (callback) callback(results.rows, element);
                        }
                    },
                    function (tx, error) {
                        console.log(error.message);
                    });
                });
                   // callback('', element);

              

            }
            catch (e) {
                console.log(" error occured in selecting data");
            }

        },
        GetAllClient: function (callback) {
            try {
                var self = this;
                this.initializeDataBase();
                var query1 = "SELECT * from ClientAttendance";
                self.Offlinedb.transaction(function (tx) {
                    tx.executeSql(query1, [], function (tx, results) {
                        var data = [];
                        if (results.rows.length > 0) {
                            for (var i = 0; i < results.rows.length; i++) {
                                data.push(results.rows.item(i));
                            }
                        }
                        if (callback) callback(data);
                    });

                });

            }
            catch (e) {
                console.log(" error occured in selecting data");
            }

        },
        UpdateUser: function (data, callback) {
            try {
                this.initializeDataBase();
                var self = this;
                var query1 = "Update ClientAttendance SET ClientID=?, AttendanceType=?,AttendanceDate=?,PSignatureIn=?,SignedInBy=?,TimeIn=?,TimeOut=?,PSignatureOut=?,SignedOutBy=?,TSignatureIn=?,TSignatureOut=?,BreakFast=?,Lunch=?,Snacks=?,AbsenceReasonId=?,CenterID=?,ClassroomID=? WHERE UserID=?";
                self.Offlinedb.transaction(function (tx) {
                   
                    tx.executeSql(query1, [data.ClientID, data.AttendanceType, data.AttendanceDate, data.PSignatureIn, data.SignedInBy, data.TimeIn, data.TimeOut, data.PSignatureOut, data.SignedOutBy, data.TSignatureIn, data.TSignatureOut, data.BreakFast, data.Lunch, data.Snacks,data.AbsenceReasonId,data.CenterID,data.ClassroomID,data.UserID], function (tx, results) {
                        if (callback) callback("Response updated");
                    });
                });

            }
            catch (e) {
                console.log(" error occured in selecting data");
            }
        },
        DeleteUser: function (data, callback) {
            try {
                this.initializeDataBase();
                var self = this;
                self.Offlinedb.transaction(function (tx) {
                    tx.executeSql("Delete from ClientAttendance where UserID=?", [data], function (tx, results) {
                        if (callback) callback("Data deleted");
                    });
                });

            }
            catch (e) {
            }
        },
        InsertDailyMealsAttendance: function (data, callback) {
           
            this.initializeDataBase();
            var self = this;
            self.Offlinedb.transaction(function (tx) {
                var query = "INSERT INTO DailyMealsAttendance(DailyID,AdultBreakFast,AdultLunch,AdultSnacks,AttendanceDate,ChildBreakFast,ChildExcused,ChildLunch,ChildPresent,ChildSnacks,ChildUnExcused,CenterID,ClassroomID) values(?,?,?,?,?,?,?,?,?,?,?,?,?)";
                tx.executeSql(query, [data.DailyID, data.AdultBreakFast, data.AdultLunch, data.AdultSnacks, data.AttendanceDate, data.ChildBreakFast, data.ChildExcused, data.ChildLunch,data.ChildPresent, data.ChildSnacks, data.ChildUnExcused,data.CenterID,data.ClassroomID], function (tx, results) {
                    if (callback) callback("User Saved");
                });
            });


        },

        GetDailyMeals: function (data, attendanceDate, callback, element) {
            try {
                var self = this;
                this.initializeDataBase();
                var query1 = "SELECT * from DailyMealsAttendance where DailyID=? and AttendanceDate=?";
                self.Offlinedb.transaction(function (tx) {
                    tx.executeSql(query1, [data,attendanceDate], function (tx, results) {
                        var data = [];
                        if (results.rows.length > 0) {
                            
                            var v = results.rows.item(0);
                            data.push(results.rows.item(0));
                           
                            if (callback) callback(data, element);
                        } else {
                           
                            if (callback) callback(data, element);
                        }

                        //if (results.rows.length > 0) {
                        //    for (var i = 0; i < results.rows.length; i++) {
                        //        data.push(results.rows.item(i));
                        //    }
                        //}

                        //if (callback) callback(data,element);
                    });


                });

            }
            catch (e) {
                console.log(" error occured in selecting data");
            }

        },

        GetAllMeals: function (callback, weeklyData) {
            try {
                var self = this;
                this.initializeDataBase();
                var query1 = "SELECT * from DailyMealsAttendance";
                self.Offlinedb.transaction(function (tx) {
                    tx.executeSql(query1, [], function (tx, results) {
                        var data = [];
                        if (results.rows.length > 0) {
                            for (var i = 0; i < results.rows.length; i++) {
                                data.push(results.rows.item(i));
                            }
                        }
                        if (callback) callback(weeklyData, data);
                    });

                });

            }
            catch (e) {
                console.log(" error occured in selecting data");
            }

        },

        UpdateDailyMealsAttendance: function (data, callback) {
            try {
                this.initializeDataBase();
                var self = this;
                var query1 = "Update DailyMealsAttendance SET AdultBreakFast=?, AdultLunch=?,AdultSnacks=?,AttendanceDate=?,ChildBreakFast=?,ChildExcused=?,ChildLunch=?,ChildPresent=?,ChildSnacks=?,ChildUnExcused=?,CenterID=?,ClassroomID=? WHERE DailyID=?";
                self.Offlinedb.transaction(function (tx) {
                    tx.executeSql(query1, [data.AdultBreakFast, data.AdultLunch, data.AdultSnacks, data.AttendanceDate, data.ChildBreakFast, data.ChildExcused, data.ChildLunch, data.ChildPresent, data.ChildSnacks, data.ChildUnExcused,data.CenterID,data.ClassroomID,data.DailyID], function (tx, results) {
                        if (callback) callback("Daily Meals/Attendance updated to DailyAttendance");
                    });
                });

            }
            catch (e) {
                console.log(" error occured in selecting data");
            }
        },

        DeleteMeals: function (data, callback) {
            try {
                this.initializeDataBase();
                var self = this;
                self.Offlinedb.transaction(function (tx) {
                    tx.executeSql("Delete from DailyMealsAttendance where DailyID=?", [data], function (tx, results) {
                        if (callback) callback("Data deleted");
                    });
                });

            }
            catch (e) {
            }
        }
    } //Db manager end

} else {
    var DataBaseManager = {
       
        InsertIntoTable: function (data, callback) {
          
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
              
                self.Server = s;
                self.Server.ClientAttendance.add(data).done(function (results) {
                    if (callback)
                        callback("Data added into UserData");
                });
            });
        },
        GetSingleClient: function (data, clientID, attendanceDate, callback, element) {
            
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s
                self.Server.ClientAttendance
                       .query()
                        .all()
                        .filter(function (result) {
                            return (result.UserID === data && result.ClientID === clientID && result.AttendanceDate === attendanceDate);
                        })
                        .execute()
                        .done(function (finalResult) {
                           
                            if (callback) callback(finalResult, element);
                            return finalResult;
                        });
            });

        },
        GetAllClient: function (callback) {
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s
                self.Server.ClientAttendance
                       .query()
                        .all()
                        .execute()
                        .done(function (finalResult) {

                            if (callback) callback(finalResult);
                            return finalResult;
                        });
            });

        },
        UpdateUser: function (data, callback) {
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s
                self.Server.ClientAttendance.update(data).done(function (item) {
                    if (callback) callback("response updated to database");
                });
            });
        },
        DeleteUser: function (data, callback) {
            var self = this;
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s
                self.Server.ClientAttendance.remove(data).done(function (a) {
                    if (callback) callback("Data deleted");
                });
            });
        },
        InsertDailyMealsAttendance: function (data, callback) {
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s;
                self.Server.DailyMealsAttendance.add(data).done(function (results) {
                    if (callback) callback("Daily Meals/Attendance added into DailyAttendance");
                });
            });
        },
        UpdateDailyMealsAttendance: function (data, callback) {
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s
                self.Server.DailyMealsAttendance.update(data).done(function (item) {
                    if (callback) callback("Daily Meals/Attendance updated to DailyAttendance");
                });
            });
        },
        GetDailyMeals: function (data, attendanceDate, callback, element) {
           
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s
                self.Server.DailyMealsAttendance
                       .query()
                        .all()
                        .filter(function (result) {
                            return (result.DailyID === data && result.AttendanceDate === attendanceDate);
                        })
                        .execute()
                        .done(function (finalResult) {
                           
                            if (callback) callback(finalResult, element);
                            return finalResult;
                        });
            });

        },
        GetAllMeals: function (callback,weeklyData) {
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s
                self.Server.DailyMealsAttendance
                       .query()
                        .all()
                        .execute()
                        .done(function (finalResult) {

                            if (callback) callback(weeklyData,finalResult);
                            return finalResult;
                        });
            });

        },
        DeleteMeals: function (data, callback) {
            var self = this;
            db.open({
                server: OfflineConfiguration.DB_NAME,
                version: OfflineConfiguration.Db_VERSION,
                schema: OfflineConfiguration.SCHEMA

            }).done(function (s) {
                self.Server = s
                self.Server.DailyMealsAttendance.remove(data).done(function (a) {
                    if (callback) callback("Data deleted");
                });
            });
        }


    } // DB manage end

}  //Else block end