using System;
using System.Collections.Generic;
using System.Linq;
using IssCalcCore.Tokens;

namespace IssCalcCore
{
    public class ExpressionParser
    {
        private static  List<char> splitters_ = new List<char>() { '+', '-', '/', '*', '(', ')'};

        //private static ExpNode ParseStringToExpTree(string s)
        //{
        //    ExpNode root = new ExpNode();
        //    ParserNode pn = new ParserNode();

        //    bool hasDeviderError = false;

        //    //Expression.Set

        //    string[] tokens = ExpressionStringToStringArray(s);
        //    Queue<ExpNode> outQueue = new Queue<ExpNode>();
        //    Stack<ExpNode> funcStack = new Stack<ExpNode>();

        //    for (int i = 0; i < tokens.Length; i++) {
        //        double num;
        //        bool isNum = double.TryParse(tokens[i], out num);

        //        if (isNum) {
        //            outQueue.Enqueue(new ExpNode(num));
        //            continue;
        //        }
        //        if (functions.Contains(tokens[i])) {

        //        }
        //    }

        //    return root;
        //}

        public static string GetTokenizedRPNString(string s)
        {
            var a = ExpressionStringToStringArray(s);
            var b = StringArrayToTokenArray(a);
            var c = TokenArrayRPN(b);

            return String.Join(" ",c.Select(x => x.ToString()));
        }

        public static IEnumerable<IToken> TokenArrayRPN(IEnumerable<IToken> tokens)
        {
            var output = new List<IToken>();
            var stack = new Stack<IToken>();

            var openBracketToken = new SymbolToken(Symbols.OpenBracket);
            foreach (var token in tokens) {
                IToken stackToken;
                if (token is SymbolToken) {
                    var t = token as SymbolToken;
                    if (t.Symbol == Symbols.OpenBracket) {
                        stack.Push(token);
                    } else if (t.Symbol == Symbols.CloseBracket) {
                        if (stack.Count == 0) break;
                        stackToken = stack.Pop();
                        while (!stackToken.Equals(openBracketToken)) {
                            output.Add(stackToken);
                            if (stack.Count == 0) break;
                            stackToken = stack.Pop();
                        }
                    } else if (t.Symbol == Symbols.Comma) {
                        stackToken = stack.Pop();

                        while (!stackToken.Equals(openBracketToken)) {
                            output.Add(stackToken);
                            stackToken = stack.Pop();
                        }

                        stack.Push(stackToken);
                    }
                } else if (token is ValueToken || token is VariableToken) {
                    output.Add(token);
                } else {
                    while (stack.Count != 0 && (stackToken = stack.Peek()).Priority >= token.Priority) {
                        if (stackToken.Equals(openBracketToken))
                            break;
                        output.Add(stack.Pop());
                    }

                    stack.Push(token);
                }
            }
            if (stack.Count != 0) {
                output.AddRange(stack);
            }

            return output;
        }

        public static double Caclulate(IEnumerable<IToken> form, double x, double y, double p, double p2) {
            double res = 0;

            Stack<double> values = new Stack<double>();

            double first = 0, second = 0;

            foreach (var token in form) {
                if (token is VariableToken) {
                    using (VariableToken otoken = token as VariableToken) {
                        switch (otoken.Var) {
                            case "x":
                                values.Push(x);
                                break;
                            case "y":
                                values.Push(y);
                                break; 
                            case "e":
                                values.Push(Math.E);
                                break;
                            case "pi":
                                values.Push(Math.PI);
                                break;
                        }
                    }
                } else if(token is ValueToken) {
                    values.Push((token as ValueToken).Number);
                } else if (token is OperationToken && values.Count > 1) {
                    using(OperationToken otoken = token as OperationToken) {
                        switch (otoken.Operation) {
                                case Operations.Add:
                                    first = values.Pop();
                                    second = values.Pop();
                                    values.Push(first+second);
                                break;
                                case Operations.Mul:
                                    first = values.Pop();
                                    second = values.Pop();
                                    values.Push(first * second);
                                break;
                                case Operations.Sub:
                                    first = values.Pop();
                                    second = values.Pop();
                                    values.Push(first - second);
                                break;
                                case Operations.Div:
                                    first = values.Pop();
                                    second = values.Pop();
                                    values.Push(first / second);
                                break;
                        }
                    }
                } else if (token is FunctionToken && values.Count > 0) {
                    using (FunctionToken otoken = token as FunctionToken) {
                        switch (otoken.Function) {
                            case Functions.Sin:
                                    first = values.Pop();
                                    values.Push(Math.Sin(first));
                                break;
                            case Functions.Cos:
                                first = values.Pop();
                                values.Push(Math.Cos(first));
                                break;
                            case Functions.Asin:
                                first = values.Pop();
                                values.Push(Math.Asin(first));
                                break;
                            case Functions.Tg:
                                first = values.Pop();
                                values.Push(Math.Tan(first));
                                break;
                            case Functions.Sh:
                                first = values.Pop();
                                values.Push(Math.Sinh(first));
                                break;
                            case Functions.Ch:
                                first = values.Pop();
                                values.Push(Math.Cosh(first));
                                break;
                            case Functions.Sqrt:
                                first = values.Pop();
                                values.Push(Math.Sqrt(first));
                                break;
                        }
                    }
                }
            }

            if (values.Count == 1) {
                return values.Pop();
            }
            return 0;
        }

        public static IEnumerable<IToken> StringArrayToTokenArray(IEnumerable<string> str)
        {
            var tokens = new List<IToken>();

            foreach (string s in str) {
                double val;
                if (double.TryParse(s, out val)) {
                    tokens.Add(new ValueToken(val));
                    continue;
                }
                if (s.Length == 1) {

                    if (OperationAliases.Contains(s)) {
                        tokens.Add(new OperationToken(OperationAliases.GetToken(s)));
                        continue;
                    }
                    if (SymbolAliases.Contains(s)) {
                        tokens.Add(new SymbolToken(SymbolAliases.GetToken(s)));
                        continue;
                    }
                }
                if (FunctionAliases.Contains(s)) {
                    tokens.Add(new FunctionToken(FunctionAliases.GetToken(s)));
                    continue;
                }

                tokens.Add(new VariableToken(s));
                continue;
            }

            return tokens;
        }

        public static IEnumerable<string> ExpressionStringToStringArray(string s)
        {
            s = s.ToLower().Trim().Replace(" ", "");

            var exp = new List<string>();
            string reader = "";
            bool isNowLetter = false;
            bool isNextExp = true;

            foreach (char t in s) {
                bool isPreviousLetter = isNowLetter;
                if (splitters_.Contains(t)) {
                    if (reader.Length != 0) {
                        exp.Add(reader);
                        reader = "";
                    }
                    exp.Add(Convert.ToString(t));
                    isNowLetter = false;
                    isNextExp = true;
                } else {
                    isNowLetter = char.IsLetter(t);
                    if (isNowLetter == isPreviousLetter || isNextExp) {
                        reader += t;
                        isNextExp = false;
                    } else {
                        exp.Add(reader);
                        reader = "";
                        reader += t;
                        isNextExp = true;
                    }
                }
            }
            if (reader.Length != 0) {
                exp.Add(reader);
            }

            return exp;
        }
    }

    public interface IExpNode
    {
        double Calculate();
        IExpNode Clone();
        IExpNode Parent { get; set; }
    }


    public class ExpValue : IExpNode
    {
        double? value_;


        public double Calculate()
        {
            throw new NotImplementedException();
        }

        public IExpNode Clone()
        {
            throw new NotImplementedException();
        }

        public IExpNode Parent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
