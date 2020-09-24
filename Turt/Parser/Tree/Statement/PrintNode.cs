using System;
using Turt.Runtime;

namespace Turt.Parser.Tree.Statement {
    public sealed class PrintNode : StatementNode {
        private ExpressionNode value;

        public PrintNode(ExpressionNode value) {
            this.value = value;
        }

        public override void Eval(ExecutionEnvironment env) {
            Console.WriteLine(value.Eval(env).AsTurtString);
            Console.Out.Flush();
        }
    }
}