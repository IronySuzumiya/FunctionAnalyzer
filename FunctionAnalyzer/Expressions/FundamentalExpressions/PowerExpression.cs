using FunctionAnalyzer.Expressions.FunctionExpressions;
using FunctionAnalyzer.Expressions.PrimitiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace FunctionAnalyzer.Expressions.FundamentalExpressions
{
    [OperationName("ExclusiveOr", "^")]
    [ExpressionCallName("Pow")]
    class PowerExpression : _BinaryExpression
    {
        public PowerExpression(RawExpression lhs, RawExpression rhs) : base(lhs, rhs) { }

        public override RawExpression Differentiate(string varName)
        {
            var lhs = this.lhs.Simplify();
            var rhs = this.rhs.Simplify();
            bool lf = lhs.ContainsVariable(varName);
            bool rf = rhs.ContainsVariable(varName);
            if (lf)
            {
                if (rf)
                {
                    return new ExpExpression(rhs * new LnExpression(lhs)).Differentiate(varName);
                }
                else
                {
                    return rhs * (lhs ^ (rhs - new NumberItem(1)));
                }
            }
            else
            {
                if (rf)
                {
                    return this * new LnExpression(lhs);
                }
                else
                {
                    return new NumberItem(0);
                }
            }
        }

        public override Expression CompileInternal(IDictionary<string, Expression> @params)
        {
            return Expression.Call(typeof(Math).GetMethod(ExpCallName, new Type[] { typeof(double), typeof(double) })
                , lhs.CompileInternal(@params), rhs.CompileInternal(@params));
        }

        public override double Execute(IDictionary<string, double> @params)
        {
            var lhsValue = lhs.Execute(@params);
            var rhsValue = rhs.Execute(@params);
            if(lhsValue == 0 && rhsValue <= 0)
            {
                throw new ExcutingException("0 to the " + rhsValue + " is NaN");
            }
            return Math.Pow(lhsValue, rhsValue);
        }

        public override RawExpression SimplifyInternal()
        {
            var lhs = this.lhs.Simplify();
            var rhs = this.rhs.Simplify();
            var lhsAsNum = lhs as NumberItem;
            var rhsAsNum = rhs as NumberItem;

            if(lhsAsNum?.Value == 0 && rhsAsNum?.Value <= 0)
            {
                throw new ExcutingException("0 to the " + rhsAsNum.Value + " is NaN");
            }
            else if(lhsAsNum?.Value == 0)
            {
                return new NumberItem(0);
            }
            else if(lhsAsNum?.Value == 1)
            {
                return new NumberItem(1);
            }
            else if(rhsAsNum?.Value == 0)
            {
                return new NumberItem(1);
            }
            else if(rhsAsNum?.Value == 1)
            {
                return lhs;
            }
            else
            {
                return lhs ^ rhs;
            }
        }
    }
}
