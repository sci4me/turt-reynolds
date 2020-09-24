namespace Turt.Lexer {
    public sealed class Token {
        private TokenType type;
        private string data;
        private uint line;
        private uint column;

        public Token(TokenType type, string data, uint line, uint column) {
            this.type = type;
            this.data = data;
            this.line = line;
            this.column = column;
        }

        public TokenType Type {
            get { return this.type; }
        }

        public string Data {
            get { return this.data; }
        }

        public uint Line {
            get { return this.line; }
        }

        public uint Column {
            get { return this.column; }
        }

        public override string ToString() {
            return data + " (" + type + ") at line " + line + ", column " + column;
        }
    }
}