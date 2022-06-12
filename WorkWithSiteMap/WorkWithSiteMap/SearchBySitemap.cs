using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WorkWithSiteMap
{
    public class SearchBySitemap:SearcherUrls
    {
        public List<string> sitemaps = new List<string>();
        public List<string> urls = new List<string>();
        public override List<string> Search(string url)
        {
            try
            {
                XmlDocument XDoc = new XmlDocument();
                XDoc.Load($"{url}/sitemap.xml");
                XmlElement? xRoot = XDoc.DocumentElement;
                if (xRoot != null)
                {
                    foreach (XmlElement xnode in xRoot)
                    {
                        if (xnode.Name == "sitemap")
                        {
                            sitemaps.Add(xnode.InnerText);
                        }
                    }
                }
                if (sitemaps.Count == 0) sitemaps.Add($"{url}/sitemap.xml");
                foreach (var sitemap in sitemaps)
                {
                    XmlDocument YDoc = new XmlDocument();
                    YDoc.Load(sitemap);
                    XmlElement? yRoot = YDoc.DocumentElement;
                    if (yRoot != null)
                    {
                        foreach (XmlElement ynode in yRoot)
                        {
                            foreach (XmlNode childnode in ynode.ChildNodes)
                            {
                                if (childnode.Name == "loc")
                                {
                                    if (childnode.InnerText.StartsWith("/"))
                                    {
                                        childnode.InnerText = url + childnode.InnerText;
                                    }
                                    if (childnode.InnerText.EndsWith("/"))
                                    {
                                        childnode.InnerText=childnode.InnerText.Remove(childnode.InnerText.Length - 1);
                                    }
                                    urls.Add(childnode.InnerText);
                                }
                            }
                        }
                    }
                }
            }
            catch
            { }          
            return urls;
        }
    }
}
