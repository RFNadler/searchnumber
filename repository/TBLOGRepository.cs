using SearchCombinations.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace SearchCombinations.repository
{
    public class TBLOGRepository
    {
        public string ErrorDescription = "";
        DBASE db = new DBASE();

        public TBLOGRepository()
        {
            db.ErrorDescription = "";
            db.TableName = "tblog";
        }

        public void execute(TBLOG pValor, DBASE.TypeExecute pTypeCommand)
        {
            try
            {
                db.EraseParameter();
                db.TypeCommand = pTypeCommand;

                db.AddParameter("SEQUENCE", pValor.sequence);
                db.AddParameter("TARGET", pValor.target);
                db.AddParameter("RESULT", pValor.result);
                db.AddParameter("MSGSTATUS", pValor.msgstatus);
                db.AddParameter("ERROR", pValor.error);
                db.ExecutaComandoSQL();

                if (db.ErrorDescription != "")
                    throw new Exception(db.ErrorDescription);

            }
            catch (Exception ex)
            {
                ErrorDescription = ex.Message.ToString();
            }
        }

        public List<TBLOG> list(string pinitialDate, string pfinalDate)
        {
            DbDataReader dr = null;
            List<TBLOG> lstReturn = new List<TBLOG>();
            
            try
            {
                db.EraseParameter();

                db.AddParameter("InitialDate", Convert.ToDateTime(pinitialDate + " 00:00"));
                db.AddParameter("FinalDate", Convert.ToDateTime(pfinalDate + " 23:59"));

                db.SQL = "select SEQUENCE, TARGET, RESULT,";
                db.SQL += " CASE";
                db.SQL += "   WHEN MSGSTATUS = 1 THEN 'OK' ";
                db.SQL += "   WHEN MSGSTATUS = 2 THEN 'SEQUENCIA NÃO ENCONTRADA' ";
                db.SQL += "   WHEN MSGSTATUS = 3 THEN 'ERRO NO PROCESSAMENTO' ";
                db.SQL += " END MSGSTATUS, ERROR, DTLOG ";
                db.SQL += " from " + db.TableName;
                db.SQL += " where DTLOG between (@InitialDate) AND (@FinalDate)";
                db.SQL += " order by DTLOG desc";

                dr = db.DataReader();

                if (!String.IsNullOrEmpty(db.ErrorDescription))
                    throw new Exception(db.ErrorDescription);
                else
                {
                    while (dr.Read())
                    {
                        TBLOG model = new TBLOG();

                        if (!dr.IsDBNull(0)) model.sequence = dr.GetString(0);
                        if (!dr.IsDBNull(1)) model.target = dr.GetInt32(1);
                        if (!dr.IsDBNull(2)) model.result = dr.GetString(2);
                        if (!dr.IsDBNull(3)) model.description = dr.GetString(3);
                        if (!dr.IsDBNull(4)) model.error = dr.GetString(4);
                        if (!dr.IsDBNull(5)) model.dtlog = dr.GetDateTime(5);

                        lstReturn.Add(model);
                    }

                    dr.Close();
                }
                return lstReturn;

            }
            catch (Exception ex)
            {
                ErrorDescription = ex.Message;
            }

            return null;
        }
    }
}