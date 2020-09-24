using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class ExpressionStatement : StatementNode {
        private readonly ExpressionNode expr;

        public ExpressionStatement(ExpressionNode expr) {
            this.expr = expr;
        }

        public override void Eval(ExecutionEnvironment env) {
            expr.Eval(env);
        }
    }
}