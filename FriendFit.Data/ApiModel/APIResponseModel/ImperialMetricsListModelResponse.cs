using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class ImperialMetricsListModelResponse
    {
        public ImperialMetricsListModelResponse()
        {
            Response = new ImperialMetricsModel();
        }
        public ImperialMetricsModel Response { get; set; }
    }

    public class ImperialMetricsList
    {
        public Int32 Id { get; set; }
        public string ImperialMetricsName { get; set; }
        public bool Type { get; set; }
    }

    public class ImperialMetricsModel
    {
        public ImperialMetricsModel()
        {
            ImperialMetricsList = new List<APIResponseModel.ImperialMetricsList>();
        }
        public List<ImperialMetricsList> ImperialMetricsList { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

