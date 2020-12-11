using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchCombinations.Models
{
    public class Result
    {
        public List<int> Combination { get; set; }
        public int MsgStatus { get; set; }

        public string message { get; set; }
    }
}