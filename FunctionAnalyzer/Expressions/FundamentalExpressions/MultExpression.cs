using FunctionAnalyzer.Expressions.PrimitiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FundamentalExpressions
{
    [OperationName("Multiply", "*")]
    [ExpressionCallName("Multiply")]
    class MultExpression : _BinaryExpression
    {
        public MultExpression(RawExpression lhs, RawExpression rhs) : base(lhs, rhs) { }

        public override RawExpression Differentiate(string varName)
        {
            return (lhs * rhs.Differentiate(varName)) + (lhs.Differentiate(varName) * rhs);
        }

        public override double Execute(IDictionary<string, double> @params)
        {
            return lhs.Execute(@params) * rhs.Execute(@params);
        }

        public override RawExpression SimplifyInternal()
        {
            var lhs = this.lhs.Simplify();
            var rhs = this.rhs.Simplify();
            var lhsAsNum = lhs as NumberItem;
            var rhsAsNum = rhs as NumberItem;
            var lhsAsVar = lhs as VariableItem;
            var rhsAsVar = rhs as VariableItem;

            if(lhsAsNum?.Value == 0 || rhsAsNum?.Value == 0)
            {
                return new NumberItem(0);
            }
            else if(lhsAsNum?.Value == 1)
            {
                return rhs;
            }
            else if(rhsAsNum?.Value == 1)
            {
                return lhs;
            }
            else if(lhsAsVar != null && lhsAsVar.Name == rhsAsVar?.Name)
            {
                return lhsAsVar ^ new NumberItem(2);
            }
            else
            {
                return lhs * rhs;
            }
        }
    }
}
