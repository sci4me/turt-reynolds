using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class StringNode : ExpressionNode {
        private string value;

        public StringNode(string value) {
            this.value = value;
        }

        public string String => value;

        public override TurtValue Eval(ExecutionEnvironment env) => new TurtString(value);
    }
}