using Turt.Parser.Tree.Expression;
using Turt.Runtime;

namespace Turt.Parser.Tree.Definition {
    public sealed class VarNode : StatementNode {
        private readonly IdentNode name;
        private readonly ExpressionNode defaultValue;

        public VarNode(IdentNode name, ExpressionNode defaultValue) {
            this.name = name;
            this.defaultValue = defaultValue;
        }

        public override void Eval(ExecutionEnvironment env) {
            if(defaultValue == null) {
                env.Frame.Scope[name.Ident] = TurtNil.NIL;
            } else {
                env.Frame.Scope[name.Ident] = defaultValue.Eval(env);
            }
        }
    }
}