using Turt.Parser.Tree.Expression;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class RepeatNode : StatementNode {
        private IdentNode iterator;
        private ExpressionNode start;
        private ExpressionNode end;
        private StatementNode statement;

        public RepeatNode(IdentNode iterator, ExpressionNode start, ExpressionNode end, StatementNode statement) {
            this.iterator = iterator;
            this.start = start;
            this.end = end;
            this.statement = statement;
        }

        public override void Eval(ExecutionEnvironment env) {
            env.Frame.PushScope();
            env.Frame.Scope.Type = ScopeType.LOOP;

            var start = this.start.Eval(env).As<TurtInteger>().Value;
            var end = this.end.Eval(env).As<TurtInteger>().Value;
            
            var current = start;
            var running = true;
            while(running) {
                if (current == end) running = false;

                env.Frame.Scope[iterator.Ident] = current.Turt();

                if (start < end) current++;
                else current--;

                try {
                    statement.Eval(env);
                } catch (BreakException) {
                    break;
                } catch (ContinueException) {
                }
            }

            env.Frame.PopScope();
        }
    }
}