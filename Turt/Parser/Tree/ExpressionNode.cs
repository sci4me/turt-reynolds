using Turt.Runtime;

namespace Turt.Parser.Tree {
    public abstract class ExpressionNode : BaseNode {
        public abstract TurtValue Eval(ExecutionEnvironment env);
    }
}