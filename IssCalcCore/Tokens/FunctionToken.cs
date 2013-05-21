using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
    public class FunctionToken : IToken
    {

        private Functions function;

        public FunctionToken(Functions func)
        {
            function = func;
        }
        public Functions Function
        {
            get
            {
                return function;
            }

            set
            {
                function = value;
            }
        }

        public int Priority
        {
            get
            {
                return 100;
            }
        }

        public override bool Equals(object obj)
        {
            FunctionToken token = obj as FunctionToken;
            if (token != null && this.Function == token.Function) {
                return true;
            }

            return false;
        }



        public override string ToString()
        {
            return FunctionAliases.FindAliasString(function);
        }
    }
}
