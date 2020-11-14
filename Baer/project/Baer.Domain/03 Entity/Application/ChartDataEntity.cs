using System.Collections.Generic;

namespace Baer.Domain.Entity
{
    public class ShowChange
    {
        public string ChangeInfo { get; set;}
        public string Type { get; set; }
        public string CreatorTime { get; set; }
    }

    public class DoughnutDataEntity
    {
        public string value { get; set; }
        public string color { get; set; }
        public string highlight { get; set; }
        public string label { get; set; }
    }

    public class LineChartData
    {
        public string[] labels { get; set; }
        public List<LineChartDataSet> datasets { get; set; }

        public LineChartData()
        {
            this.datasets = new List<LineChartDataSet>();
        }
    }

    public class LineChartDataSet
    {
        public string value { get; set; }
        public string fillColor { get; set; }
        public string strokeColor { get; set; }
        public string pointColor { get; set; }
        public string pointStrokeColor { get; set; }
        public string pointHighlightFill { get; set; }
        public string pointHighlightStroke { get; set; }
        public string[] data { get; set; }
    }
}
