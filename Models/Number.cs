using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchCombinations.Models
{
    public class Number
    {
        public List<int> Sequence { get; set; }
        public int Target { get; set; }

        public string SequenceString
        {
            get
            {
                string strReturn = "";

                if (Sequence == null)
                    return strReturn;

                foreach (int intNumber in Sequence)
                {
                    strReturn += (String.IsNullOrEmpty(strReturn) ? "" : ", ") + intNumber.ToString();
                }

                return strReturn;
            }
        }

    }
}