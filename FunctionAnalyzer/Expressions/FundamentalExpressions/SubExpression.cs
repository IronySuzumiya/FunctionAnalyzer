using FunctionAnalyzer.Expressions.PrimitiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FundamentalExpressions
{
    [OperationName("Subtraction", "-")]
    [ExpressionCallName("Subtract")]
    class SubExpression : _BinaryExpression
    {
        public SubExpression(RawExpression lhs, RawExpression rhs) : base(lhs, rhs) { }

        public override double Execute(IDictionary<string, double> @params)
        {
            return lhs.Execute(@params) - rhs.Execute(@params);
        }

        public override RawExpression SimplifyInternal()
        {
            var lhs = this.lhs.Simplify();
            var rhs = this.rhs.Simplify();
            var lhsAsNum = lhs as NumberItem;
            var rhsAsNum = rhs as NumberItem;
            var lhsAsVar = lhs as VariableItem;
            var rhsAsVar = rhs as VariableItem;
            if(lhsAsNum?.Value == 0)
            {
                return -rhs;
            }
            else if(rhsAsNum?.Value == 0)
            {
                return lhs;
            }
            else if(lhsAsVar != null && lhsAsVar.Name == rhsAsVar?.Name)
            {
                return new NumberItem(0);
            }
            else
            {
                return lhs - rhs;
            }
        }
    }
}
