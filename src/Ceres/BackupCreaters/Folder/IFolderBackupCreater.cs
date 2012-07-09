using System.Threading.Tasks;

namespace Ceres.BackupCreaters.Folder
{
    public interface IFolderBackupCreater
    {
        Task<BackupResult> CreateBackup(string backupFilePath, string folderPath, string backupName);
    }
}
