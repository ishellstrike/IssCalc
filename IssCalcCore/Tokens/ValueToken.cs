using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
    public class ValueToken : IToken
    {
        public ValueToken(double val)
        {
            number = val;
        }

        public ValueToken() { }

        private double number;
        public double Number
        {
            get
            {
                return number;
            }
        }

        public int Priority
        {
            get
            {
                return 101;
            }
        }

        public override bool Equals(object obj)
        {
            ValueToken token = obj as ValueToken;
            if (token != null && this.number == token.Number) {
                return true;
            }

            return false;
        }


        public override string ToString()
        {
            return number.ToString();
        }
    }
}
