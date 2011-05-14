using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorreioNet.Engine.PostOffice
{
    public class HongKongAgent : IPostOfficeAgent
    {

        internal HongKongAgent() { }

        public List<Entities.TrackingEvent> TrackObject(string trackingNumber)
        {
            throw new NotImplementedException();
        }
    }
}
