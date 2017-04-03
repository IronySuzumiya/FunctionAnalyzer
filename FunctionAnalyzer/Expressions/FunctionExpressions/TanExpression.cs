using FunctionAnalyzer.Expressions.PrimitiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FunctionExpressions
{
    [FunctionName("tan")]
    [ExpressionCallName("Tan")]
    class TanExpression : FunctionExpression
    {
        public TanExpression() { }
        public TanExpression(RawExpression op) : base(op) { }

        public override RawExpression DifferentiateInternal(string varName)
        {
            return new NumberItem(1) / (new CosExpression(op) ^ new NumberItem(2));
        }
    }
}
