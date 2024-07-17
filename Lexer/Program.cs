using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using LexerParser;

namespace LexerParser
{
    class rexTokenType{
        public string name;
        public Regex regex;

        public rexTokenType(string n, string r)
        {
            name = n;
            regex = new Regex(r);
        }
        public bool isMatch(string s)
        {
            return (regex.Match(s).Success);
        }
        public string match(string s)
        {
            return regex.Match(s).Value;
        }
        public int matchLength(string s)
        {
            return regex.Match(s).Value.Length;
        }
    }
   /* enum enumToken{
        whitespace,
        constant,
        int_keyword,
        void_keyword,
        return_keyword,
        identifier,
        open_paren,
        close_paren,
        open_brace,
        close_brace,
        semicolon
    } //*/
    class Token { 
        public string TokenType;
        public string strValue;
        public Token(string val, string eTT)
        {
            TokenType = eTT;
            strValue = val;
        }
        int intValue() { 
            if (TokenType == "constant") return Int32.Parse(strValue);
            else { throw new Exception("Token int Value called, but rexTokenType not a constant."); }
        }

        public override string ToString(){
            return "Tok: "+strValue+" is a "+TokenType+" token type.";
        }  
    }

    static class Lexer {
        static List <rexTokenType> rexTokens = new List<rexTokenType> ();

        public static void Add(string name, string r){
            rexTokens.Add( new rexTokenType(name, r) );
        }

        public static List<Token> Run(string s)
        {
            int len = s.Length;
            List<Token> tokens = new List<Token>();
            
            for (int i = 0; i < len;)
            {
                if (s[i]==' ')
                {
                    i++; continue;
                }
                string sub = s.Substring(i);
                //Console.WriteLine("Testing \"{0}\"", sub);

                rexTokenType tempToken= new rexTokenType("null", "");
                int tempLength=0;
                foreach (rexTokenType token in rexTokens)
                {
                    bool isMatch = token.isMatch(sub);
                    int matchLength = token.matchLength(sub);
                    if (isMatch && matchLength>tempLength)
                    {
                        tempToken = token;
                        tempLength = token.matchLength(sub);
                    }
                }
                if (tempToken.name=="null")
                { 
                    string strangeToken = (new Regex(@"[^\b]+\b")).Match(sub).Value;
                    throw new Exception("Invalid token \""+strangeToken+"\" found at position " +i.ToString() +"."); }
                //enumToken eTT;
                //Enum.TryParse(tempToken.name, out eTT);
                tokens.Add( new Token(tempToken.match(sub), tempToken.name) );


                //Console.WriteLine("{0} is a {1} token type", tempToken.match(sub), tempToken.name);
                i+=tempLength;
            }

            return tokens;
        }
    }

    abstract class ASTNode {
     /*    protected Guid guid;
        protected string nodeType;

        protected ASTNode parentNode;
        protected List<ASTNode> childrenNodes; */

        //public abstract void parse();
    }

    class ProgramNode : ASTNode {
        
        public FunctionNode FunctionNode;
        public ProgramNode(FunctionNode fn)
        {
            FunctionNode = fn;
        }
        //public override void parse() {  
        //}
    }
    class FunctionNode : ASTNode {
        public string Identifier;
        public StatementNode StatementNode;
        public FunctionNode(string i, StatementNode s)
        {
            Identifier = i;
            StatementNode = s;
            //parentNode = pn;
        }
    }
    class StatementNode : ASTNode {
        public ExpNode Value;
        public StatementNode(ExpNode v) { Value = v; }
    }
    class ExpNode : ASTNode {
        public IntNode Value;
        public ExpNode(IntNode v) { Value = v;}
    }
    class IdentifierNode : ASTNode {
        public string Value;
        public IdentifierNode(string v) { Value = v; }
    }
    class IntNode : ASTNode {
        public int Value;
        public IntNode(int v) { Value = v; }
    }

    static class Parser {

        /// <summary>
        /// TODO
        /// Update the Parsing so that more meta information is available for each level.
        /// Right now I'm ignoring keywords like Return and I'm not identifying the constant.
        /// Chuck the Pretty Print on page 17.
        /// 
        /// Additionally, add a to string override for the pretty print at each node
        /// level so that I don't have to call Value.Value.Value.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>

        static public ProgramNode Run(List<Token> tokens)
        {
            ProgramNode p = new ProgramNode(Parser.functionNode(tokens));
            Console.WriteLine("Program:");
            Console.WriteLine("Function: {0}", p.FunctionNode.Identifier);
            Console.WriteLine("Statement: {0}", p.FunctionNode.StatementNode.Value.Value.Value);

            return p;
        }

        static public IntNode intNode(List<Token> tokens) {
            IntNode i;
            Token temp = tokens[0];
            if (tokens[0].TokenType == "constant") {
                try {
                    i = new IntNode(Int32.Parse(tokens[0].strValue));
                    tokens.Remove(tokens[0]);
                    return i;
                }
                catch (Exception e) {
                    Console.WriteLine ("Problem parsing constant as integer. {0}", temp);
                    Console.WriteLine (e.Message);
                    throw;
                }
            }
            throw new Exception("IntNode Parsing invoked on Non integer/non constant.");
        }
        static public IdentifierNode identifierNode(List<Token> tokens) {
            if (tokens[0].TokenType == "identifier") {
                IdentifierNode i = new IdentifierNode(tokens[0].strValue);
                tokens.Remove(tokens[0]);
                return i;
            }
            throw new Exception("Identifier parser called on non identifier");
        }

        static public ExpNode expNode(List<Token> tokens) {
            if (tokens[0].TokenType == "constant")
            {
                return new ExpNode(Parser.intNode(tokens)) ;
            }
                
            throw new Exception ("Exp Node parser called on non constant token");
        }

        static public StatementNode statementNode(List<Token> tokens) {
            if (tokens.Count<3)
                throw new Exception("Not enough tokens in Statement Parse");
            
            else if(tokens[0].TokenType=="return_keyword" && tokens[2].TokenType=="semicolon")
            {
                tokens.Remove(tokens[0]);
                StatementNode s = new StatementNode(Parser.expNode(tokens));
                tokens.Remove(tokens[0]);
                return s;
            }
            throw new Exception("Statement Parsing did return Exp Node correctly.");
                
        }

        static public FunctionNode functionNode(List<Token> tokens) {
            IdentifierNode i;
            StatementNode s;
            if (tokens.Count < 8)
                throw new Exception("Not enough tokens in Function parse");
            if(tokens[0].TokenType=="int_keyword")
            {
                tokens.Remove(tokens[0]);
                i = Parser.identifierNode(tokens);
            }
            else
            { throw new Exception("Bad Identifier in Function Parse");}

            if(tokens[0].TokenType=="open_paren" &&
               tokens[1].TokenType=="void_keyword" &&
               tokens[2].TokenType=="close_paren" &&
               tokens[3].TokenType=="open_brace")
            {
                tokens.RemoveRange(0,4);
                s = Parser.statementNode(tokens);
            }
            else
            {
                throw new Exception("Bad format for function contents");
            }
            tokens.Remove(tokens[0]);
            return new FunctionNode(i.Value, s);
               
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
            List<rexTokenType> rexTokens = new List<rexTokenType>();
            List<Token> tokens = new List<Token>();
            Lexer.Add("whitespace", @"^\s");
            Lexer.Add("constant", @"^[0-9]+\b");
            Lexer.Add("int_keyword", @"^int\b");
            Lexer.Add("void_keyword", @"^void\b");
            Lexer.Add("return_keyword", @"^return\b");
            Lexer.Add("identifier", @"^[a-zA-Z_]\w*\b");
            Lexer.Add("open_paren", @"^\(");
            Lexer.Add("close_paren", @"^\)");
            Lexer.Add("open_brace", @"^{");
            Lexer.Add("close_brace", @"^}");
            Lexer.Add("semicolon", @"^;");

            string test = "int main(void) { return 22; }";

            try{
                tokens = Lexer.Run(test);
            }
            catch(Exception e) { Console.WriteLine(e.Message);}

            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }

            Parser.Run(tokens);
        }
    }
}