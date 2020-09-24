namespace Turt.Lexer {
    public enum TokenType {
        LBRACE,
        RBRACE,
        LPAREN,
        RPAREN,

        COMMA,
        COLON,
        SEMICOLON,
        CONCAT,
        ASSIGN,

        ADD,
        SUB,
        MUL,
        DIV,
        MOD,
        POW,

        EQ,
        NE,
        LT,
        GT,
        LTE,
        GTE,

        AND,
        OR,

        NOT,

        UP,
        DOWN,
        LEFT,
        RIGHT,
        GO,
        COLOR,
        WIDTH,
        REPEAT,
        CONTINUE,
        BREAK,
        RETURN,
        DOT,
        PRINT,

        FROM,
        TO,

        IF,
        ELSE,

        DEF,
        VAR,

        TRUE,
        FALSE,
        NIL,

        INTEGER,
        STRING,
        IDENT
    }
}