using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class GoNode : StatementNode {
        private readonly ExpressionNode distance;

        public GoNode(ExpressionNode distance) {
            this.distance = distance;
        }

        public override void Eval(ExecutionEnvironment env) {
            env.Go(distance.Eval(env).As<TurtInteger>());
        }

        public ExpressionNode Distance => distance;
    }
}
