
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Threading;
using Settings = QuickRestore.Properties.Settings;

namespace QuickRestore
{
    public class DatabaseBackup
    {
        private static readonly ManualResetEvent Sync = new ManualResetEvent(false);

        public static void Backup(string backupPath)
        {
            var backup = CreateBackup(backupPath);

            ProgressBar.SetupProgressBar("BACKUP " + Settings.Default.DatabaseName);

            var server = new Server(Settings.Default.Server);
            backup.SqlBackupAsync(server);

            Sync.WaitOne();

            Cleanup(backup);
        }

        private static Backup CreateBackup(string backupPath)
        {
            var backup = new Backup
            {
                Action = BackupActionType.Database,
                Database = Settings.Default.DatabaseName
            };

            backup.Devices.AddDevice(backupPath, DeviceType.File);
            backup.BackupSetName = Settings.Default.BackupName;
            backup.BackupSetDescription = Settings.Default.BackupSetDescription;

            backup.Initialize = true;

            backup.PercentComplete += backup_PercentComplete;
            backup.Complete += backup_Complete;

            return backup;
        }

        private static void Cleanup(Backup backup)
        {
            backup.PercentComplete -= backup_PercentComplete;
            backup.Complete -= backup_Complete;    
        }

        private static void backup_Complete(object sender, ServerMessageEventArgs e)
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine("Backup Complete".PadBoth(ProgressBar.Bar.Length));
            Sync.Set();
        }

        private static void backup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            ProgressBar.IncrementProgressBar();
        }
    }
}
