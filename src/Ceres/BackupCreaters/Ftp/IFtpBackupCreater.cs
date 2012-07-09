using System.Threading.Tasks;

namespace Ceres.BackupCreaters.Ftp
{
    public interface IFtpBackupCreater
    {
        Task<BackupResult> CreateBackup(string filePath, string username, string password, string hostname);
    }
}
