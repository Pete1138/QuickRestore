
using System.IO;
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
        private static Settings _settings;

        internal static void Backup(Settings settings)
        {
            _settings = settings;

            var backupPath = Path.Combine(_settings.BackupFolder, _settings.GetBackupFileName());

            var backup = CreateBackup(backupPath);

            ProgressBar.SetupProgressBar("BACKUP " + _settings.DatabaseName);

            var server = new Server(_settings.Server);
            backup.SqlBackupAsync(server);

            Sync.WaitOne();

            Cleanup(backup);
        }

        private static Backup CreateBackup(string backupPath)
        {
            var backup = new Backup
            {
                Action = BackupActionType.Database,
                Database = _settings.DatabaseName
            };

            backup.Devices.AddDevice(backupPath, DeviceType.File);
            backup.BackupSetName = _settings.GetBackupName();
            backup.BackupSetDescription = _settings.GetBackupSetDesecription();

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
