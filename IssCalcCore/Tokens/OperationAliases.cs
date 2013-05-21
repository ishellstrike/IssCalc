using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
    public static class OperationAliases
    {
        private static Dictionary<string, Operations> aliases = new Dictionary<string, Operations> {
                                                                                                    {"+", Operations.Add},
                                                                                                    {"-", Operations.Sub},
                                                                                                    {"/", Operations.Div},
                                                                                                    {"*", Operations.Mul},
                                                                                                 };
        private static IEnumerable<string> aliasesList = aliases.Select(x => x.Key);

        public static IEnumerable<string> GetAliasesList()
        {
            return aliasesList;
        }

        internal static bool Contains(string s)
        {
            return aliasesList.Contains(s);
        }

        internal static Operations GetToken(string s)
        {
            return aliases[s];
        }

        internal static string FindAliasString(Operations symbol)
        {
            return aliases.First(x => x.Value == symbol).Key;
        }
    }
}
