using FunctionAnalyzer.Expressions.PrimitiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAnalyzer.Expressions.FunctionExpressions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class FunctionNameAttribute : Attribute
    {
        public string Name { get; set; }

        public FunctionNameAttribute(string name)
        {
            Name = name;
        }
    }

    abstract class FunctionExpression : RawExpression
    {
        public RawExpression Op
        {
            get
            {
                return op;
            }
            set
            {
                op = value;
            }
        }
        protected RawExpression op;

        public string Name
        {
            get
            {
                var attr = (FunctionNameAttribute)GetType().GetCustomAttributes(typeof(FunctionNameAttribute), false).First();
                return attr.Name;
            }
        }

        public static IDictionary<string, Type> funcDictionary = null;

        public static FunctionExpression FromName(string funcName)
        {
            if(funcDictionary == null)
            {
                funcDictionary = new Dictionary<string, Type>();
                foreach(var type in typeof(FunctionExpression).Assembly.GetTypes())
                {
                    if(!type.IsAbstract && typeof(FunctionExpression).IsAssignableFrom(type))
                    {
                        var attr = (FunctionNameAttribute)type.GetCustomAttributes(typeof(FunctionNameAttribute), false).First();
                        funcDictionary.Add(attr.Name, type);
                    }
                }
            }
            Type funcType = null;
            if(funcDictionary.TryGetValue(funcName, out funcType))
            {
                return (FunctionExpression)funcType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            else
            {
                return null;
            }
        }

        public FunctionExpression() { }

        public FunctionExpression(RawExpression op)
        {
            this.op = op;
        }

        public override RawExpression Apply(IDictionary<string, double> @params)
        {
            var func = (FunctionExpression)GetType().GetConstructor(new Type[] { }).Invoke(new object[] { });
            func.op = op.Apply(@params);
            return func;
        }

        public override double Execute(IDictionary<string, double> @params)
        {
            return (double)typeof(Math).InvokeMember(ExpCallName, BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod
                , null, null, new object[] { op.Execute(@params) });
        }

        public override Expression CompileInternal(IDictionary<string, Expression> @params)
        {
            return Expression.Call(typeof(Math).GetMethod(ExpCallName, new Type[] { typeof(double) })
                , op.CompileInternal(@params));
        }

        public override RawExpression SimplifyInternal()
        {
            var func = (FunctionExpression)GetType().GetConstructor(new Type[] { }).Invoke(new object[] { });
            func.op = op.SimplifyInternal();
            return func;
        }

        public override string ToString()
        {
            return Name + "(" + op + ")";
        }

        public override bool ContainsVariable(string varName)
        {
            return op.ContainsVariable(varName);
        }

        public override RawExpression Differentiate(string varName)
        {
            if(op.ContainsVariable(varName))
            {
                return DifferentiateInternal(varName);
            }
            else
            {
                return new NumberItem(0);
            }
        }

        abstract public RawExpression DifferentiateInternal(string varName);
    }
}
