using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
    public interface IToken
    {
        int Priority
        {
            get;
        }
    }
}
