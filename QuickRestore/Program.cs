using QuickRestore.Properties;
using System;
using System.IO;

namespace QuickRestore
{
    public class Program
    {
        public static string BackupPath;

        private static void Main(string[] args)
        {
            try
            {
                RunProgram(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error Occurred:");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }

                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void RunProgram(string[] args)
        {

            CheckBackupFolderExists();

            var settings = Settings.Default;

            //TODO parse args into a dictionary of <string>/List<string>, e.g. "-b"/"SlatePpi", "-?"/"","-r"/"SlatePpi"/"SlatePpi2.bak"
            
            if (args.Length >= 1)
            {

                // Optional database name
                if (args.Length >= 2)
                {
                    if (args[1].Contains("?"))
                    {
                        PrintHelpAndExit(settings);
                    }
                    
                    settings.DatabaseName = args[1];

                    if (args.Length == 3)
                    {
                        settings.RestoreFilename = args[2];
                    }
                }

                if (args[0].Contains("b"))
                {
                    DatabaseBackup.Backup(settings);
                }
                else if (args[0].Contains("r"))
                {
                    DatabaseRestore.Restore(settings);
                }
                else
                {
                    PrintHelpAndExit(settings);
                }

            }
            else
            {
                PrintHelpAndExit(settings);
            }

        }

        private static void PrintHelpAndExit(Settings settings)
        {
            Console.WriteLine(@"    -b  <databasename> : perform backup on <databasename>(optional - defaults to {0})",settings.DatabaseName);
            Console.WriteLine(@"    -r  <databasename> <filename>: perform restore of <databasename>(optional) (from filename)");
            Environment.Exit(0);
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
