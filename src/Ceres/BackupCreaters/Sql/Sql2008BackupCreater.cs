using System;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Ceres.BackupCreaters.Sql
{
    public class Sql2008BackupCreater : ISqlBackupCreater
    {
        public Task<BackupResult> CreateBackup(string backupFilePath, string instanceName, string database, string username, string password)
        {
            return new Task<BackupResult>(() =>
            {
                var filePath = string.Format("{0}\\{1}_{2}.bak", backupFilePath, DateTime.Now.ToString("yyyyMMdd_HHmm"), database);

                var backupDeviceItem = new BackupDeviceItem(filePath, DeviceType.File);
                var backup = new Backup
                {
                    Database = database,
                    Initialize = true,
                    Incremental = false,
                    CopyOnly = true,
                    Action = BackupActionType.Database,
                };

                backup.Devices.Add(backupDeviceItem);

                var serverConnection = new ServerConnection(instanceName, username, password);
                var server = new Server(serverConnection);

                backup.PercentComplete += PercentComplete;

                backup.SqlBackup(server);

                return new BackupResult(filePath, BackupType.Sql);
            });
        }

        static void PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            var backup = (Backup) sender;

            Console.WriteLine("Backup progress: " + backup.Database + " "  + e.Percent + "%");
        }
    }
}
