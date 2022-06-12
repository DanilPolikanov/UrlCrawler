using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
namespace WorkWithSiteMap
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter web site url");
            string url = Console.ReadLine();
            try
            {
                Uri uri = new Uri(url);
                url = "https://" + uri.Host.ToString();
                SearchWithoutSitemap searchWithoutSitemap = new SearchWithoutSitemap(uri.AbsoluteUri);
                SearchBySitemap searchBySitemap = new SearchBySitemap();
                var sitemap = searchBySitemap.Search(url);
                var allUrls = searchWithoutSitemap.Search(url);
                if (sitemap != null && allUrls != null)
                {
                    UrlsComparer.ExceptBySitemap(sitemap, allUrls);
                    UrlsComparer.ExceptyByCrawling(sitemap, allUrls);
                    UrlsComparer.Union(sitemap, allUrls);
                }
                else Console.WriteLine("This web site does not exist or there is insufficient authority to access it");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
