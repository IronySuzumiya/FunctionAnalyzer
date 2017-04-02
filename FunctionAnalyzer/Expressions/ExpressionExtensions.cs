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

        public static double Solve(this RawExpression exp, string varName, double start)
        {
            return exp.Simplify().Solve(exp.Differentiate(varName).Simplify(), varName, start);
        }

        public static double Solve(this RawExpression f, RawExpression df, string varName, double start)
        {
            return f.Compile(varName).Solve(df.Compile(varName), start);
        }

        public static double Solve(this Func<double, double> f, Func<double, double> df, double start
            , double EPS = 1E-10, int ITER_MAX_COUNT = 100)
        {
            int cnt = 0;
            double fn, dfn;
            fn = f(start);
            while ((fn > EPS || fn < -EPS) && cnt < ITER_MAX_COUNT)
            {
                dfn = df(start);
                start -= fn / dfn;
                fn = f(start);
                ++cnt;
            }
            if (cnt >= ITER_MAX_COUNT)
            {
                return double.NaN;
            }
            else
            {
                return start;
            }
        }
    }
}
