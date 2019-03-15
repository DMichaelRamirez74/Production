using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace FingerprintsDataAccessHandler
{
    public class OledbDataAccess : IDatabaseHandler
    {
        private string ConnectionString { get; set; }

        public OledbDataAccess(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new OleDbConnection(ConnectionString);
        }

        public void CloseConnection(IDbConnection connection)
        {
            var oleDbConnection = (OleDbConnection)connection;
            oleDbConnection.Close();
            oleDbConnection.Dispose();
        }

        public IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection)
        {
            return new OleDbCommand
            {
                CommandText = commandText,
                Connection = (OleDbConnection)connection,
                CommandType = commandType
            };
        }

        public IDataAdapter CreateAdapter(IDbCommand command)
        {
            return new OleDbDataAdapter((OleDbCommand)command);
        }

        public IDbDataParameter CreateParameter(IDbCommand command)
        {
            OleDbCommand SQLcommand = (OleDbCommand)command;
            return SQLcommand.CreateParameter();
        }
    }
}
