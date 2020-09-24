using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class DotNode : StatementNode {
        private readonly ExpressionNode size;

        public DotNode(ExpressionNode size) {
            this.size = size;
        }

        public override void Eval(ExecutionEnvironment env) {
            env.Dot(size.Eval(env).As<TurtInteger>());
        }
    }
}