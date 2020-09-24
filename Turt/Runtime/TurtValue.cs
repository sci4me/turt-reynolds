using System;

namespace Turt.Runtime {
    public abstract class TurtValue {
        #region TurtValue Methods

        public T As<T>() where T : TurtValue<T> {
            var t = typeof(T);
            if (GetType().Equals(t)) {
                return (T)this;
            }
            throw new ArgumentException("attemped to convert " + Type + " to " + t.Name);
        }

        public abstract string Type { get; }

        public abstract string AsString { get; }

        public virtual TurtString AsTurtString => new TurtString(AsString);

        #endregion
    }

    public abstract class TurtValue<T> : TurtValue, IComparable<TurtValue<T>>, IEquatable<TurtValue<T>> where T : TurtValue<T> {
        #region TurtValue Methods

        public abstract int HashCode { get; }

        public abstract int Compare(T other);

        #endregion

        #region Operator Overloads

        public static TurtBool operator ==(TurtValue<T> a, TurtValue<T> b) => (a.CompareTo(b) == 0).Turt();

        public static TurtBool operator !=(TurtValue<T> a, TurtValue<T> b) => (a.CompareTo(b) != 0).Turt();

        public static TurtBool operator <(TurtValue<T> a, TurtValue<T> b) => (a.CompareTo(b) < 0).Turt();

        public static TurtBool operator >(TurtValue<T> a, TurtValue<T> b) => (a.CompareTo(b) > 0).Turt();

        public static TurtBool operator <=(TurtValue<T> a, TurtValue<T> b) => (a.CompareTo(b) <= 0).Turt();

        public static TurtBool operator >=(TurtValue<T> a, TurtValue<T> b) => (a.CompareTo(b) >= 0).Turt();

        #endregion

        #region Object Overrides

        public override bool Equals(object obj) {
            if (!(obj is TurtValue<T>)) return false;
            return Compare((dynamic)obj);
        }

        public override int GetHashCode() => HashCode;

        public override string ToString() => AsString;

        #endregion

        #region IComparable

        public int CompareTo(TurtValue<T> other) {
            if (!GetType().Equals(other.GetType())) throw new NotSupportedException("attempt to compare " + Type + " with " + other.Type);
            return Compare((dynamic)other);
        }

        #endregion

        #region IEquatable

        public bool Equals(TurtValue<T> other) {
            return object.Equals(this, other);
        }

        #endregion
    }
}