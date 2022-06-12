using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WorkWithSiteMap
{
    public class UrlsComparer
    {
        static public void ExceptBySitemap(List<string> first, List<string> second)
        {
            int count = 0;
            List<string> except = first.Except(second).ToList();
            Console.WriteLine("\n");
            Console.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            Console.WriteLine("Url");
            if (except.Count()==0)
            {
                Console.WriteLine("Sitemap was not found ");
            }
            else
            {
                foreach (var item in except)
                {
                    Console.WriteLine(++count + ") " + item);
                }
            }
        }
        static public void ExceptyByCrawling(List<string> first, List<string> second)
        {
            int count = 0;
            List<string> except = second.Except(first).ToList();
            Console.WriteLine("\n");
            Console.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            foreach (var item in except)
            {
                Console.WriteLine(++count + ") " + item);
            }
        }
        static public void Union(List<string> first, List<string> second)
        {
            int sitemapCount = first.Count();
            int crawlingCount = second.Count();
            int count = 0;
            List<string> union = first.Union(second).ToList();
            Dictionary<string, long> unionTiming = new Dictionary<string, long>();
            foreach (var unionUrl in union)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(unionUrl);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                stopWatch.Stop();
                unionTiming.Add(unionUrl, stopWatch.ElapsedMilliseconds);
            }
            Console.WriteLine("\n");
            Console.WriteLine("Timing");
            Console.WriteLine("Url\t\t\t\t\t\t\t\t\tTiming (ms)");
            unionTiming = unionTiming.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var siteUrl in unionTiming)
            {
                Console.WriteLine(++count + ") " + siteUrl.Key + "----->" + siteUrl.Value + "ms");
            }
            Console.WriteLine("Urls(html documents) found after crawling a website: " + sitemapCount);
            Console.WriteLine("Urls found in sitemap: " + crawlingCount);
        }
    }
}
