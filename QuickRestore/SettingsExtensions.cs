using QuickRestore.Properties;

namespace QuickRestore
{
    internal static class SettingsExtensions
    {
        internal static string GetBackupName(this Settings settings)
        {
            return string.Format(settings.BackupNameTemplate, settings.DatabaseName);
        }

        internal static string GetBackupFileName(this Settings settings)
        {
            return string.Format(settings.BackupFilenameTemplate, settings.DatabaseName);
        }

        internal static string GetBackupSetDesecription(this Settings settings)
        {
            return string.Format(settings.BackupSetDescriptionTemplate, settings.DatabaseName);
        }
    }
}
