using FunctionAnalyzer.Expressions.PrimitiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FunctionExpressions
{
    [FunctionName("ln")]
    [ExpressionCallName("Log")]
    class LnExpression : FunctionExpression
    {
        public LnExpression() { }
        public LnExpression(RawExpression op) : base(op) { }

        public override RawExpression DifferentiateInternal(string varName)
        {
            return (new NumberItem(1) / op) * op.Differentiate(varName);
        }
    }
}
