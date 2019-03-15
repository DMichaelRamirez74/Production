using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsDataAccessHandler
{
    public class OracleDataAccess : IDatabaseHandler
    {
        private string ConnectionString { get; set; }

        public OracleDataAccess(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new OracleConnection(ConnectionString);
        }

        public void CloseConnection(IDbConnection connection)
        {
            var oracleConnection = (OracleConnection)connection;
            oracleConnection.Close();
            oracleConnection.Dispose();
        }

        public IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection)
        {
            return new OracleCommand
            {
                CommandText = commandText,
                Connection = (OracleConnection)connection,
                CommandType = commandType
            };
        }

        public IDataAdapter CreateAdapter(IDbCommand command)
        {
            return new OracleDataAdapter((OracleCommand)command);
        }

        public IDbDataParameter CreateParameter(IDbCommand command)
        {
            OracleCommand SQLcommand = (OracleCommand)command;
            return SQLcommand.CreateParameter();
        }
    }
}
