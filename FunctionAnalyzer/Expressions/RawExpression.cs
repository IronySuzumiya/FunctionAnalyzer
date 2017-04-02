using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using FunctionAnalyzer.Expressions.FundamentalExpressions;
using FunctionAnalyzer.Expressions.PrimitiveItems;

namespace FunctionAnalyzer.Expressions
{
    public class ExcutingException : Exception
    {
        public ExcutingException(string message) : base(message) { }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class ExpressionCallNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ExpressionCallNameAttribute(string name)
        {
            Name = name;
        }
    }

    public abstract class RawExpression
    {
        public string ExpCallName
        {
            get
            {
                var attr = (ExpressionCallNameAttribute)GetType().GetCustomAttributes(typeof(ExpressionCallNameAttribute), false).First();
                return attr.Name;
            }
        }

        abstract public RawExpression Apply(IDictionary<string, double> @params);

        abstract public bool ContainsVariable(string varName);

        abstract public RawExpression Differentiate(string varName);

        abstract public double Execute(IDictionary<string, double> @params);

        abstract public override string ToString();

        abstract public RawExpression SimplifyInternal();

        public RawExpression Simplify()
        {
            try
            {
                return new NumberItem(Execute(new Dictionary<string, double>()));
            }
            catch (KeyNotFoundException)
            {
                var s = SimplifyInternal();
                try
                {
                    return new NumberItem(s.Execute(new Dictionary<string, double>()));
                }
                catch (KeyNotFoundException)
                {
                    return s;
                }
                catch (ExcutingException ex)
                {
                    Console.WriteLine(ex.Message);
                    return new NumberItem(double.NaN);
                }
            }
            catch(ExcutingException ex)
            {
                Console.WriteLine(ex.Message);
                return new NumberItem(double.NaN);
            }
        }

        abstract public Expression CompileInternal(IDictionary<string, Expression> @params);

        public Func<double, double> Compile(string varName)
        {
            var parameters = new Dictionary<string, Expression>();
            var parameter = Expression.Parameter(typeof(double), varName);
            parameters.Add(varName, parameter);
            return Expression.Lambda<Func<double, double>>(CompileInternal(parameters), parameter).Compile();
        }

        public Func<double, double, double> Compile(string var1Name, string var2Name)
        {
            var parameters = new Dictionary<string, Expression>();
            var parameter1 = Expression.Parameter(typeof(double), var1Name);
            var parameter2 = Expression.Parameter(typeof(double), var2Name);
            parameters.Add(var1Name, parameter1);
            parameters.Add(var2Name, parameter2);
            return Expression.Lambda<Func<double, double, double>>(CompileInternal(parameters), parameter1, parameter2).Compile();
        }

        public static RawExpression operator +(RawExpression lhs, RawExpression rhs)
        {
            return new AddExpression(lhs, rhs);
        }

        public static RawExpression operator -(RawExpression lhs, RawExpression rhs)
        {
            return new SubExpression(lhs, rhs);
        }

        public static RawExpression operator *(RawExpression lhs, RawExpression rhs)
        {
            return new MultExpression(lhs, rhs);
        }

        public static RawExpression operator /(RawExpression lhs, RawExpression rhs)
        {
            return new DivExpression(lhs, rhs);
        }

        public static RawExpression operator -(RawExpression op)
        {
            return new NegExpression(op);
        }

        public static RawExpression operator ^(RawExpression lhs, RawExpression rhs)
        {
            return new PowerExpression(lhs, rhs);
        }
    }
}
