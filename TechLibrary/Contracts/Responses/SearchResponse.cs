using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechLibrary.Models
{
    public class SearchResponse<T>
    {
        public List<T> Items { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
