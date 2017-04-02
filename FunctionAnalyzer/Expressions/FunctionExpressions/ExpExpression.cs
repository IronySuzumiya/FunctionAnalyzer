using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FunctionExpressions
{
    [FunctionName("exp")]
    [ExpressionCallName("Exp")]
    class ExpExpression : FunctionExpression
    {
        public ExpExpression() { }
        public ExpExpression(RawExpression op) : base(op) { }

        public override RawExpression Differentiate(string varName)
        {
            return op.Differentiate(varName) * new ExpExpression(op);
        }
    }
}
