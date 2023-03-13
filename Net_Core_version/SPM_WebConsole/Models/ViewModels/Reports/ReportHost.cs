using Newtonsoft.Json;

namespace SPM_WebConsole.Models.ViewModels.Reports
{
    public class ReportHost
    {
        public string Hostname { get; set; }

        private IEnumerable<KeyValuePair<DateTime, int?>> icmp_stats;
        public IEnumerable<KeyValuePair<DateTime, int?>> ICMPStats { get { return icmp_stats; } set { icmp_stats = value; FillChartData(); } }

        public void FillChartData()
        {
            DateTime dt_now = DateTime.Now;
            chart_data_points = new List<ChartDataPoint>();
            if (ICMPStats != null)
            {
                foreach (var item in ICMPStats)
                {
                    if (item.Key != null & item.Value != null)
                    {
                        string pointDTString;
                        if (item.Key.Day == dt_now.Day && item.Key.Month == dt_now.Month && item.Key.Year == dt_now.Year)
                        {
                            pointDTString = item.Key.ToLongTimeString();
                        }
                        else { pointDTString = item.Key.ToShortDateString() + " " + item.Key.ToLongTimeString(); }

                        chart_data_points.Add(new ChartDataPoint(pointDTString, item.Value.Value));
                    }
                }
            }
        }

        private List<ChartDataPoint> chart_data_points;


        public IEnumerable<KeyValuePair<DateTime, int?>> ICMPStats_Descending => ICMPStats.OrderByDescending(x => x.Key);
        public string ChartDataPointsString => JsonConvert.SerializeObject(chart_data_points);

        public double? AverageAnswerTime { get; set; }
        public double? UpTime { get; set; }
    }
}
