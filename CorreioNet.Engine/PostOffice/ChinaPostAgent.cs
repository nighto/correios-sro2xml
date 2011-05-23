using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using CorreioNet.Engine.Entities;

namespace CorreioNet.Engine.PostOffice
{
    public class ChinaPostAgent : IPostOfficeAgent
    {

        private const string URL = "http://intmail.183.com.cn/item/itemStatusQuery.do";
        private const string POST_DATA = "itemNo=";
        private const string PACKAGE_NOT_FOUND_MESSAGE = "Cannot find a suitable item";


        public Entities.TrackingEvent TrackLastEvent(string trackingNumber)
        {
            var html = String.Empty;
            var data = POST_DATA + trackingNumber;


            //Download the HTML string
            using (WebClient client = new WebClient())
            {
                //client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17 ( .NET CLR 3.5.30729; .NET4.0E)");
                //client.Headers.Add(HttpRequestHeader.Referer, "http://www.dealextreme.com/accounts/TrackingRedirect.dx/TrackingNumber." + trackingNumber);
                //client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                //client.Headers.Add(HttpRequestHeader.AcceptLanguage, "pt-br,es;q=0.8,en-us;q=0.5,en;q=0.3");
                //client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                //client.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8;q=0.7,*;q=0.7");
                //client.Headers.Add(HttpRequestHeader.KeepAlive, "115");
                client.Encoding = Encoding.UTF8;
                client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded ");
                


                html = client.UploadString(URL, "post", data);
            }

            return ParseSingleObjectTrackHTML(html);

        }


        public Dictionary<string, Entities.TrackingEvent> TrackLastEvent(IEnumerable<string> trackingNumbers)
        {
            var ret = new Dictionary<string, TrackingEvent>();
            foreach (var number in trackingNumbers)
            {
                var ev = TrackLastEvent(number);
                if (ev != null)
                    ret.Add(number, ev);
            }

            return ret;
        }

        public Dictionary<string, List<Entities.TrackingEvent>> TrackAllEvents(IEnumerable<string> trackingNumbers)
        {
            throw new NotSupportedException();
        }

        public List<Entities.TrackingEvent> TrackAllEvents(string trackingNumber)
        {
            throw new NotSupportedException();
        }

        private TrackingEvent ParseSingleObjectTrackHTML(String html)
        {
            //If something was downloaded...
            if (!String.IsNullOrWhiteSpace(html))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                //Jump to the first grandchild <tr>, that is, <tag1><tag2><tr>
                var tds = doc.DocumentNode.SelectNodes("//td[@bgcolor=\"#f5f5f5\"]");

                if (tds == null)
                {
                    if (html.IndexOf(PACKAGE_NOT_FOUND_MESSAGE) != 0)
                    {
                        return null;
                    }


                    throw new Exception("CN: Unable to find the track info table");
                }


                var ev = new TrackingEvent();

                ev.TrackingNumber = tds[0].InnerText.Trim().Substring(0,13);

                ev.Description = tds[2].InnerText.Trim();

                String dataString = tds[5].InnerText.Trim().Substring(0, 10);
                ev.Date = DateTime.Parse(dataString);

                ev.Place = "CHINA-" + tds[4].InnerText.Trim();

                return ev;
                
            }

            return null;
        }
    }
}
