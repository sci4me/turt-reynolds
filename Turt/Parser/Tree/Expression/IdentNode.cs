using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class IdentNode : ExpressionNode {
        private string ident;

        public IdentNode(string ident) {
            this.ident = ident;
        }

        public string Ident => ident;

        public override TurtValue Eval(ExecutionEnvironment env) => env.Frame.Scope[ident];

        public override string ToString() {
            return "IdentNode(" + ident + ")";
        }
    }
}