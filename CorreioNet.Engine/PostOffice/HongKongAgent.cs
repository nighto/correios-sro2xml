using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorreioNet.Engine.PostOffice
{
    public class HongKongAgent : IPostOfficeAgent
    {

        internal HongKongAgent() { }



        public Entities.TrackingEvent TrackLastEvent(string trackingNumber)
        {
            throw new NotImplementedException();
        }

        public List<Entities.TrackingEvent> TrackAllEvents(string trackingNumber)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Entities.TrackingEvent> TrackLastEvent(IEnumerable<string> trackingNumbers)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Entities.TrackingEvent>> TrackAllEvents(IEnumerable<string> trackingNumbers)
        {
            throw new NotImplementedException();
        }
    }
}
