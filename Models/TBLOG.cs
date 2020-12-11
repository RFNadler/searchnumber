using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchCombinations.Models
{
    public class TBLOG
    {
        public string sequence { get; set; }
        public int target { get; set; }
        public string result { get; set; }
        public ENUMMsgStatus.KdMsgStatus msgstatus { get; set; }
        public string error { get; set; }
        public string description { get; set; }
        public DateTime ?dtlog { get; set; }
    }
}