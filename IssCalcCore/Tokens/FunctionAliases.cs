using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
    public static class FunctionAliases
    {
        private static Dictionary<string, Functions> aliases = new Dictionary<string, Functions> {
                                                                                                    {"sin", Functions.Sin},
                                                                                                    {"sine", Functions.Sin},

                                                                                                    {"cos", Functions.Cos},
                                                                                                    {"cosine", Functions.Cos},

                                                                                                    {"arcsin", Functions.Asin},
                                                                                                    {"asin", Functions.Asin},

                                                                                                    {"tg",Functions.Tg},
                                                                                                    {"ctg",Functions.Ctg},
                                                                                                    {"sh",Functions.Sh},
                                                                                                    {"ch",Functions.Ch},
                                                                                                    {"sqrt",Functions.Sqrt}
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

        internal static Functions GetToken(string s)
        {
            return aliases[s];
        }

        internal static string FindAliasString(Functions symbol)
        {
            return aliases.First(x => x.Value == symbol).Key;
        }
    }
}
