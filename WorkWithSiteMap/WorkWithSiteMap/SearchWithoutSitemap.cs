using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WorkWithSiteMap
{
    public class SearchWithoutSitemap:SearcherUrls
    {
        public List<string> tempUrls = new List<string>();
        public List<string> exceptionUrls = new List<string>();
        public List<string> urls = new List<string>();
        public int counter = 0;

        public SearchWithoutSitemap(string url)
        {
            tempUrls.Add(url);
        }
        public override List<string> Search(string url)
        {
            string htmlText = "";
            string thisUrl = url;
            Uri uri = new Uri(url);
            string[] smg = null;
            try
            {
                WebRequest request = WebRequest.Create(thisUrl);
                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (htmlText.Contains("<!DOCTYPE html") || htmlText == "")
                            {
                                htmlText += line;
                                if (line.Contains("<a"))
                                {
                                    string[] parts = line.Split(" ");
                                    smg = parts.Select(p => p).Where(p => p.Contains("href=\"" + tempUrls[0]) || p.Contains("href=\"/")).ToArray();
                                    foreach (string s in smg)
                                    {
                                        string test = s.Remove(0, 5).Replace('"', ' ').Trim();
                                        if (test.Contains('>'))
                                        {
                                            test = test.Remove(test.IndexOf('>')).Trim();
                                        }
                                        try
                                        {
                                            if (test.StartsWith("/") && !test.StartsWith("//"))
                                            {
                                                uri = new Uri(tempUrls[0] + test.Remove(0, 1));
                                            }
                                            else
                                            {
                                                uri = new Uri(test);
                                            }
                                            if (!tempUrls.Contains(uri.AbsoluteUri) && uri.AbsoluteUri != tempUrls[0] + "/")
                                            {
                                                tempUrls.Add(uri.AbsoluteUri);
                                            }
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                            else
                            {
                                tempUrls.Remove(url);
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                exceptionUrls.Add(thisUrl);
            }
            counter++;
            if (tempUrls.Count > counter)
            {
                return Search(tempUrls[counter]);
            }
            else
            {
                tempUrls = tempUrls.Except(exceptionUrls).ToList();
                foreach(var s in tempUrls)
                {
                    if (s.EndsWith("/"))
                    {
                        urls.Add(s.Remove(s.Length - 1));
                    }
                    else urls.Add(s);
                }
                return urls;
            }
        }
    }
}
