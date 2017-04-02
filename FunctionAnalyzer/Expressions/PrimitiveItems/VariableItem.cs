using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.PrimitiveItems
{
    class VariableItem : RawExpression
    {
        public string Name
        {
            get
            {
                return name;
            }
        }
        private string name;
        public VariableItem(string name)
        {
            this.name = name;
        }

        public override RawExpression Apply(IDictionary<string, double> @params)
        {
            double value;
            if(@params.TryGetValue(name, out value))
            {
                return new NumberItem(value);
            }
            else
            {
                return this;
            }
        }

        public override RawExpression Differentiate(string varName)
        {
            if (name == varName)
            {
                return new NumberItem(1);
            }
            else
            {
                return new NumberItem(0);
            }
        }

        public override double Execute(IDictionary<string, double> @params)
        {
            return @params[name];
        }

        public override RawExpression SimplifyInternal()
        {
            return this;
        }

        public override Expression CompileInternal(IDictionary<string, Expression> @params)
        {
            return @params[name];
        }

        public override string ToString()
        {
            return name;
        }

        public override bool ContainsVariable(string varName)
        {
            return name == varName;
        }
    }
}
