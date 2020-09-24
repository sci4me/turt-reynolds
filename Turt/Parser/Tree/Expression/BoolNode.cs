using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class BoolNode : ExpressionNode {
        public static readonly BoolNode TRUE = new BoolNode(true);
        public static readonly BoolNode FALSE = new BoolNode(false);

        private readonly bool value;

        private BoolNode(bool value) {
            this.value = value;
        }

        public bool Value => value;

        public override TurtValue Eval(ExecutionEnvironment env) => value.Turt();
    }
}