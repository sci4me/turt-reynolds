using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class UpNode : StatementNode {
        public override void Eval(ExecutionEnvironment env) {
            env.PenUp();
        }
    }
}