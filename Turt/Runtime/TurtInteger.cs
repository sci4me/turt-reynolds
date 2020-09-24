using System;

namespace Turt.Runtime {
    public sealed class TurtInteger : TurtValue<TurtInteger> {
        private readonly int value;

        public TurtInteger(int value) {
            this.value = value;
        }

        public int Value => value;

        public override string Type => "integer";

        public override int HashCode => value.GetHashCode();

        public override string AsString => value.ToString();

        public override int Compare(TurtInteger other) => value.CompareTo(other.value);

        public static TurtInteger operator +(TurtInteger a, TurtInteger b) => (a.value + b.value).Turt();

        public static TurtInteger operator -(TurtInteger a, TurtInteger b) => (a.value - b.value).Turt();

        public static TurtInteger operator *(TurtInteger a, TurtInteger b) => (a.value * b.value).Turt();

        public static TurtInteger operator /(TurtInteger a, TurtInteger b) => (a.value / b.value).Turt();

        public static TurtInteger operator %(TurtInteger a, TurtInteger b) => (a.value % b.value).Turt();

        public static TurtInteger operator -(TurtInteger a) => (-a.value).Turt();
    }

    public static class TurtIntegerExtensions {
        private const int cache_size = 256;
        private const int offset = cache_size / 2;
        private const int min = -offset;
        private const int max = offset - 1;

        private static TurtInteger[] cache = new TurtInteger[cache_size];

        static TurtIntegerExtensions() {
            for(int i = 0; i < cache_size; i++) {
                cache[i] = new TurtInteger(i - offset);
            }
        }

        public static TurtInteger Turt(this Int32 value) {
            if(value >= min && value <= max) {
                return cache[value + offset];
            }
            return new TurtInteger(value);
        }
    }
}