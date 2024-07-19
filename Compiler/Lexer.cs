using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Lexer;

namespace Lexer
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
        public override string ToString(){
            return "Program(\n\t"+FunctionNode.ToString()+"\n)";
        }  
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
        public override string ToString(){
            return "Function(\n\t\tname="+Identifier+"\",\n\t\tbody="+StatementNode.ToString()+"\n\t)";
        }  
    }
    class StatementNode : ASTNode {
        public ExpNode Value;
        public string Body;
        public StatementNode(ExpNode v) { Value = v; Body = ""; }
        public StatementNode(ExpNode v, string b) { Value = v; Body = b; }

        public override string ToString(){
            return "Return(\n\t\t\t"+Value.ToString()+"\n\t\t)";
        }
    }
    class ExpNode : ASTNode {
        public IntNode Value;
        public string DataType;
        public ExpNode(IntNode v) { Value = v; DataType = "Unknown";}
        public ExpNode(IntNode v, string d) { Value = v; DataType = d;}

        public override string ToString(){
            return DataType.ToString()+"("+Value.ToString()+")";
        }
    }
    class IdentifierNode : ASTNode {
        public string Value;
        public IdentifierNode(string v) { Value = v; }
    }
    class IntNode : ASTNode {
        public int Value;
        public IntNode(int v) { Value = v; }

        public override string ToString(){
            return Value.ToString();
        }
    }
}