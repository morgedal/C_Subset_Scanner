using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scanner
{
    /****************************************************************
     *  USAGE: 
     *  args[0] - path to source code
     *  args[1] - path to destination html file
     ****************************************************************/
    public class Program
    {
        
        

        public static void Main(string[] args)
        {
            string sourcePath = args[0];
            string destinationPath = args[1];

            List<char> Chars = new List<char>();
            List<KeyValuePair<string, ETokenType>> Tokens = new List<KeyValuePair<string, ETokenType>>();

            var code = LoadSourceCode( sourcePath );
            int index = 0;
            Chars.Add( code[index++] );

            while (index < code.Length)
            {
                var next = code[index++];
                ETokenType tokenType;
                if (MayBeToken(Chars, next, out tokenType))
                {
                    Chars.Add(next);
                    continue;
                }
                else if (IsToken(Chars, out tokenType))
                {
                    Tokens.Add(new KeyValuePair<string, ETokenType>(GetString(Chars), tokenType));
                    index--;
                }
                else
                    Tokens.Add(new KeyValuePair<string, ETokenType>(GetString(Chars), ETokenType.E_ERROR));

                Chars.Clear();
            }

            //foreach (var token in Tokens.Where( x => x.Value != ETokenType.E_EMPTY ) )
            //    Console.WriteLine("{0} -> {1}", token.Key, token.Value.ToString());
            //Console.ReadKey();

            HTMLBuilder.Build(Tokens, destinationPath);

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
            tokenType = ETokenType.E_ERROR;
            var listWithNext = list.ToList();
            listWithNext.Add(next);

            if (MayBeComment(listWithNext, ref tokenType) || MayBeString(listWithNext, ref tokenType))
                return true;

            // we can ignore checking if ( list + next ) can be keyword, because set of keywors is subset of possible id's, 
            // so if sth is not an id, it for sure won't be a keyword 
            return IsToken(listWithNext, out tokenType);
        }

        private static bool MayBeComment( List<char> listWithNext, ref ETokenType tokenType  )
        {
            if (listWithNext.Count == 1 && listWithNext[0] == '/')
            {
                tokenType = ETokenType.E_STRING;
                return true;
            }
            else if (listWithNext.Count > 1
                && GetString(listWithNext).StartsWith("/*")
                && !GetString(listWithNext).Contains("*/"))
            {
                tokenType = ETokenType.E_COMMENT;
                return true;
            }

            return false;
        }

        private static bool MayBeString(List<char> listWithNext, ref ETokenType tokenType)
        {
            if (GetString(listWithNext).StartsWith("\"")
                && !GetString(listWithNext).Substring(1).Contains("\""))
            {
                tokenType = ETokenType.E_STRING;
                return true;
            }

            return false;
        }

        private static bool IsToken(List<char> list, out ETokenType tokenType)
        {
            var a = GetString( list );

            if (a == " " || a == "\n" || a == "\r" )
                tokenType = ETokenType.E_EMPTY;

            else if (a.StartsWith("/*") && a.EndsWith("*/"))
                tokenType = ETokenType.E_COMMENT;

            else if (a.StartsWith("\"") && a.EndsWith("\"") && a.Length > 1)
                tokenType = ETokenType.E_STRING;

            else if (TokenTypeTabs.RoundBrackets.Contains(a))
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
            Regex regex = new Regex( @"^[0-9]+(\.)?([0-9]+)?$" );
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
