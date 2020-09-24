using System;
using Turt.Runtime;

namespace Turt.Parser.Tree.Expression {
    public sealed class BinaryNode : ExpressionNode {
        private readonly BinaryOP op;
        private readonly ExpressionNode left;
        private readonly ExpressionNode right;

        public BinaryNode(BinaryOP op, ExpressionNode left, ExpressionNode right) {
            this.op = op;
            this.left = left;
            this.right = right;
        }

        public BinaryOP OP => op;

        public ExpressionNode Left => left;

        public ExpressionNode Right => right;

        public override TurtValue Eval(ExecutionEnvironment env) {
            var left = this.left.Eval(env);
            var right = this.right.Eval(env);

            switch(op) {
                case BinaryOP.ADD:
                    return left.As<TurtInteger>() + right.As<TurtInteger>();
                case BinaryOP.SUB:
                    return left.As<TurtInteger>() - right.As<TurtInteger>();
                case BinaryOP.MUL:
                    return left.As<TurtInteger>() * right.As<TurtInteger>();
                case BinaryOP.DIV:
                    return left.As<TurtInteger>() / right.As<TurtInteger>();
                case BinaryOP.POW:
                    return ((int)Math.Pow(left.As<TurtInteger>().Value, right.As<TurtInteger>().Value)).Turt();
                case BinaryOP.MOD:
                    return left.As<TurtInteger>() % right.As<TurtInteger>();
                case BinaryOP.EQ:
                    return (left == right).Turt();
                case BinaryOP.NE:
                    return (left != right).Turt();
                case BinaryOP.LT:
                    return left.As<TurtInteger>() < right.As<TurtInteger>();
                case BinaryOP.GT:
                    return left.As<TurtInteger>() > right.As<TurtInteger>();
                case BinaryOP.LTE:
                    return left.As<TurtInteger>() <= right.As<TurtInteger>();
                case BinaryOP.GTE:
                    return left.As<TurtInteger>() >= right.As<TurtInteger>();
                case BinaryOP.AND:
                    return left.As<TurtBool>() && right.As<TurtBool>();
                case BinaryOP.OR:
                    return left.As<TurtBool>() || right.As<TurtBool>();
                case BinaryOP.CONCAT:
                    return left.AsTurtString + right.AsTurtString;
            }

            throw new Exception();
        }
    }

    public enum BinaryOP {
        NONE,

        ADD,
        SUB,
        MUL,
        DIV,
        MOD,
        POW,

        EQ,
        NE,
        LT,
        GT,
        LTE,
        GTE,

        AND,
        OR,

        CONCAT
    }
}