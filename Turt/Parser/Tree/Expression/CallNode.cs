using System;
using System.Collections.Generic;
using Turt.Parser.Tree.Statement;
using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class CallNode : ExpressionNode {
        private ExpressionNode procedure;
        private List<ExpressionNode> arguments;

        public CallNode(ExpressionNode procedure) {
            this.procedure = procedure;
            arguments = new List<ExpressionNode>();
        }

        public void AddArgument(ExpressionNode argument) {
            arguments.Add(argument);
        }

        public override TurtValue Eval(ExecutionEnvironment env) {
            var procValue = procedure.Eval(env);

            if(!(procValue is TurtProcedure)) {
                throw new Exception("Attempt to call " + procValue + " (" + procedure + ")");
            }

            var proc = procValue.As<TurtProcedure>().Procedure;

            var arguments = new TurtValue[Math.Min(this.arguments.Count, proc.Parameters.Count)];
            for(int i = 0; i < arguments.Length; i++) {
                arguments[i] = this.arguments[i].Eval(env);
            }

            env.PushFrame();
            env.Frame.Scope.Type = ScopeType.PROCEDURE;

            for(int i = 0; i < arguments.Length; i++) {
                env.Frame.Scope.Set(proc.Parameters[i].Ident, arguments[i]);
            }

            foreach(var statement in proc.Code) {
                try {
                    statement.Eval(env);
                } catch(ReturnException e) {
                    env.PopFrame();
                    return e.Value;
                }
            }

            env.PopFrame();

            return TurtNil.NIL;
        }
    }
}