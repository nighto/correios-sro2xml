using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CorreioNet.Engine.Entities;

namespace CorreioNet.Engine.PostOffice
{
    public interface IPostOfficeAgent
    {
        List<TrackingEvent> TrackObject(String trackingNumber);
    }
}
