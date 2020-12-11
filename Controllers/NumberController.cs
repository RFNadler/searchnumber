using SearchCombinations.Core;
using SearchCombinations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SearchCombinations.Controllers
{
    public class NumberController : Controller
    {

        [HttpPost]
        public JsonResult Execute(string psequence)
        {
            Processing processing = new Processing();
            Log log = new Log();
            
            try
            {
                Number number = Newtonsoft.Json.JsonConvert.DeserializeObject<Number>(psequence);

                if (number.Sequence == null)
                    throw new Exception("Sem sequência de numeros");
                else if (number.Sequence.Count == 0)
                    throw new Exception("Sem sequência de numeros");
                else if (number.Sequence.Count == 1)
                    throw new Exception("A sequência deve conterno minímo dois números");

                if (number.Target == 0)
                    throw new Exception("Sem número alvo");

                Result result = processing.execute(number);

                log.write(result, number);

                return Json(result);
            }
            catch(Exception ex)
            {
               return Json(new Result { MsgStatus = (int)ENUMMsgStatus.KdMsgStatus.kdError, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult List(string pinitialDate, string pfinalDate)
        {
            Log log = new Log();
            List<TBLOG> lstReturn = new List<TBLOG>();
            
            try
            {
                lstReturn = log.list(pinitialDate, pfinalDate);

                if(lstReturn.Count == 0)
                {
                    lstReturn = new List<TBLOG>();
                    lstReturn.Add(new TBLOG { msgstatus = ENUMMsgStatus.KdMsgStatus.kdNoData, error = "A pesquisa não retornou nenhuma informação."});
                }
            }
            catch (Exception ex)
            {
                lstReturn = new List<TBLOG>();
                lstReturn.Add(new TBLOG { msgstatus = ENUMMsgStatus.KdMsgStatus.kdError, error = "Erro ao executar a pesquisa. Erro: " + ex.Message });
            }

            return Json(lstReturn);
        }
    }
}
