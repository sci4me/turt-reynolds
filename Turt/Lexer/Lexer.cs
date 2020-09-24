using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turt.Lexer {
    public sealed class Lexer {
        private string source;
        private int start;
        private int pos;
        private List<Token> tokens;
        private uint line;
        private uint column;

        public Lexer(string source) {
            this.source = source;
            tokens = new List<Token>();
        }

        private string current() {
            return source.Substring(start, pos - start);
        }

        private void emit(TokenType type) {
            tokens.Add(new Token(type, current(), line + 1, (uint)(column - (pos - start))));
            start = pos;
        }

        private char next() {
            if (!more()) {
                return (char)0;
            }

            var c = source[pos];
            pos++;
            column++;
            return c;
        }

        private void prev() {
            pos--;
            column--;
        }

        private char peek() {
            if (!more()) {
                return (char)0;
            }

            return source[pos];
        }

        private void ignore() {
            start = pos;
        }

        private bool accept(string valid) {
            if (!more()) {
                return false;
            }

            if (valid.Contains(next())) {
                return true;
            }

            prev();
            return false;
        }

        private void acceptRun(string valid) {
            while (more() && valid.Contains(peek())) {
                next();
            }
        }

        private bool acceptSeq(string seq) {
            int savedPos = pos;
            uint savedColumn = column;

            for (int i = 0; i < seq.Length; i++) {
                if (seq[i] != next()) {
                    pos = savedPos;
                    column = savedColumn;
                    return false;
                }
            }

            return true;
        }

        private bool more() {
            return pos < source.Length;
        }

        public List<Token> Tokenize() {
            bool running = true;
            while (running) {
                var c = next();
                switch (c) {
                    case (char)0:
                        running = false;
                        break;
                    case '\n':
                        ignore();
                        line++;
                        column = 0;
                        break;
                    case '{':
                        emit(TokenType.LBRACE);
                        break;
                    case '}':
                        emit(TokenType.RBRACE);
                        break;
                    case '(':
                        emit(TokenType.LPAREN);
                        break;
                    case ')':
                        emit(TokenType.RPAREN);
                        break;
                    case ',':
                        emit(TokenType.COMMA);
                        break;
                    case ':':
                        emit(TokenType.COLON);
                        break;
                    case ';':
                        emit(TokenType.SEMICOLON);
                        break;
                    case '.':
                        if (accept(".")) {
                            emit(TokenType.CONCAT);
                        } else {
                            throw new LexException("Unexpected character '" + peek() + "'");
                        }
                        break;
                    case '+':
                        emit(TokenType.ADD);
                        break;
                    case '-':
                        emit(TokenType.SUB);
                        break;
                    case '*':
                        if (accept("*")) {
                            emit(TokenType.POW);
                        } else {
                            emit(TokenType.MUL);
                        }
                        break;
                    case '/':
                        if (accept("/")) {
                            while (more() && peek() != '\n') {
                                next();
                            }
                            ignore();

                            line++;
                            column = 0;
                        } else if (accept("*")) {
                            var depth = 1;

                            next();
                            while (more() && depth > 0) {
                                if (acceptSeq("/*")) {
                                    depth++;
                                } else if (acceptSeq("*/")) {
                                    depth--;
                                } else if (peek() == '\n') {
                                    line++;
                                    column = 0;
                                }

                                next();
                            }

                            if (depth != 0) {
                                throw new LexException("Block comment not ended");
                            }

                            ignore();
                        } else {
                            emit(TokenType.DIV);
                        }
                        break;
                    case '%':
                        emit(TokenType.MOD);
                        break;
                    case '=':
                        if (accept("=")) {
                            emit(TokenType.EQ);
                        } else {
                            emit(TokenType.ASSIGN);
                        }
                        break;
                    case '!':
                        if (accept("=")) {
                            emit(TokenType.NE);
                        } else {
                            emit(TokenType.NOT);
                        }
                        break;
                    case '<':
                        if (accept("=")) {
                            emit(TokenType.LTE);
                        } else {
                            emit(TokenType.LT);
                        }
                        break;
                    case '>':
                        if (accept("=")) {
                            emit(TokenType.GTE);
                        } else {
                            emit(TokenType.GT);
                        }
                        break;
                    case '"':
                        var sb = new StringBuilder();

                        while (more() && peek() != '"') {
                            if (accept("\\")) {
                                if (accept("\\")) {
                                    sb.Append("\\");
                                } else if (accept("\"")) {
                                    sb.Append("\"");
                                } else if (accept("n")) {
                                    sb.Append("\n");
                                } else {
                                    throw new LexException("Invalid escape character '" + peek() + "'");
                                }
                            } else {
                                sb.Append(peek());
                                next();
                            }
                        }

                        next();
                        ignore();

                        tokens.Add(new Token(TokenType.STRING, sb.ToString(), line + 1, (uint)(column - (pos - start) + 1)));
                        start = pos;
                        break;
                    default:
                        if (char.IsLetter(c) || c == '_') {
                            while (more() && (char.IsLetterOrDigit(peek()) || peek() == '_')) {
                                next();
                            }

                            var ident = current();
                            switch (ident) {
                                case "and":
                                    emit(TokenType.AND);
                                    break;
                                case "or":
                                    emit(TokenType.OR);
                                    break;
                                case "up":
                                    emit(TokenType.UP);
                                    break;
                                case "down":
                                    emit(TokenType.DOWN);
                                    break;
                                case "left":
                                    emit(TokenType.LEFT);
                                    break;
                                case "right":
                                    emit(TokenType.RIGHT);
                                    break;
                                case "go":
                                    emit(TokenType.GO);
                                    break;
                                case "color":
                                    emit(TokenType.COLOR);
                                    break;
                                case "width":
                                    emit(TokenType.WIDTH);
                                    break;
                                case "repeat":
                                    emit(TokenType.REPEAT);
                                    break;
                                case "continue":
                                    emit(TokenType.CONTINUE);
                                    break;
                                case "break":
                                    emit(TokenType.BREAK);
                                    break;
                                case "return":
                                    emit(TokenType.RETURN);
                                    break;
                                case "dot":
                                    emit(TokenType.DOT);
                                    break;
                                case "print":
                                    emit(TokenType.PRINT);
                                    break;
                                case "from":
                                    emit(TokenType.FROM);
                                    break;
                                case "to":
                                    emit(TokenType.TO);
                                    break;
                                case "if":
                                    emit(TokenType.IF);
                                    break;
                                case "else":
                                    emit(TokenType.ELSE);
                                    break;
                                case "def":
                                    emit(TokenType.DEF);
                                    break;
                                case "var":
                                    emit(TokenType.VAR);
                                    break;
                                case "true":
                                    emit(TokenType.TRUE);
                                    break;
                                case "false":
                                    emit(TokenType.FALSE);
                                    break;
                                case "nil":
                                    emit(TokenType.NIL);
                                    break;
                                default:
                                    emit(TokenType.IDENT);
                                    break;
                            }
                        } else if (char.IsDigit(c) || c == '-') {
                            accept("-");
                            acceptRun("0123456789");
                            emit(TokenType.INTEGER);
                        } else if (char.IsWhiteSpace(c)) {
                            while (more() && char.IsWhiteSpace(peek())) {
                                if (peek() == '\n') {
                                    line++;
                                    column = 0;
                                }

                                next();
                            }
                            ignore();
                        } else {
                            throw new LexException("Unexpected character '" + c + "'");
                        }
                        break;
                }
            }

            return tokens;
        }
    }
}