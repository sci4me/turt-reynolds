﻿using System;

namespace Turt.Parser {
    [Serializable]
    public sealed class ParseException : Exception {
        public ParseException(string message) : base(message) {
        }
    }
}