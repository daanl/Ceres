namespace Ceres.BackupCreaters
{
    public class BackupResult
    {
        private readonly string _filePath;
        private readonly BackupType _backupType;

        public BackupResult(
            string filePath,
            BackupType backupType
        )
        {
            _filePath = filePath;
            _backupType = backupType;
        }

        public BackupType BackupType
        {
            get { return _backupType; }
        }

        public string FilePath
        {
            get { return _filePath; }
        }
    }
}
