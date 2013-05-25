using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
        public class VariableToken : IToken, IDisposable
        {

            private string var;

            public VariableToken(string symbol)
            {
                this.var = symbol;

            }

            public override bool Equals(object obj)
            {
                VariableToken token = obj as VariableToken;
                if (token != null && this.var == token.var) {
                    return true;
                }

                return false;
            }


            public int Priority
            {
                get
                {
                    return 100;
                }
            }

            public string Var
            {
                get
                {
                    return var;
                }
            }



            public override string ToString()
            {
                return Convert.ToString(var);
            }

            public void Dispose()
            {

            }
        }
    }

