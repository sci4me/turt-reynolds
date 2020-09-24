using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class ParenNode : ExpressionNode {
        private readonly ExpressionNode value;

        public ParenNode(ExpressionNode value) {
            this.value = value;
        }

        public override TurtValue Eval(ExecutionEnvironment env) {
            return value.Eval(env);
        }
    }
}