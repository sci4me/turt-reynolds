using Turt.Parser.Tree.Expression;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class ColorNode : StatementNode {
        private readonly ExpressionNode red;
        private readonly ExpressionNode green;
        private readonly ExpressionNode blue;

        public ColorNode(ExpressionNode red, ExpressionNode green, ExpressionNode blue) {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public override void Eval(ExecutionEnvironment env) {
            env.SetColor(red.Eval(env).As<TurtInteger>(), green.Eval(env).As<TurtInteger>(), blue.Eval(env).As<TurtInteger>());
        }

        public ExpressionNode Red => red;

        public ExpressionNode Green => green;

        public ExpressionNode Blue => blue;
    }
}