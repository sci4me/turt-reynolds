using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turt.Lexer;
using Turt.Parser.Tree;
using Turt.Parser.Tree.Definition;
using Turt.Parser.Tree.Expression;
using Turt.Parser.Tree.Statement;

namespace Turt.Parser {
    public sealed class Parser {
        private List<Token> tokens;
        private int index;

        public Parser(List<Token> tokens) {
            this.tokens = tokens;
        }

        private void next() {
            index++;
        }

        private void prev() {
            index--;
        }

        private Token current() {
            if (!more()) {
                throw new ParseException("No more tokens");
            }

            return tokens[index];
        }

        private bool more() {
            return index < tokens.Count;
        }

        private bool accept(params TokenType[] types) {
            if (!more()) return false;
            return types.Contains(current().Type);
        }

        private void expect(params TokenType[] types) {
            if (!accept(types)) {
                var sb = new StringBuilder();

                foreach (var type in types) {
                    sb.Append(type);
                    sb.Append(" ");
                }

                throw new ParseException("Unexpected token: " + current() + " Possible types: " + sb.ToString());
            }
        }

        private void expectNot(params TokenType[] types) {
            if (accept(types)) {
                throw new ParseException("Unexpected token: " + current());
            }
        }

        private BinaryOP parseBinaryOP() {
            switch (current().Type) {
                case TokenType.ADD:
                    return BinaryOP.ADD;
                case TokenType.SUB:
                    return BinaryOP.SUB;
                case TokenType.MUL:
                    return BinaryOP.MUL;
                case TokenType.DIV:
                    return BinaryOP.DIV;
                case TokenType.MOD:
                    return BinaryOP.MOD;
                case TokenType.POW:
                    return BinaryOP.POW;
                case TokenType.EQ:
                    return BinaryOP.EQ;
                case TokenType.NE:
                    return BinaryOP.NE;
                case TokenType.LT:
                    return BinaryOP.LT;
                case TokenType.GT:
                    return BinaryOP.GT;
                case TokenType.LTE:
                    return BinaryOP.LTE;
                case TokenType.GTE:
                    return BinaryOP.GTE;
                case TokenType.AND:
                    return BinaryOP.AND;
                case TokenType.OR:
                    return BinaryOP.OR;
                case TokenType.CONCAT:
                    return BinaryOP.CONCAT;
            }

            return BinaryOP.NONE;
        }

        private UnaryOP parseUnaryOP() {
            switch (current().Type) {
                case TokenType.SUB:
                    return UnaryOP.NEG;
                case TokenType.NOT:
                    return UnaryOP.NOT;
            }

            return UnaryOP.NONE;
        }

        private IntegerNode parseInteger() {
            expect(TokenType.INTEGER);

            if (int.TryParse(current().Data, out var value)) {
                next();
                return new IntegerNode(value);
            }

            throw new ParseException("Failed to parse integer" + current().Data + "'");
        }

        private BoolNode parseBool() {
            expect(TokenType.TRUE, TokenType.FALSE);
            var value = current().Type == TokenType.TRUE ? BoolNode.TRUE : BoolNode.FALSE;
            next();
            return value;
        }

        private IdentNode parseIdent() {
            expect(TokenType.IDENT);
            var ident = new IdentNode(current().Data);
            next();
            return ident;
        }

        private StringNode parseString() {
            expect(TokenType.STRING);
            var str = new StringNode(current().Data);
            next();
            return str;
        }

        private ExpressionNode parseBinary(Func<ExpressionNode> p, params TokenType[] types) {
            var ret = p();

            while (more() && accept(types)) {
                var op = parseBinaryOP();
                next();
                ret = new BinaryNode(op, ret, p());
            }

            return ret;
        }

        private CallNode parseCall() {
            var call = new CallNode(parseIdent());

            expect(TokenType.LPAREN);
            next();

            while (more() && current().Type != TokenType.RPAREN) {
                call.AddArgument(parseExpression());

                if (accept(TokenType.COMMA)) {
                    next();
                    expectNot(TokenType.RPAREN);
                }
            }

            expect(TokenType.RPAREN);
            next();

            return call;
        }

        private ExpressionNode parseAtom() {
            expect(TokenType.TRUE, TokenType.FALSE, TokenType.NIL, TokenType.INTEGER, TokenType.IDENT, TokenType.STRING, TokenType.LPAREN);
            if (accept(TokenType.TRUE, TokenType.FALSE)) {
                return parseBool();
            } else if (accept(TokenType.NIL)) {
                next();
                return NilNode.NIL;
            } else if (accept(TokenType.INTEGER)) {
                return parseInteger();
            } else if (accept(TokenType.IDENT)) {
                next();
                var isCall = accept(TokenType.LPAREN);
                prev();

                if (isCall) {
                    return parseCall();
                } else {
                    return parseIdent();
                }
            } else if (accept(TokenType.LPAREN)) {
                next();
                var p = new ParenNode(parseExpression());
                expect(TokenType.RPAREN);
                next();
                return p;
            } else if (accept(TokenType.STRING)) {
                return parseString();
            }

            throw new Exception("Unreachable");
        }

        private ExpressionNode parsePower() {
            return parseBinary(parseAtom, TokenType.POW);
        }

        private ExpressionNode parseUnary() {
            var op = parseUnaryOP();
            if (op != UnaryOP.NONE) {
                var ops = new List<UnaryOP>();

                while (op != UnaryOP.NONE) {
                    next();
                    ops.Add(op);
                    op = parseUnaryOP();
                }

                var ret = parsePower();

                for (int i = ops.Count - 1; i >= 0; i--) {
                    ret = new UnaryNode(ops[i], ret);
                }

                return ret;
            } else {
                return parsePower();
            }
        }

        private ExpressionNode parseBinaryMul() {
            return parseBinary(parseUnary, TokenType.MUL, TokenType.DIV, TokenType.MOD);
        }

        private ExpressionNode parseBinaryAdd() {
            return parseBinary(parseBinaryMul, TokenType.ADD, TokenType.SUB);
        }

        private ExpressionNode parseBinaryComparison() {
            return parseBinary(parseBinaryAdd, TokenType.LT, TokenType.GT, TokenType.LTE, TokenType.GTE);
        }

        private ExpressionNode parseBinaryEquality() {
            return parseBinary(parseBinaryComparison, TokenType.EQ, TokenType.NE);
        }

        private ExpressionNode parseBinaryLogicalAnd() {
            return parseBinary(parseBinaryEquality, TokenType.AND);
        }

        private ExpressionNode parseBinaryLogicalOr() {
            return parseBinary(parseBinaryLogicalAnd, TokenType.OR);
        }

        private ExpressionNode parseConcat() {
            return parseBinary(parseBinaryLogicalOr, TokenType.CONCAT);
        }

        private ExpressionNode parseExpression() {
            return parseConcat();
        }

        private BlockNode parseBlock() {
            expect(TokenType.LBRACE);
            next();

            var block = new BlockNode();

            while (more() && current().Type != TokenType.RBRACE) {
                block.AddStatement(parseStatement());
            }

            expect(TokenType.RBRACE);
            next();

            return block;
        }

        private RepeatNode parseRepeat() {
            expect(TokenType.REPEAT);
            next();

            var iterator = parseIdent();

            expect(TokenType.FROM);
            next();

            var start = parseExpression();

            expect(TokenType.TO);
            next();

            var end = parseExpression();
            var statement = parseStatement();

            return new RepeatNode(iterator, start, end, statement);
        }

        private IfNode parseIf() {
            expect(TokenType.IF);
            next();

            var condition = parseExpression();
            var success = parseStatement();
            StatementNode failure = null;

            if (accept(TokenType.ELSE)) {
                next();
                failure = parseStatement();
            }

            return new IfNode(condition, success, failure);
        }

        private DefNode parseDef() {
            expect(TokenType.DEF);
            next();

            var def = new DefNode(parseIdent());

            expect(TokenType.LPAREN);
            next();

            while (more() && current().Type != TokenType.RPAREN) {
                def.Parameters.Add(parseIdent());

                if (accept(TokenType.COMMA)) {
                    next();
                    expectNot(TokenType.RPAREN);
                }
            }

            expect(TokenType.RPAREN);
            next();

            expect(TokenType.LBRACE);
            next();

            while (more() && current().Type != TokenType.RBRACE) {
                def.Code.Add(parseStatement());
            }

            expect(TokenType.RBRACE);
            next();

            return def;
        }

        private VarNode parseVar() {
            expect(TokenType.VAR);
            next();

            var name = parseIdent();
            ExpressionNode defaultValue = null;

            if(accept(TokenType.ASSIGN)) {
                next();
                defaultValue = parseExpression();
            }

            return new VarNode(name, defaultValue);
        }

        private StatementNode parseStatement() {
            expect(TokenType.UP, TokenType.DOWN, TokenType.LEFT, TokenType.RIGHT, TokenType.GO, TokenType.COLOR, TokenType.WIDTH, TokenType.REPEAT,
                TokenType.CONTINUE, TokenType.BREAK, TokenType.RETURN, TokenType.RETURN, TokenType.DOT, TokenType.PRINT, TokenType.LBRACE, TokenType.IF, 
                TokenType.DEF, TokenType.VAR, TokenType.IDENT);
            if (accept(TokenType.UP)) {
                next();
                return new UpNode();
            } else if (accept(TokenType.DOWN)) {
                next();
                return new DownNode();
            } else if (accept(TokenType.LEFT)) {
                next();
                return new LeftNode(parseExpression());
            } else if (accept(TokenType.RIGHT)) {
                next();
                return new RightNode(parseExpression());
            } else if (accept(TokenType.GO)) {
                next();
                return new GoNode(parseExpression());
            } else if (accept(TokenType.COLOR)) {
                next();
                return new ColorNode(parseExpression(), parseExpression(), parseExpression());
            } else if (accept(TokenType.WIDTH)) {
                next();
                return new WidthNode(parseExpression());
            } else if (accept(TokenType.REPEAT)) {
                return parseRepeat();
            } else if(accept(TokenType.CONTINUE)) {
                next();
                return new ContinueNode();
            } else if (accept(TokenType.BREAK)) {
                next();
                return new BreakNode();
            } else if (accept(TokenType.RETURN)) {
                next();

                if(accept(TokenType.SEMICOLON)) {
                    return new ReturnNode();
                } else {
                    return new ReturnNode(parseExpression());
                }
            } else if (accept(TokenType.DOT)) {
                next();
                return new DotNode(parseExpression());
            } else if (accept(TokenType.PRINT)) {
                next();
                return new PrintNode(parseExpression());
            } else if (accept(TokenType.LBRACE)) {
                return parseBlock();
            } else if (accept(TokenType.IF)) {
                return parseIf();
            } else if (accept(TokenType.DEF)) {
                return parseDef();
            } else if (accept(TokenType.VAR)) {
                return parseVar();
            } else if (accept(TokenType.IDENT)) {
                next();

                if(accept(TokenType.LPAREN)) {
                    prev();
                    return new ExpressionStatement(parseCall());
                } else if(accept(TokenType.ASSIGN)) {
                    prev();
                    var name = parseIdent();
                    expect(TokenType.ASSIGN);
                    next();
                    return new AssignNode(name, parseExpression());
                } else {
                    prev();
                    throw new ParseException(current().ToString());
                }
            }

            throw new Exception("Unreachable");
        }

        public BlockNode Parse() {
            var block = new BlockNode();

            while (more()) {
                block.AddStatement(parseStatement());
            }

            return block;
        }
    }
}