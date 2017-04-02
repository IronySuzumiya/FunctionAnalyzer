using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FundamentalExpressions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class OperationNameAttribute : Attribute
    {
        public string Name { get; set; }
        public string Symbol { get; set; }

        public OperationNameAttribute(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }

    abstract class FundamentalExpression : RawExpression
    {
        public OperationNameAttribute OperNameAttr
        {
            get
            {
                return (OperationNameAttribute)GetType().GetCustomAttributes(typeof(OperationNameAttribute), false).First();
            }
        }
    }
}
