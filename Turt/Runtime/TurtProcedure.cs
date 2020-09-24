using Turt.Parser.Tree.Definition;

namespace Turt.Runtime {
    public sealed class TurtProcedure : TurtValue<TurtProcedure> {
        private DefNode procedure;

        public TurtProcedure(DefNode procedure) {
            this.procedure = procedure;
        }

        public DefNode Procedure => procedure;

        public override int HashCode {
            get {
                int hash = 31;
                hash = hash * 29 + procedure.Name.GetHashCode();
                foreach(var param in procedure.Parameters) {
                    hash = hash * 29 + param.Ident.GetHashCode();
                }
                return 0;
            }
        }

        public override string Type => "procedure";

        public override string AsString => "procedure";

        public override int Compare(TurtProcedure other) => procedure.Name.Ident.CompareTo(other.Procedure.Name.Ident);
    }
}