﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
    public enum Symbols
    {

        /// <summary>
        /// (
        /// </summary>
        OpenBracket,
        /// <summary>
        /// )
        /// </summary>
        CloseBracket,
        /// <summary>
        /// ,
        /// </summary>
        Comma,

    }

    public class SymbolToken : IToken
    {

        private Symbols symbol;
        private int priority;

        public SymbolToken(Symbols symbol)
        {
            this.symbol = symbol;

            SetPriority();
        }

        public override bool Equals(object obj)
        {
            SymbolToken token = obj as SymbolToken;
            if (token != null && this.Symbol == token.Symbol) {
                return true;
            }

            return false;
        }

        private void SetPriority()
        {
            switch (symbol) {
                case Symbols.OpenBracket:
                    priority = 1;
                    break;
                case Symbols.CloseBracket:
                    priority = 2;
                    break;
                case Symbols.Comma:
                    priority = 3;
                    break;
            }
        }

        public int Priority
        {
            get
            {
                return priority;
            }
        }

        public Symbols Symbol
        {
            get
            {
                return symbol;
            }
        }



        public override string ToString()
        {
            return SymbolAliases.FindAliasString(symbol);
        }
    }
}
