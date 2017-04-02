using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FundamentalExpressions
{
    [OperationName("UnaryNegation", "-")]
    [ExpressionCallName("Negate")]
    class NegExpression : _UnaryExpression
    {
        public NegExpression(RawExpression op) : base(op) { }

        public override RawExpression SimplifyInternal()
        {
            return -op.Simplify();
        }

        public override double Execute(IDictionary<string, double> @params)
        {
            return -op.Execute(@params);
        }
    }
}
