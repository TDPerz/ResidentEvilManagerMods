using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ResidentEvilManagerMods.Driver
{
    class GoogleDrive
    {
        private static string[] Scopes = { DriveService.Scope.DriveReadonly };
        private static string ApplicationName = "Re4Moder";
        private UserCredential credential;
        private Hashtable pages = new Hashtable();
        private DriveService service;
        private string updateID;

        public GoogleDrive()
        {
            using (var stream = new FileStream(@"data\Credencial\credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = @"data\token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            int i = 0;

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    switch ((file.Name.Split('-')[0]))
                    {
                        case "Mods":
                            pages.Add(i, file);
                            i++;
                            break;
                        case "Update":
                            updateID = file.Id;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
        }

        public string getPage(string id)
        {
            Console.WriteLine("Id: " + id);
            var request = service.Files.Get(id);
            string miJson = "";
            var stream = new System.IO.MemoryStream();
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            using (System.IO.FileStream file = new System.IO.FileStream("data/pageTemp.json", System.IO.FileMode.Create, System.IO.FileAccess.Write))
                            {
                                stream.WriteTo(file);
                            }
                            string json = File.ReadAllText("data/pageTemp.json");
                            File.Delete("data/pageTemp.json");
                            Console.WriteLine("Download complete.");
                            miJson = json;
                            break;

                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
            Console.WriteLine("Descargado bien");
            return miJson;
        }

        public string getUpdate()
        {
            var request = service.Files.Get(updateID);
            string miJson = "";
            var stream = new System.IO.MemoryStream();
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            using (System.IO.FileStream file = new System.IO.FileStream("data/update.json", System.IO.FileMode.Create, System.IO.FileAccess.Write))
                            {
                                stream.WriteTo(file);
                            }
                            string json = File.ReadAllText("data/update.json");
                            Console.WriteLine("Download complete.");
                            miJson = json;
                            break;

                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
            Console.WriteLine("Descargado bien");
            return miJson;
        }

        public Hashtable getTable() { return pages; }

        public DriveService getService() { return service; }
    }
}
