namespace Turt.Runtime {
    public sealed class TurtNil : TurtValue<TurtNil> {
        public static readonly TurtNil NIL = new TurtNil();

        private TurtNil() { }

        public override int HashCode => 0;

        public override string Type => "nil";

        public override string AsString => "nil";

        public override int Compare(TurtNil other) => 0;
    }
}