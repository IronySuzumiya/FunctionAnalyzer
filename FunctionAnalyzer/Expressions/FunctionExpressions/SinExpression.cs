using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FunctionExpressions
{
    [FunctionName("sin")]
    [ExpressionCallName("Sin")]
    class SinExpression : FunctionExpression
    {
        public SinExpression() { }
        public SinExpression(RawExpression op) : base(op) { }

        public override RawExpression Differentiate(string varName)
        {
            return op.Differentiate(varName) * new CosExpression(op);
        }
    }
}
