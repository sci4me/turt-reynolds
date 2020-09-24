using System.Collections.Generic;
using Turt.Parser.Tree.Expression;
using Turt.Runtime;

namespace Turt.Parser.Tree.Definition {
    public sealed class DefNode : StatementNode {
        private readonly IdentNode name;
        private readonly List<IdentNode> parameters;
        private readonly List<StatementNode> code;

        public DefNode(IdentNode name) {
            this.name = name;
            parameters = new List<IdentNode>();
            code = new List<StatementNode>();
        }

        public IdentNode Name => name;

        public List<IdentNode> Parameters => parameters;

        public List<StatementNode> Code => code;

        public override void Eval(ExecutionEnvironment env) {
            env.Frame.Scope[name.Ident] = new TurtProcedure(this); 
        }
    }
}