
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Settings = QuickRestore.Properties.Settings;

namespace QuickRestore
{
    public class DatabaseRestore
    {
        //SMO Code http://www.mssqltips.com/sqlservertip/1849/backup-and-restore-sql-server-databases-programmatically-with-smo/

        private static readonly ManualResetEvent Sync = new ManualResetEvent(false);
        private static readonly SqlConnectionStringBuilder SqlConnectionStringBuilder = new SqlConnectionStringBuilder(string.Format("Server={0};Database=Master;Trusted_Connection=True;", Settings.Default.Server));
        private static SqlConnection _sqlConnection;
        private const string SetDatabaseSingleUserCommandText = "ALTER DATABASE {0} SET {1} WITH ROLLBACK IMMEDIATE";

        public static void Restore(string backupPath)
        {
            var restoreDb = CreateRestore(backupPath);

            var connection = SetSingleUser(true);
            var serverConnection = new ServerConnection(connection);
            var server = new Server(serverConnection);

            ProgressBar.SetupProgressBar();

            restoreDb.SqlRestoreAsync(server);

            Sync.WaitOne();

            SetSingleUser(false);

            Cleanup(restoreDb);
           
        }

        private static Restore CreateRestore(string backupPath)
        {
            var restoreDb = new Restore
            {
                Database = Settings.Default.DatabaseName,
                Action = RestoreActionType.Database
            };

            restoreDb.Devices.AddDevice(backupPath, DeviceType.File);
            restoreDb.ReplaceDatabase = true;

            restoreDb.PercentComplete += Restore_PercentComplete;
            restoreDb.Complete += Restore_Complete;

            return restoreDb;
        }

        private static void Cleanup(Restore restoreDb)
        {
            _sqlConnection.Dispose();
            restoreDb.PercentComplete -= Restore_PercentComplete;
            restoreDb.Complete -= Restore_Complete;
        }

        private static SqlConnection SetSingleUser(bool singleUser)
        {
            var commandText = string.Format(SetDatabaseSingleUserCommandText, Settings.Default.DatabaseName, singleUser ? "SINGLE_USER" : "MULTI_USER");

            if (_sqlConnection == null)
            {
                _sqlConnection = new SqlConnection(SqlConnectionStringBuilder.ToString());
            }

            using (var command = new SqlCommand(commandText, _sqlConnection))
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                command.ExecuteNonQuery();

                if (_sqlConnection.State == ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }

            return _sqlConnection;
        }

        static void Restore_Complete(object sender, ServerMessageEventArgs e)
        {
            Console.WriteLine(Environment.NewLine + "Restore Done!");
            Sync.Set();
        }

        static void Restore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            ProgressBar.IncrementProgressBar();
        }
    }
}
