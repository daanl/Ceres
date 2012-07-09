using System;
using System.Configuration;
using Ceres.BackupCreaters.Sql;

namespace Ceres
{
    public class BackupConfiguration : ConfigurationSection
    {
        private static string _configurationSection = "backup";

        public static BackupConfiguration GetConfig()
        {
            return (BackupConfiguration)System.Configuration.ConfigurationManager.
               GetSection(BackupConfiguration._configurationSection) ??
               new BackupConfiguration();
        }

        [ConfigurationProperty("databases")]
        public Databases Databases
        {
            get
            {
                return (Databases)this["databases"] ?? new Databases();
            }
        }
        [ConfigurationProperty("folders")]
        public Folders Folders
        {
            get
            {
                return (Folders)this["folders"] ?? new Folders();
            }
        }
    }

    public class Folders : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Folder();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Folder) element).Name;
        }
    }

    public class Folder : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("backupDirectory", IsRequired = true)]
        public string BackupDirectory
        {
            get
            {
                return this["backupDirectory"] as string;
            }
        }

        [ConfigurationProperty("folderPath", IsRequired = true)]
        public string FolderPath
        {
            get
            {
                return this["folderPath"] as string;
            }
        }

        [ConfigurationProperty("deleteOlderThenDays", IsRequired = false)]
        public int? DeleteOlderThenDays
        {
            get
            {
                return this["deleteOlderThenDays"] as Int32?;
            }
        }

        [ConfigurationProperty("ftpBackup", IsRequired = false)]
        public bool? FtpBackup
        {
            get { return this["ftpBackup"] as bool?; }
        }

        [ConfigurationProperty("ftpHostname", IsRequired = false)]
        public string FtpHostname
        {
            get { return this["ftpHostname"] as string; }
        }

        [ConfigurationProperty("ftpUsername", IsRequired = false)]
        public string FtpUsername
        {
            get { return this["ftpUsername"] as string; }
        }

        [ConfigurationProperty("ftpPassword", IsRequired = false)]
        public string FtpPassword
        {
            get { return this["ftpPassword"] as string; }
        }
    }

    public class Databases : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Database();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Database)element).DatabaseName;
        }
    }

    public class Database : ConfigurationElement
    {
        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get
            {
                return this["username"] as string;
            }
        }
            
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return this["password"] as string;
            }
        }

        [ConfigurationProperty("database", IsRequired = true)]
        public string DatabaseName
        {
            get
            {
                return this["database"] as string;
            }
        }

        [ConfigurationProperty("serverType", IsRequired = true)]
        public ServerType ServerType
        {
            get
            {
                return (ServerType)this["serverType"]; ;
            }
        }

        [ConfigurationProperty("instance", IsRequired = true)]
        public string InstanceName
        {
            get
            {
                return this["instance"] as string;
            }
        }

        [ConfigurationProperty("backupDirectory", IsRequired = true)]
        public string BackupDirectory
        {
            get
            {
                return this["backupDirectory"] as string;
            }
        }

        [ConfigurationProperty("deleteOlderThenDays", IsRequired = false)]
        public int? DeleteOlderThenDays
        {
            get
            {
                return this["deleteOlderThenDays"] as Int32?;
            }
        }

        [ConfigurationProperty("filename", IsRequired = false)]
        public string FilenameFormat
        {
            get { return this["filename"] as string; }
        }

        [ConfigurationProperty("ftpBackup", IsRequired = false)]
        public bool? FtpBackup
        {
            get { return this["ftpBackup"] as bool?; }
        }

        [ConfigurationProperty("ftpHostname", IsRequired = false)]
        public string FtpHostname
        {
            get { return this["ftpHostname"] as string; }
        }

        [ConfigurationProperty("ftpUsername", IsRequired = false)]
        public string FtpUsername
        {
            get { return this["ftpUsername"] as string; }
        }

        [ConfigurationProperty("ftpPassword", IsRequired = false)]
        public string FtpPassword
        {
            get { return this["ftpPassword"] as string; }
        }
    }
}
