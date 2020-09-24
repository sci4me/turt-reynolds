using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class IntegerNode : ExpressionNode {
        private readonly int value;

        public IntegerNode(int value) {
            this.value = value;
        }

        public int Value => value;

        public override TurtValue Eval(ExecutionEnvironment env) => value.Turt();
    }
}