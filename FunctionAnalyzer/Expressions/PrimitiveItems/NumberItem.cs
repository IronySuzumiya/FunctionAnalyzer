using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.PrimitiveItems
{
    class NumberItem : RawExpression
    {
        public double Value
        {
            get
            {
                return value;
            }
        }
        private double value;
        public NumberItem(double value)
        {
            this.value = value;
        }

        public override RawExpression Apply(IDictionary<string, double> @params)
        {
            return this;
        }

        public override RawExpression Differentiate(string varName)
        {
            return new NumberItem(0);
        }

        public override double Execute(IDictionary<string, double> @params)
        {
            return value;
        }

        public override RawExpression SimplifyInternal()
        {
            return this;
        }

        public override Expression CompileInternal(IDictionary<string, Expression> @params)
        {
            return Expression.Constant(value);
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override bool ContainsVariable(string varName)
        {
            return false;
        }
    }
}
