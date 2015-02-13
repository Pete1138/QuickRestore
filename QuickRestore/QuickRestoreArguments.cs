using System.Collections.Generic;
using System.Linq;

namespace QuickRestore
{
    internal class QuickRestoreArguments
    {
        public string FilePath { get; private set; }
        public bool HasFilePath { get; private set; }

        public string DatabaseName { get; private set; }
        public bool HasDatabaseName { get; private set; }

        public bool PerformBackupOperation { get; private set; }

        public bool HasHelpArgument { get; private set; }
        
        public QuickRestoreArguments(IList<string> args)
        {
            var argsList = args.ToList();

            if (argsList.Any(x => x.Contains("?")))
            {
                HasHelpArgument = true;
                return;
            }

            switch (args.Count)
            {
                case 3:
                    FilePath = args[2];
                    HasFilePath = true;
                    break;
                case 2:
                    DatabaseName = args[1];
                    HasDatabaseName = true;
                    break;
            }

            PerformBackupOperation = args[0].Contains("b");
            
        }
      
    }
}
