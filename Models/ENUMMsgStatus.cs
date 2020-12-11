using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchCombinations.Models
{
    public class ENUMMsgStatus
    {
        public enum KdMsgStatus
        {
            kdFound = 1,
            kdNotFound = 2,
            kdErrorProc = 3,
            kdNoData = 4,
            kdError = 100
        }
    }
}