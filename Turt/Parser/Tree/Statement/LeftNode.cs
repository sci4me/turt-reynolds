using Turt.Parser.Tree.Expression;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class LeftNode : StatementNode {
        private readonly ExpressionNode angle;

        public LeftNode(ExpressionNode angle) {
            this.angle = angle;
        }

        public override void Eval(ExecutionEnvironment env) {
            env.TurnLeft(angle.Eval(env).As<TurtInteger>());
        }

        public ExpressionNode Angle => angle;
    }
}