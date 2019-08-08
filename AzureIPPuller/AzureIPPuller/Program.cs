using System;
using System.IO;
using System.Net;
using System.Text;

namespace AzureIPPuller
{
    class Program
    {
        static void Main(string[] args)
        {
            // URL to download the IP ranges file
            string urlAddress = "https://www.microsoft.com/en-us/download/confirmation.aspx?id=41653";

            // Creating the HTTP request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // If it is able to find the web page
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Here I am creating the stream structure which is going to contain all the html page content
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                // This is going to be all the HTML content 
                string htmlContent = readStream.ReadToEnd();

                // Closing the stream
                response.Close();
                readStream.Close();

                // This is a NuGet packet which helps you to manage html strings and apply commands as Javascript
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(htmlContent);

                // There is an <a> </a> tag which contains in the href the link where the file located, so that is the file that we are going to download
                string href = document.GetElementbyId("c50ef285-c6ea-c240-3cc4-6c9d27067d6c").GetAttributeValue("href", "");

                var client = new WebClient();
                client.DownloadFile(href, @"C:\Users\jujimen\Desktop\MicrosoftAzureDatacenterIPRanges.xml");
            }

            Console.Read();
        }
    }
}
