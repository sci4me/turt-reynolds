using System;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class ContinueNode : StatementNode {
        public override void Eval(ExecutionEnvironment env) {
            if(env.Frame.Scope.IsWithin(ScopeType.LOOP)) {
                throw new ContinueException();
            } else {
                throw new Exception("Attempt to continue but not in a loop");
            }
        }
    }

    public sealed class ContinueException : Exception {
        
    }
}