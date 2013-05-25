using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IssCalcCore.Tokens
{
    public class OperationToken : IToken, IDisposable
    {
        private Operations operation;
        public Operations Operation
        {
            get
            {
                return operation;
            }
        }

        public OperationToken(Operations operation)
        {
            this.operation = operation;

            SetPriority();
        }

        private int priority;

        private void SetPriority()
        {
            switch (operation) {
                case Operations.Add:
                    priority = 10;
                    break;
                case Operations.And:
                    priority = 15;
                    break;
                case Operations.Assign:
                    priority = 0;
                    break;
                case Operations.Div:
                    priority = 11;
                    break;
                case Operations.Exponentiation:
                    priority = 12;
                    break;
                case Operations.Sub:
                    priority = 10;
                    break;
                case Operations.Mul:
                    priority = 11;
                    break;
                case Operations.UMinus:
                    priority = 13;
                    break;
                case Operations.Not:
                    priority = 15;
                    break;
                case Operations.Or:
                    priority = 15;
                    break;
                case Operations.Xor:
                    priority = 15;
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

        public override bool Equals(object obj)
        {
            OperationToken token = obj as OperationToken;
            if (token != null && this.Operation == token.Operation) {
                return true;
            }

            return false;
        }


        public override string ToString()
        {
            return OperationAliases.FindAliasString(operation);
        }

        public void Dispose()
        {
            
        }
    }
}
