using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner
{
    public static class TokenTypeTabs
    {
        public static readonly string[] RoundBrackets = { "(", ")" };
        public static readonly string[] CurlyBrackets = { "{", "}" };
        public static readonly string[] SquareBrackets = { "[", "]" };
        public static readonly string Semicolon = ";";
        public static readonly string[] UnaryFrontOperators = { "-", "!", "~", "++", "--" };
        public static readonly string[] UnaryBackOperators = { "++", "--" };
        public static readonly string[] BinaryOperators = 
            { "+", "-", "*", "/", "%", "<", ">", "&", "|", "^",
            "&&", "||", "+=", "==", "-=", "*=", "/=", "%=", "<=",
            ">=", "&=", "|=", "!=", "~=","<<", "<<=", ">>", ">>=" };
        public static readonly string[] Keywords =
            { "auto",  "double", "int", "struct",
            "break", "else", "long", "switch",
            "case", "enum", "register", "typedef",
            "char", "extern", "return", "union",
            "const", "float", "short", "unsigned",
            "continue", "for", "signed", "void",
            "default", "goto", "sizeof", "volatile",
            "do", "if", "static", "while" };
    }
}
