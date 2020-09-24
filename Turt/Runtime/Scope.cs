using System.Collections.Generic;

namespace Turt.Runtime {
    public sealed class Scope {
        private readonly Scope parent;
        private readonly Dictionary<string, TurtValue> bindings;
        private ScopeType type;

        public Scope() : this(null) { }

        private Scope(Scope parent) {
            this.parent = parent;
            bindings = new Dictionary<string, TurtValue>();
            type = ScopeType.NORMAL;
        }

        public ScopeType Type {
            get => type;
            set => type = value;
        }

        public bool IsWithin(ScopeType type) {
            var scope = this;

            while (scope != null) {
                if (scope.Type == type) {
                    return true;
                } else {
                    scope = scope.Parent;
                }
            }

            return false;
        }

        public Scope Root => parent == null ? this : parent.Parent;

        public Scope Parent => parent;

        public Scope FindDeclaringScope(string ident) {
            var scope = this;

            while (scope != null && !scope.Contains(ident)) {
                scope = scope.parent;
            }

            return scope;
        }

        public bool Contains(string ident) {
            return bindings.ContainsKey(ident);
        }

        public void Set(string ident, TurtValue value) {
            bindings[ident] = value;
        }

        public TurtValue Get(string ident) {
            if (bindings.ContainsKey(ident)) {
                return bindings[ident];
            }
            return TurtNil.NIL;
        }

        public TurtValue this[string ident] {
            get {
                if (bindings.ContainsKey(ident)) {
                    return bindings[ident];
                }

                if (parent == null) {
                    return TurtNil.NIL;
                } else {
                    return parent[ident];
                }
            }
            set {
                var declaringScope = FindDeclaringScope(ident);
                if (declaringScope == null || declaringScope == this) {
                    bindings[ident] = value;
                } else {
                    declaringScope[ident] = value;
                }
            }
        }

        public Scope CreateChild() {
            return new Scope(this);
        }
    }

    public enum ScopeType {
        NORMAL,
        LOOP,
        PROCEDURE
    }
}