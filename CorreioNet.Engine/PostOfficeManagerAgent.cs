using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CorreioNet.Engine.PostOffice;

namespace CorreioNet.Engine
{
    public class PostOfficeManagerAgent
    {
        private PostOfficeManagerAgent() { }

        private static IPostOfficeAgent FindAgent(String destinationCountry)
        {

            switch (destinationCountry)
            {
                case "BR":
                    return new CorreiosAgent();

                case "HK":
                    return new HongKongAgent();

                default:
                    throw new Exception("Unknown Post Office!");
                //break;
            }
        }

        public static List<Entities.TrackingEvent> TrackObject(String destinationCountry, string trackingNumber)
        {
            IPostOfficeAgent agent = FindAgent(destinationCountry);
            return agent.TrackObject(trackingNumber);
        }
    }
}
