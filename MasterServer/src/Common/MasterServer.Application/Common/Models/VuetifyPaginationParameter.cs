using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Application.Common.Models
{
    public class VuetifyPaginationParameter
    {
        [FromQuery(Name = "page")]
        public int PaginationPage { get; set; } = 1;
        [FromQuery(Name = "itemsPerPage")]
        public int PaginationLimit { get; set; } = 20;
        [FromQuery]
        public SortInfo[] SortBy { get; set; }

        [FromQuery(Name = "search")]
        public string Search { get; set; }

        [FromQuery(Name = "full")]
        public bool Full { get; set; }

        public override string ToString()
        {
            return $"page={PaginationPage}&itemsPerPage={PaginationLimit}&search={Search}&full=${Full}&{string.Join("&", SortBy.Select((x, i) => $"sort[{i}].key={x.Key}&sort[{i}].order={x.Order}"))}";
        }
    }

    public class SortInfo
    {
        public string Key { get; set; }
        public string Order { get; set; }
    }
}
