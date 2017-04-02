using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FunctionExpressions
{
    [FunctionName("cos")]
    [ExpressionCallName("Cos")]
    class CosExpression : FunctionExpression
    {
        public CosExpression() { }
        public CosExpression(RawExpression op) : base(op) { }

        public override RawExpression Differentiate(string varName)
        {
            return -(op.Differentiate(varName) * new SinExpression(op));
        }
    }
}
