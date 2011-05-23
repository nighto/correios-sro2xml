using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CorreioNet.Engine.PostOffice;
using CorreioNet.Engine.Entities;

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

                //case "HK":
                //    return new HongKongAgent();

                default:
                    throw new Exception("Unknown Post Office!");
                //break;
            }
        }

    
        /// <summary>
        /// Returns the most recent event for the given tracking number
        /// </summary>
        /// <param name="trackingNumber"></param>
        /// <returns></returns>
        public static TrackingEvent TrackLastEvent(String destinationCountry, String trackingNumber)
        {
            IPostOfficeAgent agent = FindAgent(destinationCountry);
            return agent.TrackLastEvent(trackingNumber);
        }

        /// <summary>
        /// Returns all event history for the given tracking number
        /// </summary>
        /// <param name="trackingNumber"></param>
        /// <returns></returns>
        public static List<TrackingEvent> TrackAllEvents(String destinationCountry, String trackingNumber)
        {
            IPostOfficeAgent agent = FindAgent(destinationCountry);
            return agent.TrackAllEvents(trackingNumber);
        }

        /// <summary>
        /// Returns the last event for each one of the tracking numbers specified.
        /// </summary>
        /// <param name="trackingNumbers"></param>
        /// <returns></returns>
        public static Dictionary<String, TrackingEvent> TrackLastEvent(String destinationCountry, IEnumerable<String> trackingNumbers)
        {
            IPostOfficeAgent agent = FindAgent(destinationCountry);
            return agent.TrackLastEvent(trackingNumbers);
        }

        /// <summary>
        /// Returns all event history for each one of the tracking numbers specified.
        /// </summary>
        /// <param name="trackingNumbers"></param>
        /// <returns></returns>
        public static Dictionary<String, List<TrackingEvent>> TrackAllEvents(String destinationCountry, IEnumerable<String> trackingNumbers)
        {
            IPostOfficeAgent agent = FindAgent(destinationCountry);
            return agent.TrackAllEvents(trackingNumbers);
        }



    }
}
