using System;
using System.Runtime.Serialization;

namespace Turt.Lexer {
    [Serializable]
    public sealed class LexException : Exception {
        public LexException(string message) : base(message) {
        }
    }
}