using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendFit.Data.ApiModel.APIResponseModel
{
    public class ImperialMetricsModelResponse
    {
        public ImperialMetricsModelResponse()
        {
            Response = new ImperialMetricsModel1();
        }
        public ImperialMetricsModel1 Response { get; set; }
    }

    public class ImperialMetrics
    {
        public Int32 WeightImperialId { get; set; }
        public string WeightImperialName { get; set; }

        public Int32 DistenceImperialId { get; set; }
        public string DistenceImperialName { get; set; }
    }

    public class ImperialMetricsModel1
    {
        public ImperialMetricsModel1()
        {
            ImperialMetrics = new APIResponseModel.ImperialMetrics();
        }
        public ImperialMetrics ImperialMetrics { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
