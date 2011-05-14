using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorreioNet.Engine.Entities
{
    /// <summary>
    /// tem um <numero></numero> e um ou mais <evento></evento>
    /// </summary>
    public class TrackableObject
    {
        public String TrackingNumber { get; set; }
        public List<TrackingEvent> Events { get; set; }
    }
}
