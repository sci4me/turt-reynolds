using Turt.Parser.Tree.Expression;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class RightNode : StatementNode {
        private readonly ExpressionNode angle;

        public RightNode(ExpressionNode angle) {
            this.angle = angle;
        }

        public override void Eval(ExecutionEnvironment env) {
            env.TurnRight(angle.Eval(env).As<TurtInteger>());
        }

        public ExpressionNode Angle => angle;
    }
}