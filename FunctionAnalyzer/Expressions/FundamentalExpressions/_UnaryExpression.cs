using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FundamentalExpressions
{
    abstract class _UnaryExpression : FundamentalExpression
    {
        protected RawExpression op;
        
        public _UnaryExpression(RawExpression op)
        {
            this.op = op;
        }

        public override RawExpression Apply(IDictionary<string, double> @params)
        {
            return (RawExpression)typeof(RawExpression).InvokeMember("op_" + OperNameAttr.Name
                , BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod
                , null, null, new object[] { op.Apply(@params) });
        }

        public override RawExpression Differentiate(string varName)
        {
            return (RawExpression)typeof(RawExpression).InvokeMember("op_" + OperNameAttr.Name
                , BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod
                , null, null, new object[] { op.Differentiate(varName) });
        }

        public override Expression CompileInternal(IDictionary<string, Expression> @params)
        {
            var methodInfo = typeof(Expression).GetMethod(ExpCallName
                , new Type[] { typeof(Expression) });
            return (Expression)methodInfo.Invoke(null, new object[] { op.CompileInternal(@params) });
        }

        public override string ToString()
        {
            return "(" + OperNameAttr.Symbol + op + ")";
        }

        public override bool ContainsVariable(string varName)
        {
            return op.ContainsVariable(varName);
        }
    }
}
