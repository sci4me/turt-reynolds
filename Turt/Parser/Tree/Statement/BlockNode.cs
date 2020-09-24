using System.Collections.Generic;
using Turt.Runtime;

namespace Turt.Parser.Tree {
    public sealed class BlockNode : StatementNode {
        private List<StatementNode> statements;

        public BlockNode() {
            statements = new List<StatementNode>();
        }

        public override void Eval(ExecutionEnvironment env) {
            env.Frame.PushScope();
            statements.ForEach(s => s.Eval(env));
            env.Frame.PopScope();
        }

        public void AddStatement(StatementNode statement) {
            statements.Add(statement);
        }

        public IReadOnlyList<StatementNode> Statements => statements.AsReadOnly();
    }
}