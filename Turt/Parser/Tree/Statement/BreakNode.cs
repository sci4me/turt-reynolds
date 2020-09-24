using System;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class BreakNode : StatementNode {
        public override void Eval(ExecutionEnvironment env) {
            if (env.Frame.Scope.IsWithin(ScopeType.LOOP)) {
                throw new BreakException();
            } else {
                throw new Exception("Attempt to break but not in a loop");
            }
        }
    }

    public sealed class BreakException : Exception {

    }
}