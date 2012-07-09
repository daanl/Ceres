using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ceres.BackupCreaters;
using Ceres.BackupCreaters.Folder;
using Ceres.BackupCreaters.Ftp;
using Ceres.BackupCreaters.Sql;

namespace Ceres
{
    class Program
    {
        static void Main()
        {
            var backupConfiguration = BackupConfiguration.GetConfig();

            foreach (Database databaseSettings in backupConfiguration.Databases)
            {
                try
                {
                    var backupCreater = SqlBackupCreaterFactory.GetInstance(databaseSettings.ServerType);

                    var backupTask = backupCreater.CreateBackup(
                        databaseSettings.BackupDirectory,
                        databaseSettings.InstanceName,
                        databaseSettings.DatabaseName,
                        databaseSettings.Username,
                        databaseSettings.Password
                    );

                    backupTask.Start();

                    if (databaseSettings.DeleteOlderThenDays.HasValue)
                    {
                        var filesToBeDeleted = Directory.GetFiles(databaseSettings.BackupDirectory)
                            .Select(x => new FileInfo(x))
                            .Where(x => IsFromDatabase(databaseSettings, x))
                            .Where(x => ShouldBeDeleted(databaseSettings, x))
                            .ToList();

                        filesToBeDeleted.ForEach(x => x.Delete());
                    }

                    backupTask.ContinueWith(x =>
                    {
                        if (databaseSettings.FtpBackup.HasValue && databaseSettings.FtpBackup.Value)
                        {
                            var ftpBackupper = new FtpBackupCreater();

                            var ftpBackupTask = ftpBackupper.CreateBackup(
                                x.Result.FilePath,
                                databaseSettings.FtpUsername,
                                databaseSettings.FtpPassword,
                                databaseSettings.FtpHostname
                            );

                            ftpBackupTask.Start();
                            ftpBackupTask.Wait();

                            return ftpBackupTask;
                        }
                        
                        return null;

                    }).Wait();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    File.WriteAllText("log.txt", exception.ToString());
                }
            }

            foreach (Folder folderSettings in backupConfiguration.Folders)
            {
                try
                {
                    var folderBackupCreater = new ZipFolderBackupCreater();

                    var backupTask = folderBackupCreater.CreateBackup(
                        folderSettings.BackupDirectory,
                        folderSettings.FolderPath,
                        folderSettings.Name
                    );

                    backupTask.Start();

                    if (folderSettings.DeleteOlderThenDays.HasValue)
                    {
                        var filesToBeDeleted = Directory.GetFiles(folderSettings.BackupDirectory)
                            .Select(x => new FileInfo(x))
                            .Where(x => IsFromFolder(folderSettings, x))
                            .Where(x => ShouldBeDeleted(folderSettings, x))
                            .ToList();

                        filesToBeDeleted.ForEach(x => x.Delete());
                    }

                    backupTask.ContinueWith(x =>
                    {
                        if (folderSettings.FtpBackup.HasValue && folderSettings.FtpBackup.Value)
                        {
                            var ftpBackupper = new FtpBackupCreater();

                            var ftpBackupTask = ftpBackupper.CreateBackup(
                                x.Result.FilePath,
                                folderSettings.FtpUsername,
                                folderSettings.FtpPassword,
                                folderSettings.FtpHostname
                            );

                            ftpBackupTask.Start();
                            ftpBackupTask.Wait();

                            return ftpBackupTask;
                        }

                        return null;

                    }).Wait();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    File.WriteAllText("log.txt", exception.ToString());
                }
            }

            Console.ReadLine();
        }

        private static bool IsFromDatabase(Database database, FileInfo fileInfo)
        {
            var regex = new Regex(string.Format(@"^[0-9]{{8}}_[0-9]{{4}}_{0}.bak$", database.DatabaseName));

            return regex.IsMatch(fileInfo.Name);
        }

        private static bool IsFromFolder(Folder folder, FileInfo fileInfo)
        {
            var regex = new Regex(string.Format(@"^[0-9]{{8}}_[0-9]{{4}}_{0}.zip", folder.Name));

            return regex.IsMatch(fileInfo.Name);
        }

        private static bool ShouldBeDeleted(Database database, FileInfo fileInfo)
        {
            return fileInfo.CreationTime < DateTime.Now.AddDays(-database.DeleteOlderThenDays.Value);
        }

        private static bool ShouldBeDeleted(Folder folder, FileInfo fileInfo)
        {
            return fileInfo.CreationTime < DateTime.Now.AddDays(-folder.DeleteOlderThenDays.Value);
        }
    }
}
