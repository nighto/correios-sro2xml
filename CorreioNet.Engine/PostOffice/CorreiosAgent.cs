using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using CorreioNet.Engine.Entities;
using HtmlAgilityPack;
using System.Globalization;

namespace CorreioNet.Engine.PostOffice
{
    public class CorreiosAgent : IPostOfficeAgent
    {
        internal CorreiosAgent() { }

        private const string URL = "http://websro.correios.com.br/sro_bin/txect01$.QueryList?P_LINGUA=001&P_TIPO=001&P_COD_UNI=";
        CultureInfo dateCulture = new System.Globalization.CultureInfo("pt-BR");


        public List<TrackingEvent> TrackObject(String trackingNumber)
        {
            try
            {
                var ret = new List<TrackingEvent>();
                var html = String.Empty;
                var finalURL = URL + trackingNumber;
                
                
                //Download the HTML string
                using (WebClient client = new WebClient())
                {
                    html = client.DownloadString(finalURL);
                }

                //If something were downloaded...
                if (!String.IsNullOrWhiteSpace(html))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var trs = doc.DocumentNode.SelectNodes("//tr");

                    foreach (HtmlNode tr in trs.Skip(1))
                    {

                        if (tr.FirstChild.Attributes.Contains("colspan") && tr.FirstChild.Attributes["colspan"].Value == "2")
                        {
                            continue;
                        }

                        var ev = new TrackingEvent();

                        HtmlNodeCollection tds = tr.ChildNodes;

                        String dataString = tds[0].InnerText;
                        ev.Date = DateTime.Parse(dataString, dateCulture);

                        String local = tds[1].InnerText;
                        ev.Place = local;

                        String status = tds[2].FirstChild.InnerText;
                        ev.Description = status;

                        ret.Add(ev);
                    }
                }


                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
