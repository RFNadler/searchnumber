using SearchCombinations.Models;
using SearchCombinations.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchCombinations.Core
{
    public class Processing
    {
        public Result execute(Number psequence)
        {
            List<int> lstNumber = new List<int>();
            List<int[]> aCombination = new List<int[]>();

            Result result = new Result();
            decimal decTryAgain = 0;
            int intColumn = 0;

            try
            {
                result.MsgStatus = (int)ENUMMsgStatus.KdMsgStatus.kdNotFound;
                result.message = "Sem combinações.";

                foreach (int intNumber in psequence.Sequence)
                {
                    lstNumber.Add(intNumber);
                }
                
                int[] aNumber = listToArray(lstNumber);

                while (aCombination.Count == 0 && decTryAgain <= psequence.Target)
                {
                    aCombination = combinationSearch(aNumber, psequence.Target);

                    if (aCombination.Count > 0)
                    {
                        List<int> lstSequence = new List<int>();
                            
                        foreach (var combo in aCombination.Take(20))
                        {
                            String strSequence = "";

                            for (int it = 0; it < combo.Length; it++)
                            {
                                lstSequence.Add(aNumber[combo[it]]); 
                               strSequence += (it > 0 ? " + " : "") + aNumber[combo[it]];
                            }

                            result.Combination = lstSequence;
                            result.MsgStatus = (int)ENUMMsgStatus.KdMsgStatus.kdFound;
                            result.message = "Combinação encontrada.";
                            break;
                        }

                    }
                    else
                    {
                        lstNumber.Add(lstNumber[intColumn]);
                        aNumber = listToArray(lstNumber);
                        decTryAgain++;
                        intColumn++;
                        if (intColumn >= psequence.Sequence.Count)
                            intColumn = 0;
                    }
                }

                
            }
            catch (Exception ex)
            {
                result.MsgStatus = (int)ENUMMsgStatus.KdMsgStatus.kdErrorProc;
                result.message = "Erro ao tentar gerar as combinações dos números. Erro:" + ex.Message;
            }

            return result;
        }

        private List<int[]> combinationSearch(int[] pvNumero, int pTarget)
        {
            var tipoBusca = (Busca)BuscaBinaria;

            int objetivo = pTarget;

            int soma = 0;

            int tampilha;

            try
            {
                for (tampilha = 0; tampilha < pvNumero.Length; tampilha++)
                {
                    soma += pvNumero[tampilha];
                    if (soma > objetivo)
                        break;
                }

                var pilha = new Stack<int>(tampilha);

                var combos = new List<int[]>(pvNumero.Length * pvNumero.Length);
                soma = 0;
                var idx = pvNumero.Length - 1;
                pilha.Push(idx);
                soma += pvNumero[idx];
                while (pilha.Count > 0)
                {
                    if (soma == objetivo)
                    {
                        combos.Add(pilha.Reverse().ToArray());
                        while (pilha.Count > 0)
                        {
                            idx = pilha.Pop();
                            soma -= pvNumero[idx];
                            idx--;
                            if (idx >= 0)
                            {
                                pilha.Push(idx);
                                soma += pvNumero[idx];
                                break;
                            }
                        }
                    }
                    else if (soma < objetivo)
                    {
                        var top = pilha.Peek() - 1;
                        idx = tipoBusca(pvNumero, objetivo - soma, 0, top);
                        if (idx < 0) idx = ~idx - 1;
                        if (idx >= 0 && idx <= top && pvNumero[idx] + soma <= objetivo)
                        {
                            pilha.Push(idx);
                            soma += pvNumero[idx];
                        }
                        else
                        {
                            while (pilha.Count > 0)
                            {
                                idx = pilha.Pop();
                                soma -= pvNumero[idx];
                                idx--;
                                if (idx >= 0)
                                {
                                    pilha.Push(idx);
                                    soma += pvNumero[idx];
                                    break;
                                }
                            }
                        }
                    }
                }

                return combos;
            }
            catch (Exception ex)
            {
                string strTeste = "";
            }

            return null;
        }

        private string arrayToString(List<int> parray)
        {
            string strReturn = "";
            try
            {
                if (parray == null)
                    return "";

                foreach (int inNumber in parray)
                {
                    strReturn += (String.IsNullOrEmpty(strReturn) ? "" : ", ") + inNumber.ToString();
                }

            }
            catch(Exception ex)
            {
                
            }

            return strReturn;
        }

        private int[] listToArray(List<int> plist)
        {
            int[] vReturn = new int[plist.Count];
            int intCount = 0;
            foreach (int number in plist)
            {
                vReturn[intCount] = number;
                intCount++;
            }

            return vReturn;
        }

        private static int BuscaBinaria(int[] array, int valor, int a, int b)
        {
            if (b - a + 1 < 0) return ~a;
            return Array.BinarySearch(array, a, b - a + 1, valor);
        }

        delegate int Busca(int[] array, int valor, int a, int b);
    }
}