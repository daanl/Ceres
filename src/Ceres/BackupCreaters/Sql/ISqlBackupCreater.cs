using System.Threading.Tasks;

namespace Ceres.BackupCreaters.Sql
{
    public interface ISqlBackupCreater
    {
        Task<BackupResult> CreateBackup(string backupFilePath, string instanceName, string database, string username, string password);
    }
}
