using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner
{
    public class HTMLBuilder
    {
        public static void Build( List<KeyValuePair<string, ETokenType>> tokens, string path )
        {
            var preparedString = PrepareString( tokens );

            using ( System.IO.StreamWriter file = new System.IO.StreamWriter( path, false ) )
                file.Write( preparedString );
        }

        private static string PrepareString( List<KeyValuePair<string, ETokenType>> tokens )
        {
            StringBuilder builder = new StringBuilder();
            
            foreach( var token in tokens )
            {
                if ( token.Value == ETokenType.E_EMPTY && ( token.Key == "\n" || token.Key == "\r\n" ) )
                    builder.Append( "<br>" );
                if ( token.Value == ETokenType.E_EMPTY && token.Key == " " )
                    builder.Append( "&nbsp;" );
                if (token.Value == ETokenType.E_ERROR)
                    builder.Append(String.Format("<font color={0}>ERROR{1}</font>", GetColorForToken(token.Value), token.Key));
                else
                    builder.Append(String.Format("<font color={0}>{1}</font>", GetColorForToken(token.Value), token.Key));
            }

            return builder.ToString();
        }

        private static string GetColorForToken( ETokenType tokenType )
        {
            string color = "";

            switch ( tokenType )
            {
                case ETokenType.E_KEYWORD: color = "blue"; break;
                case ETokenType.E_ID: color = "magenta"; break;
                case ETokenType.E_NUMBER: color = "cyan"; break;
                case ETokenType.E_STRING: color = "green"; break;
                case ETokenType.E_ROUND_BRACKET: color = "navy"; break;
                case ETokenType.E_CURLY_BRACKET: color = "olive"; break;
                case ETokenType.E_SQUARE_BRACKET: color = "orange"; break;
                case ETokenType.E_SEMICOLON: color = "pink"; break;
                case ETokenType.E_UNARY_FRONT_OPERATOR: color = "salmon"; break;
                case ETokenType.E_UNARY_BACK_OPERATOR: color = "silver"; break;
                case ETokenType.E_BINARY_OPERATOR: color = "steelblue"; break;
                case ETokenType.E_COMMENT: color = "teal"; break;
                case ETokenType.E_ERROR: color = "red"; break;
                case ETokenType.E_EMPTY: color = "white"; break;
                default: break;
            }

            return color;
        }

    }

}
