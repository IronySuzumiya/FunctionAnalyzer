using FunctionAnalyzer.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FunctionAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                string e = "(k1 *Cs1 + k2 * Co2) / (k1 * C1 + k2 * C2)";
                {
                    RawExpression exp = ExpressionParser.Parse(e);
                    RawExpression dk1 = exp.Differentiate("k1").Simplify();
                    RawExpression dk2 = exp.Differentiate("k2").Simplify();

                    Console.WriteLine("input:\t\t" + e);
                    Console.WriteLine("parse:\t\t" + exp);
                    Console.WriteLine("dk1:\t\t" + dk1);
                    Console.WriteLine("dk2:\t\t" + dk2);
                }

                // Cs1 = 128, Co2 = 1024, C1 = 256, C2 = 2048
                Dictionary<string, double> values = new Dictionary<string, double>
                {
                    {"Cs1", 128},
                    {"Co2", 1024},
                    {"C1", 256},
                    {"C2", 2048},
                };
                foreach (var p in values)
                {
                    Console.WriteLine("{0} => {1}", p.Value, p.Key);
                }
                {
                    RawExpression exp = ExpressionParser.Parse(e).Apply(values).Simplify();
                    RawExpression dk1 = exp.Differentiate("k1").Simplify();
                    RawExpression dk2 = exp.Differentiate("k2").Simplify();
                    Console.WriteLine("applied:\t" + exp);
                    Console.WriteLine("dk1:\t\t" + dk1);
                    Console.WriteLine("dk2:\t\t" + dk2);
                }
            }
            {
                string[] expressions =
                {
                    "1",
                    "1.2",
                    "(-3)",
                    "12+34",
                    "56*78",
                    "x^y",
                    "x+2*y+4",
                    "(x+y)*(3+4)",
                    "exp(x+y)",
                    "x+ln(y)",
                    "x/y",
                    "exp(x)/ln(x)",
                    "sin(x)",
                    "cos(x)",
                    "tan(x)",
                    "cot(x)"
                };
                foreach (var e in expressions)
                {
                    RawExpression exp = ExpressionParser.Parse(e);
                    Console.WriteLine("input:\t\t" + e);
                    Console.WriteLine("parse:\t\t" + exp);
                    Console.WriteLine("simplified:\t" + exp.Simplify());
                    Console.WriteLine("dx:\t\t" + exp.Differentiate("x").Simplify());
                    Console.WriteLine("dy:\t\t" + exp.Differentiate("y").Simplify());
                    Console.WriteLine("no x:\t\t" + exp.Apply("x", 0).Simplify());
                    Console.WriteLine("no y:\t\t" + exp.Apply("y", 0).Simplify());
                    Console.WriteLine();

                    Debug.Assert(exp.ToString() == ExpressionParser.Parse(exp.ToString()).ToString());
                }
            }
            {
                string[] expressions =
                {
                    "(x-3)*(x-5)",
                    "ln(x)/ln(2)-3",
                    "(x-5)^2-4",
                };
                foreach (var e in expressions)
                {
                    RawExpression exp = ExpressionParser.Parse(e);
                    Console.WriteLine("input:\t\t" + e);
                    Console.WriteLine("parse:\t\t" + exp);
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine("solve:\t\t" + i + " => " + exp.Solve("x", i));
                    }
                    Console.WriteLine();
                }
            }
            {
                string[] expressions =
                {
                    "x^2",
                    "x+4-2/x",
                    "x*(3+4*x)",
                    "exp(x+1/x)",
                    "x+ln(5)",
                    "exp(x)/ln(x)"
                };
                foreach (var e in expressions)
                {
                    RawExpression exp = ExpressionParser.Parse(e);
                    Console.WriteLine("input:\t\t" + e);
                    Console.WriteLine("parse:\t\t" + exp);
                    Console.WriteLine("simplified:\t" + exp.Simplify());
                    for(int i = 0; i < 10; ++i)
                    {
                        Console.WriteLine($"{i} => x:\t\t{exp.Compile("x").Invoke(i)}");
                    }
                    Console.WriteLine();

                    Debug.Assert(exp.ToString() == ExpressionParser.Parse(exp.ToString()).ToString());
                }
            }
            {
                string[] expressions =
                {
                    "x^2+y",
                    "x+4-2/y",
                    "x*(3+4*y)",
                    "exp(x+1/y)",
                    "x*ln(20-y*2)",
                    "exp(x)/ln(y)"
                };
                foreach (var e in expressions)
                {
                    RawExpression exp = ExpressionParser.Parse(e);
                    Console.WriteLine("input:\t\t" + e);
                    Console.WriteLine("parse:\t\t" + exp);
                    Console.WriteLine("simplified:\t" + exp.Simplify());
                    for (int i = 1; i < 6; ++i)
                    {
                        for(int j = 1; j < 6; ++j)
                        {
                            Console.WriteLine($"{i} => x, {j} => y:\t{exp.Compile("x", "y").Invoke(i, j)}");
                        }
                    }
                    Console.WriteLine();

                    Debug.Assert(exp.ToString() == ExpressionParser.Parse(exp.ToString()).ToString());
                }
            }
        }
    }
}
