namespace Turt.Runtime {
    public sealed class TurtString : TurtValue<TurtString> {
        private string value;

        public TurtString(string value) {
            this.value = value;
        }

        public string Value => value;

        public override int HashCode => value.GetHashCode();

        public override string Type => "string";

        public override string AsString => value;

        public override int Compare(TurtString other) => other.value.CompareTo(value);

        public static TurtString operator +(TurtString a, TurtString b) => new TurtString(a.Value + b.Value);
    }
}