using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FundamentalExpressions
{
    abstract class _BinaryExpression : FundamentalExpression
    {
        protected RawExpression lhs;
        protected RawExpression rhs;

        public _BinaryExpression(RawExpression lhs, RawExpression rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public override RawExpression Apply(IDictionary<string, double> @params)
        {
            return (RawExpression)typeof(RawExpression).InvokeMember("op_" + OperNameAttr.Name
                , BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod
                , null, null, new object[] { lhs.Apply(@params), rhs.Apply(@params) });
        }

        public override RawExpression Differentiate(string varName)
        {
            return (RawExpression)typeof(RawExpression).InvokeMember("op_" + OperNameAttr.Name
                , BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod
                , null, null, new object[] { lhs.Differentiate(varName), rhs.Differentiate(varName) });
        }

        public override Expression CompileInternal(IDictionary<string, Expression> @params)
        {
            var methodInfo = typeof(Expression).GetMethod(ExpCallName
                , new Type[] { typeof(Expression), typeof(Expression) });
            return (Expression)methodInfo.Invoke(null, new object[] { lhs.CompileInternal(@params), rhs.CompileInternal(@params) });
        }

        public override string ToString()
        {
            return "(" + lhs + OperNameAttr.Symbol + rhs + ")";
        }

        public override bool ContainsVariable(string varName)
        {
            return lhs.ContainsVariable(varName) || rhs.ContainsVariable(varName);
        }
    }
}
