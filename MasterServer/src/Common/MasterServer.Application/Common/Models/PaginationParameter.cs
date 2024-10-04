using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Application.Common.Models
{
    public interface IPaginationParameter
    {

        int PaginationPage { get; set; }

        int PaginationLimit { get; set; }

        string SortBy { get; set; }


        string OrderBy { get; set; }


        string Search { get; set; }


        bool Full { get; set; }
    }

    public class QueryPaginationParameter : IPaginationParameter
    {
        [FromQuery(Name = "currentPage")]
        public int PaginationPage { get; set; } = 1;
        [FromQuery(Name = "pageSize")]
        public int PaginationLimit { get; set; } = 20;
        [FromQuery(Name = "sortBy")]
        public string SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public string OrderBy { get; set; }

        [FromQuery(Name = "q")]
        public string Search { get; set; }

        [FromQuery(Name = "full")]
        public bool Full { get; set; }
    }

    public class BodyPaginationParameter : IPaginationParameter
    {

        public int PaginationPage { get; set; } = 1;

        public int PaginationLimit { get; set; } = 20;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }


        public string Search { get; set; }


        public bool Full { get; set; }
    }

}
