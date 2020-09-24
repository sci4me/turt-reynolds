using Turt.Runtime;

namespace Turt.Parser.Tree {
    public abstract class StatementNode : BaseNode {
        public abstract void Eval(ExecutionEnvironment env);
    }
}