using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using LexerParser;
using Parser;
using Lexer;
using ASDLGenerator;
using System.IO;

namespace LexerParser
{
    class Program
    {
        static void Main(string[] args)
        {
            /*//*/
            List<rexTokenType> rexTokens = new List<rexTokenType>();
            List<Token> tokens = new List<Token>();
            Lexer.Lexer.Add("whitespace", @"^\s");
            Lexer.Lexer.Add("constant", @"^[0-9]+\b");
            Lexer.Lexer.Add("int_keyword", @"^int\b");
            Lexer.Lexer.Add("void_keyword", @"^void\b");
            Lexer.Lexer.Add("return_keyword", @"^return\b");
            Lexer.Lexer.Add("identifier", @"^[a-zA-Z_]\w*\b");
            Lexer.Lexer.Add("open_paren", @"^\(");
            Lexer.Lexer.Add("close_paren", @"^\)");
            Lexer.Lexer.Add("open_brace", @"^{");
            Lexer.Lexer.Add("close_brace", @"^}");
            Lexer.Lexer.Add("semicolon", @"^;");
            ///*/
            ///
            string code;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(paramFileInput);

                code = sr.ReadToEnd();
                
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }

            string test = "int main(void) { return 22; }";

            try{
                tokens = Lexer.Lexer.Run(test);
            }
            catch(Exception e) { Console.WriteLine(e.Message);}

            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }

            ProgramNode pn = Parser.Parser.Run(tokens);
            ProgramConstruct asdl = ASDLGenerator.ASDLGenerator.Run(pn);
            
            Console.WriteLine("\n\n***\n");
            Console.Write(asdl.ToString());
        }
    }
}