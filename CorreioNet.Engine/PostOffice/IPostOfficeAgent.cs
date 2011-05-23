using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CorreioNet.Engine.Entities;

namespace CorreioNet.Engine.PostOffice
{
    public interface IPostOfficeAgent
    {

        /// <summary>
        /// Returns the most recent event for the given tracking number
        /// </summary>
        /// <param name="trackingNumber"></param>
        /// <returns></returns>
        TrackingEvent TrackLastEvent(String trackingNumber);

        /// <summary>
        /// Returns all event history for the given tracking number
        /// </summary>
        /// <param name="trackingNumber"></param>
        /// <returns></returns>
        List<TrackingEvent> TrackAllEvents(String trackingNumber);

        /// <summary>
        /// Returns the last event for each one of the tracking numbers specified.
        /// </summary>
        /// <param name="trackingNumbers"></param>
        /// <returns></returns>
        Dictionary<String, TrackingEvent> TrackLastEvent(IEnumerable<String> trackingNumbers);

        /// <summary>
        /// Returns all event history for each one of the tracking numbers specified.
        /// </summary>
        /// <param name="trackingNumbers"></param>
        /// <returns></returns>
        Dictionary<String, List<TrackingEvent>> TrackAllEvents(IEnumerable<String> trackingNumbers);
       
    }
}
