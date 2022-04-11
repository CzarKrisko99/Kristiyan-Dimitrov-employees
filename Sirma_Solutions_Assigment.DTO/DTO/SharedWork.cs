
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Sirma_Solutions_Assigment.DTO.DTO
{
    public class SharedWork
    {

        public SharedWork()
        {
            this.ProjectIdsAndWorkTime = new Dictionary<int, int>();
        }

        [JsonPropertyName("firstEmployeeId")]
        public int FirstEmployeeId { get; set; }

        [JsonPropertyName("secondEmployeeId")]
        public int SecondEmployeeId { get; set; }

        [JsonPropertyName("projectIdsAndWorkTime")]
        public Dictionary<int, int> ProjectIdsAndWorkTime { get; set; }

        public int TotalWorkedDays
        {
            get
            {
                int totalWorkTime = 0;
                foreach (var worktime in ProjectIdsAndWorkTime.Values)
                {
                    totalWorkTime += worktime;
                }

                return totalWorkTime;
            }
        }
    }
}
