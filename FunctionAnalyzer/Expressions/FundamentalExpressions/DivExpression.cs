using FunctionAnalyzer.Expressions.PrimitiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FundamentalExpressions
{
    [OperationName("Division", "/")]
    [ExpressionCallName("Divide")]
    class DivExpression : _BinaryExpression
    {
        public DivExpression(RawExpression lhs, RawExpression rhs) : base(lhs, rhs) { }

        public override double Execute(IDictionary<string, double> @params)
        {
            var denominator = rhs.Execute(@params);
            if(denominator != 0)
            {
                return lhs.Execute(@params) / denominator;
            }
            else
            {
                throw new ExcutingException(lhs + " divided by 0 is NaN");
            }
        }

        public override RawExpression Differentiate(string varName)
        {
            return (lhs.Differentiate(varName) * rhs - lhs * rhs.Differentiate(varName)) / (rhs ^ new NumberItem(2));
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
                return new NumberItem(0);
            }
            else if(rhsAsNum?.Value == 1)
            {
                return lhs;
            }
            else if(lhsAsVar != null && lhsAsVar.Name == rhsAsVar?.Name)
            {
                return new NumberItem(1);
            }
            else
            {
                return lhs / rhs;
            }
        }
    }
}
