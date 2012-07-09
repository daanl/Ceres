using System;
using System.IO;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Ceres.BackupCreaters.Folder
{
    public class ZipFolderBackupCreater : IFolderBackupCreater
    {
        public Task<BackupResult> CreateBackup(string backupFilePath, string folderPath, string backupName)
        {
            return new Task<BackupResult>(() =>
            {
                if (Directory.Exists(folderPath) == false)
                {
                    throw new Exception(string.Format("Folder: {0} to backup doesn't exists", folderPath));
                }

                var zipPath = string.Format("{0}\\{1}_{2}.zip", backupFilePath, DateTime.Now.ToString("yyyyMMdd_HHmm"), backupName);

                using (var zip = new ZipFile())
                {
                  zip.AddDirectory(folderPath);
                  zip.Comment = "This zip was created at " + DateTime.Now.ToString("G") ;
                  zip.Save(zipPath);
                }

                return new BackupResult(zipPath, BackupType.Ftp);
            });
        }
    }
}
