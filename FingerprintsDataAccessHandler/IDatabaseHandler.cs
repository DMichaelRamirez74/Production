using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsDataAccessHandler
{
    public interface IDatabaseHandler
    {
        IDbConnection CreateConnection();

        void CloseConnection(IDbConnection connection);

        IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection);

        IDbDataParameter CreateParameter(IDbCommand command);

        IDataAdapter CreateAdapter(IDbCommand command);

        
    }
}
