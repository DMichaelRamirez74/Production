using System.Data.SqlClient;

namespace Fingerprints.SqlServerNotifier
{
    public delegate void SqlNotificationEventHandler(object sender, SqlNotificationEventArgs e);
}