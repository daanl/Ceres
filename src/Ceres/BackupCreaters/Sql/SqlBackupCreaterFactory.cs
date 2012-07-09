using System.Collections.Generic;

namespace Ceres.BackupCreaters.Sql
{
    public class SqlBackupCreaterFactory
    {
        private static readonly IDictionary<ServerType, ISqlBackupCreater> _sqlBackupCreaters = new Dictionary<ServerType, ISqlBackupCreater>();

        static SqlBackupCreaterFactory()
        {
            _sqlBackupCreaters.Add(ServerType.SqlServer2008, new Sql2008BackupCreater());
        }

        public static ISqlBackupCreater GetInstance(ServerType serverType)
        {
            return _sqlBackupCreaters[serverType];
        }
    }
}
