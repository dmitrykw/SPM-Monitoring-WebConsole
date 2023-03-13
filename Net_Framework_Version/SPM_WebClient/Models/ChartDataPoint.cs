using System;
using System.Runtime.Serialization;

namespace SPM_WebClient.Models
{
    //DataContract for Serializing Data - required to serve in JSON format
    [DataContract]
    public class ChartDataPoint
    {
        public ChartDataPoint(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}