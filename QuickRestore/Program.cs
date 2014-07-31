using QuickRestore.Properties;
using System;
using System.IO;

namespace QuickRestore
{
    public class Program
    {
        public static string BackupPath;

        static void Main(string[] args)
        {
            CheckBackupFolderExists();

            BackupPath = Path.Combine(Settings.Default.BackupFolder, Settings.Default.BackupFilename);

            if (args.Length == 1)
            {
                if (args[0].Contains("?"))
                {
                    Console.WriteLine(@"Any parameter (apart from '?') performs a backup, otherwise perform a restore");
                    Environment.Exit(0);
                }
                
                DatabaseBackup.Backup(BackupPath);
                
            }
            else
            {
                DatabaseRestore.Restore(BackupPath);
            }

           // Console.ReadLine();
        }

        private static void CheckBackupFolderExists()
        {
            if (Directory.Exists(Settings.Default.BackupFolder)) return;

            Console.WriteLine("The backup folder '{0}' does not exist", Settings.Default.BackupFolder);
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
