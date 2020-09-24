using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class NilNode : ExpressionNode {
        public static readonly NilNode NIL = new NilNode();

        private NilNode() { }

        public override TurtValue Eval(ExecutionEnvironment env) => TurtNil.NIL;
    }
}