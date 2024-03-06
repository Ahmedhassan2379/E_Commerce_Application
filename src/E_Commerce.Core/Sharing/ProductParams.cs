using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Sharing
{
    public class ProductParams
    {
        public string sort { get; set; }
        public int MaxPageSize { get; set; } = 15;
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; }

        private int _PageSize = 3;
        public int PageSize 
        {
            get { return _PageSize; }
            set { _PageSize = value>MaxPageSize ? MaxPageSize:value; }
        }
        private string _Search;

        public string Search
        {
            get { return _Search; }
            set { _Search = value.ToLower(); }
        }
    }
}
