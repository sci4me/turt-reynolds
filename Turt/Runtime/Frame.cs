using System;

namespace Turt.Runtime {
    public sealed class Frame {
        private readonly Frame parent;
        private Scope scope;

        public Frame(Scope parentScope) : this(null, parentScope) {
            
        }
    
        private Frame(Frame parent, Scope parentScope) {
            this.parent = parent;
            scope = parentScope.CreateChild();
        }

        public Frame Parent => parent;

        public void PushScope() {
            scope = scope.CreateChild();
        }

        public void PopScope() {
            scope = scope.Parent;
            if (scope == null) throw new InvalidOperationException();
        }

        public Scope Scope => scope;

        public Frame CreateChild() {
            return new Frame(this, scope);
        }
    }
}