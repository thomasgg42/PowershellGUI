using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PsGui.ViewModels
    {
    class ActiveDirectoryInfo
        {
        public string TabName { get; } = "AD Info";

        private string _test;
        private string _test2;

        public string GetTest()
            {
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create("https://www.nrk.no");
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
   //         Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
           
            /*
            // Parse site
            var html = @"https://html-agility-pack.net/";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");
            return "Node Name: " + node.Name + "\n" + node.OuterHtml;
            */
            return "";
            }

        public string GetTest2()
            {
            string tmp = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://outlook.live.com/mail/inbox");
            request.CookieContainer = new CookieContainer();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            foreach(Cookie cook in response.Cookies)
                {
                tmp += cook + "\n";
                }
            return tmp;
            }

        public ActiveDirectoryInfo()
            {
            _test = GetTest();
            _test2 = GetTest2();
            }

        public string Test
            {
            get
                {
                return _test;
                }
            set
                {
                _test = value;
                }
            }

        public string Test2
            {
            get
                {
                return _test2;
                }
            set
                {
                _test2 = value;
                }
            }
        }
    }
