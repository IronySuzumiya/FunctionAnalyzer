using FunctionAnalyzer.Expressions.PrimitiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace FunctionAnalyzer.Expressions.FunctionExpressions
{
    [FunctionName("cot")]
    [ExpressionCallName("Cot")] // place-holder
    class CotExpression : FunctionExpression
    {
        public CotExpression() { }
        public CotExpression(RawExpression op) : base(op) { }

        public override RawExpression DifferentiateInternal(string varName)
        {
            return -((new NumberItem(1) / (new SinExpression(op) ^ new NumberItem(2))) * op.Differentiate(varName));
        }

        public override double Execute(IDictionary<string, double> @params)
        {
            var denominator = Math.Tan(op.Execute(@params));
            if(denominator != 0)
            {
                return 1 / denominator;
            }
            else
            {
                return double.NaN;
            }
        }

        public override Expression CompileInternal(IDictionary<string, Expression> @params)
        {
            return Expression.Divide(
                Expression.Constant(1.0),
                Expression.Call(typeof(Math).GetMethod("Tan", new Type[] { typeof(double) }), op.CompileInternal(@params))
                );
        }
    }
}
