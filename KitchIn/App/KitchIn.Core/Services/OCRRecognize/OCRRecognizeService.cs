﻿using System.Configuration;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace KitchIn.Core.Services.OCRRecognize
{
    using System.Collections.Generic;

    public class OCRRecognizeService
    {
        public OCRRecognizeService(MemoryStream stream)
        {
            ImgStream = stream;
        }

        /// <summary>
        /// User applicationId
        /// </summary>
        private static string ApplicationId
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplicationLingvoId"];
            }
        }

        /// <summary>
        /// User password
        /// </summary>
        private static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["PasswordLingvo"];
            }
        }

        private static MemoryStream ImgStream { get; set; }

        /// <summary>
        /// Network credentials
        /// </summary>
        private ICredentials Credentials
        {
            get
            {
                return this.credentials ??
                       (this.credentials =
                        string.IsNullOrEmpty(ApplicationId) || string.IsNullOrEmpty(Password)
                            ? CredentialCache.DefaultNetworkCredentials
                            : new NetworkCredential(ApplicationId, Password));
            }
        }

        /// <summary>
        /// Network proxy
        /// </summary>
        private IWebProxy Proxy
        {
            get
            {
                if (this.proxy == null)
                {
                    this.proxy = WebRequest.DefaultWebProxy;
                    this.proxy.Credentials = CredentialCache.DefaultCredentials;
                }

                return this.proxy;
            }
        }

        /// <summary>
        /// Gets file processing result, specified by provided parameters, and returns it as downloadable resource
        /// </summary>
        /// <remarks>Language and export formats specification can be obtained from "http://ocrsdk.com/documentation/apireference/processImage/"</remarks>
        public string[] GetResult()
        {
            // Specifying new post request filling it with file content
            var url = "http://cloud.ocrsdk.com/processImage?language=English&exportFormat=txt";
            List<string> srings = new List<string>();
            var request = CreateRequest(url, "POST", this.Credentials, this.Proxy);
            FillRequestWithContent(request);

            // Getting task id from response
            var response = GetResponse(request);
            var taskId = GetTaskId(response);

            // Checking if task is completed and downloading result by provided url
            url = string.Format("http://cloud.ocrsdk.com/getTaskStatus?taskId={0}", taskId);
            var resultUrl = string.Empty;
            var status = string.Empty;
            while (status != "Completed")
            {
                System.Threading.Thread.Sleep(1000);
                request = CreateRequest(url, "GET", this.Credentials, this.Proxy);
                response = GetResponse(request);
                status = GetStatus(response);
                resultUrl = GetResultUrl(response);
            }

            request = (HttpWebRequest)WebRequest.Create(resultUrl);
            using (var result = request.GetResponse())
            {
                using (var stream = result.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var item = reader.ReadLine();
                            srings.Add(item);
                        }
                    }
                }
            }
            return srings.ToArray();
        }

        private static void copyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        private static void SaveStreamToFile(string filepath, Stream stream)
        {
            if (stream.Length == 0)
            {
                return;
            }

            // Create a FileStream object to write a stream to a file
            using (var fileStream = File.Create(filepath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                var bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }

        /// <summary>
        /// Creates new request with defined parameters
        /// </summary>
        private static HttpWebRequest CreateRequest(string url, string method, ICredentials credentials, IWebProxy proxy)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/octet-stream";
            request.Credentials = credentials;
            request.Method = method;
            request.Proxy = proxy;
            return request;
        }

        /// <summary>
        /// Adds content from local file to request stream
        /// </summary>
        private static void FillRequestWithContent(WebRequest request)
        {
            //var filepath = HttpContext.Current.Server.MapPath("~/Content/temp/2013-11-03_13.08.26.jpg");
            ImgStream.Position = 0;
            using (var reader = new BinaryReader(ImgStream)) //File.OpenRead(filepath)
            {
                request.ContentLength = reader.BaseStream.Length;
                using (var stream = request.GetRequestStream())
                {
                    var buffer = new byte[reader.BaseStream.Length];
                    while (true)
                    {
                        var bytesRead = reader.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        stream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        /// <summary>
        /// Gets response xml document
        /// </summary>
        private static XDocument GetResponse(WebRequest request)
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    return XDocument.Load(new XmlTextReader(stream));
                }
            }
        }

        /// <summary>
        /// Gets file processing task id from response document
        /// </summary>
        private static string GetTaskId(XDocument doc)
        {
            var id = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                id = task.Attribute("id").Value;
            }

            return id;
        }

        /// <summary>
        /// Gets task's processing status from response document
        /// </summary>
        private static string GetStatus(XDocument doc)
        {
            var status = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                status = task.Attribute("status").Value;
            }

            return status;
        }

        /// <summary>
        /// Gets result url to download from response document
        /// </summary>
        /// <remarks> Result url will be available only after task status set to "Complete"</remarks>
        protected static string GetResultUrl(XDocument doc)
        {
            var resultUrl = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                resultUrl = task.Attribute("resultUrl") != null ? task.Attribute("resultUrl").Value : string.Empty;
            }

            return resultUrl;
        }

        private IWebProxy proxy;
        private ICredentials credentials;
    }
}