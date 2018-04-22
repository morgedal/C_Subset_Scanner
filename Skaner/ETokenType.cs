using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner
{
    public enum ETokenType
    {
        E_KEYWORD,
        E_ID,
        E_NUMBER,
        E_STRING,
        E_ROUND_BRACKET,
        E_CURLY_BRACKET,
        E_SQUARE_BRACKET,
        E_SEMICOLON,
        E_UNARY_FRONT_OPERATOR,
        E_UNARY_BACK_OPERATOR,
        E_BINARY_OPERATOR,
        E_COMMENT,
        E_ERROR,
        E_EMPTY
    }
}
