using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ceres.BackupCreaters.Ftp
{
    public class FtpBackupCreater : IFtpBackupCreater
    {
        public Task<BackupResult> CreateBackup(string filePath, string username, string password, string hostname)
        {
            return new Task<BackupResult>(() =>
            {
                var ftpHostname = string.Format("ftp://{0}/{1}", hostname, Path.GetFileName(filePath));

                var request = (FtpWebRequest)WebRequest.Create(ftpHostname);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential (username, password);
            
                var sourceStream = new StreamReader(filePath);
                var fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                
                sourceStream.Close();
                
                request.ContentLength = fileContents.Length;

                var requestStream = request.GetRequestStream();
                
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                var response = (FtpWebResponse)request.GetResponse();
    
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
    
                response.Close();

                return new BackupResult("", BackupType.Ftp);
            });
        }
    }
}
