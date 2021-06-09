using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechLibrary.Models
{
    public class SearchRequest
    {
        public string Text { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
