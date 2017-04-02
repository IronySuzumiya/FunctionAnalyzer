using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions
{
    public static class ExpressionExtensions
    {
        public static RawExpression Apply(this RawExpression exp, string varName, double varValue)
        {
            return exp.Apply(new Dictionary<string, double> { { varName, varValue } });
        }

        public static double Execute(this RawExpression exp, string varName, double varValue)
        {
            return exp.Execute(new Dictionary<string, double> { { varName, varValue } });
        }

        public static double Solve(this RawExpression exp, string varName, double varValue, double EPS = 1E-15, int ITER_MAX_COUNT = 100)
        {
            try
            {
                int cnt = 0;
                double fn, dfn;
                fn = exp.Execute(varName, varValue);
                while ((fn > EPS || fn < -EPS) && cnt < ITER_MAX_COUNT)
                {
                    dfn = exp.Differentiate(varName).Execute(varName, varValue);
                    varValue -= fn / dfn;
                    fn = exp.Execute(varName, varValue);
                    ++cnt;
                }
                if(cnt >= ITER_MAX_COUNT)
                {
                    return double.NaN;
                }
                else
                {
                    return varValue;
                }
            }
            catch(ExcutingException ex)
            {
                Console.WriteLine(ex.Message);
                return double.NaN;
            }
        }
    }
}
