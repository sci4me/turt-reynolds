namespace Turt.Runtime {
    public sealed class TurtBool : TurtValue<TurtBool> {
        public static readonly TurtBool TRUE = new TurtBool(true);
        public static readonly TurtBool FALSE = new TurtBool(false);

        private readonly bool value;

        private TurtBool(bool value) {
            this.value = value;
        }

        public override string Type => "bool";

        public override string AsString => value.ToString().ToLower();

        public override int HashCode => value.GetHashCode();

        public override int Compare(TurtBool other) => value.CompareTo(other.value);

        #region Operator Overloads

        public static bool operator true(TurtBool value) => value.value;

        public static bool operator false(TurtBool value) => !value.value;

        public static TurtBool operator !(TurtBool value) => (!value.value).Turt();

        public static TurtBool operator &(TurtBool a, TurtBool b) => (a.value && b.value).Turt();

        public static TurtBool operator |(TurtBool a, TurtBool b) => (a.value || b.value).Turt();

        public static explicit operator bool(TurtBool value) => value.value;

        #endregion
    }

    public static class TurtBoolExtensions {
        public static TurtBool Turt(this bool value) {
            return value ? TurtBool.TRUE : TurtBool.FALSE;
        }
    }
}