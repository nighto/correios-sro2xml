using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using CorreioNet.Engine.Entities;
using HtmlAgilityPack;
using System.Globalization;
using CorreioNet.Engine.Extensions;

namespace CorreioNet.Engine.PostOffice
{
    public class CorreiosAgent : IPostOfficeAgent
    {
        internal CorreiosAgent() { }

        private const string URL = "http://websro.correios.com.br/sro_bin/txect01$.QueryList";
        private const string POST_DATA = "P_LINGUA=001&P_TIPO=002&Z_ACTION=Continuar&P_COD_LIS=";
        private const string PACKAGE_NOT_FOUND_MESSAGE = "Nenhum objeto do intervalo foi encontrado";
        private const string SINGLE_PACKAGE_HISTORY_MESSAGE = "Histórico do Objeto";
        private const int MAX_OBJECTS_PER_REQUEST = 3;

        CultureInfo dateCulture = new System.Globalization.CultureInfo("pt-BR");


        public TrackingEvent TrackLastEvent(String trackingNumber)
        {
            var events = TrackObject(trackingNumber, true);

            if (events.Count > 0)
                return events.First();

            return null;
        }

        public Dictionary<String, TrackingEvent> TrackLastEvent(IEnumerable<String> trackingNumbers)
        {
            var ret = new Dictionary<String, TrackingEvent>();
            var html = String.Empty;
            var postdataSB = new StringBuilder(POST_DATA);

            //Prepare post data
            foreach (var number in trackingNumbers.Take(MAX_OBJECTS_PER_REQUEST))
            {
                postdataSB.Append(number);
                postdataSB.Append(";");
            }

            //Download the HTML string
            using (WebClient client = new WebClient())
            {
                html = client.UploadString(URL, "POST", postdataSB.ToString());
            }

            //If something was downloaded...
            if (!String.IsNullOrWhiteSpace(html))
            {
                //If Only one package returned
                if (html.Contains(SINGLE_PACKAGE_HISTORY_MESSAGE))
                {
                    var ev = ParseSingleObjectTrackHTML(html, true).First();
                    ret.Add(ev.TrackingNumber, ev);
                }
                else
                {
                    ret = ParseMultiObjectTrackHTML(html);
                }
            }

            //If we have more pending objects, call again recursively.
            if (trackingNumbers.Count() > MAX_OBJECTS_PER_REQUEST)
            {
                var pendingNumbers = trackingNumbers.Skip(MAX_OBJECTS_PER_REQUEST);
                ret.AddRange(this.TrackLastEvent(pendingNumbers));
            }

            return ret;
        }

        public Dictionary<String, List<TrackingEvent>> TrackAllEvents(IEnumerable<String> trackingNumbers)
        {
            var ret = new Dictionary<String, List<TrackingEvent>>();
            foreach (var number in trackingNumbers)
            {
                var events = this.TrackAllEvents(number);

                if (events != null && events.Count > 0)
                    ret.Add(number, events);
            }

            return ret;
        }

        public List<TrackingEvent> TrackAllEvents(String trackingNumber)
        {
            return TrackObject(trackingNumber, false);
        }
   
        private Dictionary<String, TrackingEvent> ParseMultiObjectTrackHTML(String html)
        {
            var ret = new Dictionary<String, TrackingEvent>();

            //If something was downloaded...
            if (!String.IsNullOrWhiteSpace(html))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                //Jump to the first grandchild <tr>, that is, <tag1><tag2><tr>
                var trs = doc.DocumentNode.SelectNodes("//tr");

                if (trs == null)
                {
                    if (html.IndexOf(PACKAGE_NOT_FOUND_MESSAGE) != 0)
                    {
                        //throw new Exception("Package not found.");
                        return ret;
                    }

                    throw new Exception("no <tr> found");
                }

                //Skips the first <tr> as it's a header
                foreach (HtmlNode tr in trs.Skip(1))
                {
                 
                    var ev = new TrackingEvent();

                    HtmlNodeCollection tds = tr.ChildNodes;

                    ev.TrackingNumber = tds[0].FirstChild.InnerText;
                    ev.Description = tds[2].FirstChild.InnerText; // Status

                    String dataString = tds[4].InnerText;
                    ev.Date = DateTime.Parse(dataString, dateCulture);

                    ev.Place = tds[6].InnerText;


                    if (ev.Place.Contains("/"))
                    {
                        string[] city = ev.Place.Trim().Split("/".ToCharArray());
                        ev.City = city[0];
                        if (city.Count() == 2)
                            ev.UF = city[1];
                    }

                    ret.Add(ev.TrackingNumber, ev);
                }
            }

            return ret;
        }

        private List<TrackingEvent> ParseSingleObjectTrackHTML(String html, bool lastOnly)
        {
            var ret = new List<TrackingEvent>();

            //If something was downloaded...
            if (!String.IsNullOrWhiteSpace(html))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                //Jump to the first grandchild <tr>, that is, <tag1><tag2><tr>
                var trs = doc.DocumentNode.SelectNodes("//tr");

                if (trs == null)
                {
                    if (html.IndexOf(PACKAGE_NOT_FOUND_MESSAGE) != 0)
                    {
                        /* JC: I changed this because exception costs more than a return with empty results.
                         * This will result in a faster tracking
                         */

                        //throw new Exception("Package not found.");
                        return ret;
                    }


                    throw new Exception("no <tr> found");
                }


                var fonts = doc.DocumentNode.SelectNodes("//font");
                var trackingNumber = fonts[0].FirstChild.InnerText.Substring(1, 13);

                //Skips the first <tr> as it's a header
                foreach (HtmlNode tr in trs.Skip(1))
                {
                    //If it's a destination row "semirow", not a new table row, then we should treat it later.
                    if (tr.FirstChild.Attributes.Contains("colspan") && tr.FirstChild.Attributes["colspan"].Value == "2")
                    {
                        continue;
                    }

                    //If only the most recent event must be returned, then it's time to stop
                    if (lastOnly && ret.Count > 0)
                    {
                        break;
                    }

                    var ev = new TrackingEvent();
                    ev.TrackingNumber = trackingNumber;

                    HtmlNodeCollection tds = tr.ChildNodes;

                    String dataString = tds[0].InnerText;
                    ev.Date = DateTime.Parse(dataString, dateCulture);

                    string[] place = tds[1].InnerText.Split("-".ToCharArray());
                    ev.Place = place[0].Trim();
                    if (place.Count() == 2)
                    {
                        string[] city = place[1].Trim().Split("/".ToCharArray());
                        ev.City = city[0];
                        if (city.Count() == 2)
                            ev.UF = city[1];
                    }

                    String status = tds[2].FirstChild.InnerText;
                    ev.Description = status;

                    ret.Add(ev);
                }
            }

            return ret;
        }

        private List<TrackingEvent> TrackObject(String trackingNumber, bool lastOnly)
        {
            try
            {
                
                var html = String.Empty;
                var data = POST_DATA + trackingNumber;
                
                
                //Download the HTML string
                using (WebClient client = new WebClient())
                {
                    html = client.UploadString(URL, data);
                }

                return ParseSingleObjectTrackHTML(html, lastOnly);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
