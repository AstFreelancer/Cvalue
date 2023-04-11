using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cvalue
{
    public class TermCandidate
    {
        public int frequency;
        public string s;
        public int length;
        public double Cval;
        public int t;
        public int c;
        public TermCandidate(int f, string s1)
        {
            frequency = f;
            s = s1;

            string[] arr = s.Split(' ');
            length = arr.Length;

            Cval = 0;
            t = 0;
            c = 0;
        }
        public TermCandidate(TermCandidate tc)
        {
            frequency = tc.frequency;
            s = tc.s;
            length = tc.length;
            Cval = tc.Cval;
            t = tc.t;
            c = tc.c;
        }
        public void SetCValue()
        {
            if (c == 0)
                Cval = Math.Log(length, 2) * frequency;
            else
                Cval = Math.Log(length, 2) * (frequency - t / c);
        }
    }

    public class CValueCalculations
    {
        public List<TermCandidate> TermCandidateList;
        public CValueCalculations()
        {
            TermCandidateList = new List<TermCandidate>();
        }
        public void Calculate()
        {
            int maxlen = TermCandidateList[0].length;
            for (int i = 0; i < TermCandidateList.Count; i++)
            {
                if (TermCandidateList[i].length == maxlen)
                    TermCandidateList[i].SetCValue();
                    //for each substring b of a
                    List<string> allSubstrings = GenerateAllSubstrings(TermCandidateList[i].s);
                    //idem vniz i ishcem takuyu. Esli nashli, to v naidennoi obnovlyaem t i c.
                    for (int j = i + 1; j < TermCandidateList.Count; j++)
                    {
                        foreach (string str in allSubstrings)
                            if (TermCandidateList[j].s == str)
                            {
                                TermCandidateList[j].c ++;
                                TermCandidateList[j].t += TermCandidateList[i].frequency - TermCandidateList[i].t;
                            }
                    }
            }
            for (int i = 0; i < TermCandidateList.Count; i++)
            {
                if (TermCandidateList[i].Cval != 0)
                    continue;
                TermCandidateList[i].SetCValue();       
            }
        }
        public List<string> GenerateAllSubstrings(string s)
        {
            List<string> result = new List<string>();
            string[] mas = s.Split(' ');
            for (int i = 0; i < mas.Length; i++)
            {
                string res = mas[i].ToString();
                for (int j = i + 1; j < mas.Length; j++)
                {
                    if (i == 0 && j == mas.Length - 1)
                        continue;
                    res += " " + mas[j].ToString();
                    result.Add(res);
                }
            }

            return result;
        }
        public void CreateList()
        {
            TermCandidate tc1 = new TermCandidate(5, "adenoid cystic basal cell carcinoma");
            TermCandidateList.Add(tc1);

            TermCandidate tc2 = new TermCandidate(11, "cystic basal cell carcinoma");
            TermCandidateList.Add(tc2);

            TermCandidate tc3 = new TermCandidate(7, "ulcerated basal cell carcinoma");
            TermCandidateList.Add(tc3);

            TermCandidate tc4 = new TermCandidate(5, "recurrent basal cell carcinoma");
            TermCandidateList.Add(tc4);

            TermCandidate tc5 = new TermCandidate(3, "circuscribed basal cell carcinoma");
            TermCandidateList.Add(tc5);

            TermCandidate tc6 = new TermCandidate(984, "basal cell carcinoma");
            TermCandidateList.Add(tc6);

        }
    }

    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("CValues<br>");
            CValueCalculations ccalc = new CValueCalculations();
            ccalc.CreateList();
            ccalc.Calculate();
            Response.Write("<table><tr><td>C-value</td><td>P(Ta)</td><td>sum</td><td>Freq</td><td>Candidate terms</td></tr>");
            foreach (TermCandidate tc in ccalc.TermCandidateList)
                Response.Write("<tr><td>" + tc.Cval + "</td><td>"+tc.c+"</td><td>"+tc.t+"</td><td>"+tc.frequency+"</td><td>"+tc.s+"</td></tr>");
            Response.Write("</table>");
        }
    }
}
