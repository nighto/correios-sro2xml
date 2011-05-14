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
        private const string packageNotFound = "O nosso sistema não possui dados sobre o objeto informado.";
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

                //If something was downloaded...
                if (!String.IsNullOrWhiteSpace(html))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    //Jump to the first grandchild <tr>, that is, <tag1><tag2><tr>
                    var trs = doc.DocumentNode.SelectNodes("//tr");

                    if (trs == null)
                    {
                        if (html.IndexOf(packageNotFound) != 0)
                            throw new Exception("Package not found.");
                        throw new Exception("no <tr> found");
                    }

                    //Skips the first <tr> as it's a header
                    foreach (HtmlNode tr in trs.Skip(1))
                    {
                        //If it's a destination row "semirow", not a new table row, then we should treat it later.
                        if (tr.FirstChild.Attributes.Contains("colspan") && tr.FirstChild.Attributes["colspan"].Value == "2")
                        {
                            continue;
                        }

                        var ev = new TrackingEvent();

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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
