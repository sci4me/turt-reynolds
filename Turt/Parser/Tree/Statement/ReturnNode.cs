using System;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class ReturnNode : StatementNode {
        private readonly ExpressionNode value;

        public ReturnNode() {

        }

        public ReturnNode(ExpressionNode value) {
            this.value = value;
        }

        public override void Eval(ExecutionEnvironment env) {
            if (env.Frame.Scope.IsWithin(ScopeType.PROCEDURE)) {
                throw new ReturnException(value == null ? TurtNil.NIL : value.Eval(env));
            } else {
                throw new Exception("Attempt to return but not in a procedure");
            }
        }
    }

    internal class ReturnException : Exception {
        public readonly TurtValue Value;

        public ReturnException(TurtValue value) {
            this.Value = value;
        }
    }
}