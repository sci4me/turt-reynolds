using System;
using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class UnaryNode : ExpressionNode {
        private readonly UnaryOP op;
        private readonly ExpressionNode value;

        public UnaryNode(UnaryOP op, ExpressionNode value) {
            this.op = op;
            this.value = value;
        }

        public override TurtValue Eval(ExecutionEnvironment env) {
            switch (op) {
                case UnaryOP.NEG:
                    return -(value.Eval(env).As<TurtInteger>());
                case UnaryOP.NOT:
                    return !(value.Eval(env).As<TurtBool>());
            }

            throw new Exception();
        }
    }

    public enum UnaryOP {
        NONE,

        NEG,
        NOT
    }
}