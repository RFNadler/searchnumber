using SearchCombinations.Models;
using SearchCombinations.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchCombinations.Core
{
    public class Log
    {
        public void write(Result presult, Number pnumber)
        {
            TBLOGRepository repository = new TBLOGRepository();
                
            try
            {
                TBLOG tblog = new TBLOG();
                
                tblog.sequence = pnumber.SequenceString;
                tblog.target = pnumber.Target;
                if (presult.Combination != null)
                {
                    foreach (int intNumber in presult.Combination)
                    {
                        tblog.result += (String.IsNullOrEmpty(tblog.result) ? "" : ", ") + intNumber.ToString();
                    }
                }
                tblog.msgstatus = (ENUMMsgStatus.KdMsgStatus)presult.MsgStatus;
                if (presult.MsgStatus == (int)ENUMMsgStatus.KdMsgStatus.kdError)
                    tblog.error = presult.message;

                repository.execute(tblog, DBASE.TypeExecute.INSERT);

                if (!String.IsNullOrEmpty(repository.ErrorDescription))
                    throw new Exception(repository.ErrorDescription); 

            }catch(Exception ex)
            {
                presult.MsgStatus = (int)ENUMMsgStatus.KdMsgStatus.kdError + presult.MsgStatus;
            }
        }

        public List<TBLOG> list(string pinitialDate, string pfinalDate)
        {
            TBLOGRepository repository = new TBLOGRepository();
            List<TBLOG> lstReturn = new List<TBLOG>();
            
            try
            {
                lstReturn = repository.list(pinitialDate, pfinalDate);

                if (!String.IsNullOrEmpty(repository.ErrorDescription))
                    throw new Exception(repository.ErrorDescription);

                return lstReturn;
            }
            catch(Exception ex)
            {
                lstReturn = new List<TBLOG>();
                lstReturn.Add(new TBLOG { msgstatus = ENUMMsgStatus.KdMsgStatus.kdError, error = "Erro ao executar a pesquisa. Erro: " + ex.Message });
            }

            return lstReturn;
        }
    }
}