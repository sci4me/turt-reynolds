using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class WidthNode : StatementNode {
        private readonly ExpressionNode width;

        public WidthNode(ExpressionNode width) {
            this.width = width;
        }

        public override void Eval(ExecutionEnvironment env) {
            env.SetWidth(width.Eval(env).As<TurtInteger>());
        }

        public ExpressionNode Width => width;
    }
}