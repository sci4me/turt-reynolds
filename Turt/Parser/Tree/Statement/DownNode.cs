using System;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class DownNode : StatementNode {
        public override void Eval(ExecutionEnvironment env) {
            env.PenDown();
        }
    }
}