using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
    public static class SymbolAliases
    {
        private static Dictionary<string, Symbols> aliases = new Dictionary<string, Symbols> {
                                                                                                    {",", Symbols.Comma},
                                                                                                    {"(", Symbols.OpenBracket},
                                                                                                    {")", Symbols.CloseBracket},
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

        internal static Symbols GetToken(string s)
        {
            return aliases[s];
        }

        internal static string FindAliasString(Symbols symbol)
        {
            return aliases.First(x => x.Value == symbol).Key;
        }
    }
}
