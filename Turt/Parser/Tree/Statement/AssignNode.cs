using Turt.Parser.Tree.Expression;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class AssignNode : StatementNode {
        private readonly IdentNode name;
        private readonly ExpressionNode value;

        public AssignNode(IdentNode name, ExpressionNode value) {
            this.name = name;
            this.value = value;
        }

        public override void Eval(ExecutionEnvironment env) {
            var name = this.name.Ident;
            var value = this.value.Eval(env);
            var scope = env.Frame.Scope.FindDeclaringScope(name);
            if (scope != null) {
                scope[name] = value;
            } else {
                env.Frame.Scope.Root[name] = value;
            }
        }
    }
}