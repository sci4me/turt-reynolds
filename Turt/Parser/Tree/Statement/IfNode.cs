using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class IfNode : StatementNode {
        private readonly ExpressionNode condition;
        private readonly StatementNode success;
        private readonly StatementNode failure;

        public IfNode(ExpressionNode condition, StatementNode success, StatementNode failure) {
            this.condition = condition;
            this.success = success;
            this.failure = failure;
        }

        public override void Eval(ExecutionEnvironment env) {
            if(condition.Eval(env).As<TurtBool>()) {
                success.Eval(env);
            } else if(failure != null) {
                failure.Eval(env);
            }
        }
    }
}