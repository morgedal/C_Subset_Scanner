using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scanner
{
    public class Program
    {
        const string HPATH = @"C:\Users\Maciek\Documents\test.txt";
        static List<char> list = new List<char>();
        static List<KeyValuePair<string, ETokenType>> Tokens = new List<KeyValuePair<string, ETokenType>>();

        public static void Main(string[] args)
        {
            var code = LoadSourceCode( HPATH );
            int index = 0;
            list.Add( code[index++] );

            while (index < code.Length)
            {
                var next = code[ index++ ];
                ETokenType tokenType;
                if ( MayBeToken(list, next, out tokenType) )
                {
                    list.Add(next);
                    continue;
                }
                else if (IsToken(list, out tokenType))
                    Tokens.Add( new KeyValuePair<string, ETokenType>( GetString(list), tokenType ) );
                else
                    Tokens.Add(new KeyValuePair<string, ETokenType>( GetString(list), ETokenType.E_ERROR ) );

                list.Clear();
            }

            foreach (var token in Tokens)
                Console.WriteLine("{0} -> {1}", token.Key, token.Value.ToString());
            Console.ReadKey();

        }

        private static string GetString(List<char> list)
        {
            string text = "";
            foreach ( var c in list )
                text = text + c;
            return text;
        }

        private static bool MayBeToken(List<char> list, char next, out ETokenType tokenType)
        {
            var listWithNext = list.ToList();
            listWithNext.Add(next);

            // we can ignore checking if ( list + next ) can be keyword, because set of keywors is subset of possible id's, 
            // so if sth is not an id, it for sure won't be a keyword 
            return IsToken(listWithNext, out tokenType);
        }

        private static bool IsToken(List<char> list, out ETokenType tokenType)
        {
            var a = GetString( list );

            if (TokenTypeTabs.RoundBrackets.Contains(a))
                tokenType = ETokenType.E_SQUARE_BRACKET;
            else if (TokenTypeTabs.CurlyBrackets.Contains(a))
                tokenType = ETokenType.E_CURLY_BRACKET;
            else if (TokenTypeTabs.SquareBrackets.Contains(a))
                tokenType = ETokenType.E_SQUARE_BRACKET;
            else if (TokenTypeTabs.Semicolon == a)
                tokenType = ETokenType.E_SEMICOLON;
            else if (TokenTypeTabs.UnaryFrontOperators.Contains(a))
                tokenType = ETokenType.E_UNARY_FRONT_OPERATOR;
            else if (TokenTypeTabs.UnaryBackOperators.Contains(a))
                tokenType = ETokenType.E_UNARY_BACK_OPERATOR;
            else if (TokenTypeTabs.BinaryOperators.Contains(a))
                tokenType = ETokenType.E_BINARY_OPERATOR;
            else if (TokenTypeTabs.Keywords.Contains(a))
                tokenType = ETokenType.E_KEYWORD;
            else if (a.StartsWith("/*") && a.EndsWith("*/"))
                tokenType = ETokenType.E_COMMENT;
            else if (a.StartsWith("\"") && a.EndsWith("\"") && a.Length > 1)
                tokenType = ETokenType.E_STRING;
            else if (IsNumber(a))
                tokenType = ETokenType.E_NUMBER;
            else if (IsID(a))
                tokenType = ETokenType.E_ID;
            else
            {
                tokenType = ETokenType.E_ERROR;
                return false;
            }

            return true;
        }

        private static bool IsNumber( string a )
        {
            Regex regex = new Regex( @"^[0-9]+(\.[0-9]+)?$" );
            Match match = regex.Match(a);
            return match.Success;
        }

        private static bool IsID( string a )
        {
            Regex regex = new Regex( @"^[_a-zA-Z][_a-zA-Z0-9]*$" );
            Match match = regex.Match(a);
            return match.Success;
        }

        private static string LoadSourceCode( string path )
        {
            string text;
            using (var streamReader = new StreamReader( path , Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }

            return text;
        }
    }

}
