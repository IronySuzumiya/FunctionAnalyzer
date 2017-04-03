using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionAnalyzer.Expressions;
using FunctionAnalyzer.Expressions.FundamentalExpressions;
using FunctionAnalyzer.Expressions.PrimitiveItems;
using FunctionAnalyzer.Expressions.FunctionExpressions;

namespace FunctionAnalyzer
{
    public class ParsingException : Exception
    {
        public ParsingException(string message) : base(message) { }
    }

    public unsafe static class ExpressionParser
    {
        private static bool Is(char** reader, char obj)
        {
            if(**reader == obj)
            {
                (*reader)++;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static double ConsumeNumber(char** reader)
        {
            bool floating = false;
            var numberString = new StringBuilder();
            while(*reader != null)
            {
                if (char.IsDigit(**reader))
                {
                    numberString.Append(**reader);
                }
                else if (!floating && **reader == '.')
                {
                    numberString.Append(**reader);
                    floating = true;
                }
                else
                {
                    break;
                }
                (*reader)++;
            }
            if(numberString.Length == 0)
            {
                return double.NaN;
            }
            else
            {
                return double.Parse(numberString.ToString());
            }
        }

        private static string ConsumeName(char** reader)
        {
            bool started = false;
            var nameString = new StringBuilder();
            while (*reader != null)
            {
                if ((**reader >= 'a' && **reader <= 'z')
                    || (**reader >= 'A' && **reader <= 'Z')
                    || **reader == '_')
                {
                    nameString.Append(**reader);
                    started = true;
                }
                else if(started && char.IsDigit(**reader))
                {
                    nameString.Append(**reader);
                }
                else
                {
                    break;
                }
                (*reader)++;
            }
            return nameString.ToString();
        }

        private static RawExpression InBracketParse(char** reader)
        {
            var negCount = 0;
            while (Is(reader, '-'))
            {
                negCount++;
            }
            if (negCount == 0)
            {
                return Exp3Parse(reader);
            }
            else if (negCount % 2 == 0)
            {
                return Exp0Parse(reader);
            }
            else // negCount % 2 == 1
            {
                return new NegExpression(Exp0Parse(reader));
            }
        }

        private static RawExpression Exp0Parse(char** reader)
        {
            if (Is(reader, '('))
            {
                var exp = InBracketParse(reader);
                if (!Is(reader, ')'))
                {
                    var tail = new string(*reader);
                    if (tail != "")
                    {
                        throw new ParsingException("Expect a \")\" at " + tail);
                    }
                    else
                    {
                        throw new ParsingException("Except a \")\" at the end");
                    }
                }
                else
                {
                    return exp;
                }
            }
            else
            {
                var number = ConsumeNumber(reader);
                if(!double.IsNaN(number))
                {
                    return new NumberItem(number);
                }
                else
                {
                    var name = ConsumeName(reader);
                    if(name != string.Empty)
                    {
                        if(Is(reader, '('))
                        {
                            var func = FunctionExpression.FromName(name);
                            if(func != null)
                            {
                                func.Op = InBracketParse(reader);
                            }
                            else
                            {
                                throw new ParsingException("Unknown function name " + name);
                            }
                            if(!Is(reader, ')'))
                            {
                                var tail = new string(*reader);
                                if(tail != "")
                                {
                                    throw new ParsingException("Expect a \")\" at " + tail);
                                }
                                else
                                {
                                    throw new ParsingException("Except a \")\" at the end");
                                }
                            }
                            else
                            {
                                return func;
                            }
                        }
                        else
                        {
                            return new VariableItem(name);
                        }
                    }
                    else
                    {
                        var tail = new string(*reader);
                        if (tail != "")
                        {
                            throw new ParsingException("Invalid Symbol at " + tail);
                        }
                        else
                        {
                            throw new ParsingException("Invalid Symbol at the end");
                        }
                    }
                }
            }
        }
        
        private static RawExpression Exp1Parse(char** reader)
        {
            var exp = Exp0Parse(reader);
            while (*reader != null)
            {
                if (Is(reader, '^'))
                {
                    exp ^= Exp0Parse(reader);
                }
                else
                {
                    break;
                }
            }
            return exp;
        }

        private static RawExpression Exp2Parse(char** reader)
        {
            var exp = Exp1Parse(reader);
            while (*reader != null)
            {
                if (Is(reader, '*'))
                {
                    exp *= Exp1Parse(reader);
                }
                else if (Is(reader, '/'))
                {
                    exp /= Exp1Parse(reader);
                }
                else
                {
                    break;
                }
            }
            return exp;
        }

        private static RawExpression Exp3Parse(char** reader)
        {
            var exp = Exp2Parse(reader);
            while (*reader != null)
            {
                if(Is(reader, '+'))
                {
                    exp += Exp2Parse(reader);
                }
                else if(Is(reader, '-'))
                {
                    exp -= Exp2Parse(reader);
                }
                else
                {
                    break;
                }
            }
            return exp;
        }

        private static RawExpression UnsafeParse(char* input)
        {
            return Exp3Parse(&input);
        }

        public static RawExpression Parse(string origFuncString)
        {
            var funcString = origFuncString.Replace(" ", "");
            fixed(char* input = funcString)
            {
                return UnsafeParse(input);
            }
        }
    }
}
