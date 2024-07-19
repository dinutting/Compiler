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

namespace LexerParser
{
//     class rexTokenType{
//         public string name;
//         public Regex regex;

//         public rexTokenType(string n, string r)
//         {
//             name = n;
//             regex = new Regex(r);
//         }
//         public bool isMatch(string s)
//         {
//             return (regex.Match(s).Success);
//         }
//         public string match(string s)
//         {
//             return regex.Match(s).Value;
//         }
//         public int matchLength(string s)
//         {
//             return regex.Match(s).Value.Length;
//         }
//     }
//    /* enum enumToken{
//         whitespace,
//         constant,
//         int_keyword,
//         void_keyword,
//         return_keyword,
//         identifier,
//         open_paren,
//         close_paren,
//         open_brace,
//         close_brace,
//         semicolon
//     } //*/
//     class Token { 
//         public string TokenType;
//         public string strValue;
//         public Token(string val, string eTT)
//         {
//             TokenType = eTT;
//             strValue = val;
//         }
//         int intValue() { 
//             if (TokenType == "constant") return Int32.Parse(strValue);
//             else { throw new Exception("Token int Value called, but rexTokenType not a constant."); }
//         }

//         public override string ToString(){
//             return "Tok: "+strValue+" is a "+TokenType+" token type.";
//         }  
//     }

//     static class Lexer {
//         static List <rexTokenType> rexTokens = new List<rexTokenType> ();

//         public static void Add(string name, string r){
//             rexTokens.Add( new rexTokenType(name, r) );
//         }

//         public static List<Token> Run(string s)
//         {
//             int len = s.Length;
//             List<Token> tokens = new List<Token>();
            
//             for (int i = 0; i < len;)
//             {
//                 if (s[i]==' ')
//                 {
//                     i++; continue;
//                 }
//                 string sub = s.Substring(i);
//                 //Console.WriteLine("Testing \"{0}\"", sub);

//                 rexTokenType tempToken= new rexTokenType("null", "");
//                 int tempLength=0;
//                 foreach (rexTokenType token in rexTokens)
//                 {
//                     bool isMatch = token.isMatch(sub);
//                     int matchLength = token.matchLength(sub);
//                     if (isMatch && matchLength>tempLength)
//                     {
//                         tempToken = token;
//                         tempLength = token.matchLength(sub);
//                     }
//                 }
//                 if (tempToken.name=="null")
//                 { 
//                     string strangeToken = (new Regex(@"[^\b]+\b")).Match(sub).Value;
//                     throw new Exception("Invalid token \""+strangeToken+"\" found at position " +i.ToString() +"."); }
//                 //enumToken eTT;
//                 //Enum.TryParse(tempToken.name, out eTT);
//                 tokens.Add( new Token(tempToken.match(sub), tempToken.name) );


//                 //Console.WriteLine("{0} is a {1} token type", tempToken.match(sub), tempToken.name);
//                 i+=tempLength;
//             }

//             return tokens;
//         }
//     }
//     abstract class ASTNode {
//      /*    protected Guid guid;
//         protected string nodeType;

//         protected ASTNode parentNode;
//         protected List<ASTNode> childrenNodes; */

//         //public abstract void parse();
//     }
//     class ProgramNode : ASTNode {
        
//         public FunctionNode FunctionNode;
//         public ProgramNode(FunctionNode fn)
//         {
//             FunctionNode = fn;
//         }
//         public override string ToString(){
//             return "Program(\n\t"+FunctionNode.ToString()+"\n)";
//         }  
//     }
//     class FunctionNode : ASTNode {
//         public string Identifier;
//         public StatementNode StatementNode;
//         public FunctionNode(string i, StatementNode s)
//         {
//             Identifier = i;
//             StatementNode = s;
//             //parentNode = pn;
//         }
//         public override string ToString(){
//             return "Function(\n\t\tname="+Identifier+"\",\n\t\tbody="+StatementNode.ToString()+"\n\t)";
//         }  
//     }
//     class StatementNode : ASTNode {
//         public ExpNode Value;
//         public string Body;
//         public StatementNode(ExpNode v) { Value = v; Body = ""; }
//         public StatementNode(ExpNode v, string b) { Value = v; Body = b; }

//         public override string ToString(){
//             return "Return(\n\t\t\t"+Value.ToString()+"\n\t\t)";
//         }
//     }
//     class ExpNode : ASTNode {
//         public IntNode Value;
//         public string DataType;
//         public ExpNode(IntNode v) { Value = v; DataType = "Unknown";}
//         public ExpNode(IntNode v, string d) { Value = v; DataType = d;}

//         public override string ToString(){
//             return DataType.ToString()+"("+Value.ToString()+")";
//         }
//     }
//     class IdentifierNode : ASTNode {
//         public string Value;
//         public IdentifierNode(string v) { Value = v; }
//     }
//     class IntNode : ASTNode {
//         public int Value;
//         public IntNode(int v) { Value = v; }

//         public override string ToString(){
//             return Value.ToString();
//         }
//     }

    // static class Parser {

    //     static public ProgramNode Run(List<Token> tokens)
    //     {
    //         ProgramNode p = new ProgramNode(Parser.functionNode(tokens));
    //         //Console.WriteLine("Program:");
    //         //Console.WriteLine("Function: {0}", p.FunctionNode.Identifier);
    //         //Console.WriteLine("Statement: {0}", p.FunctionNode.StatementNode.Value.Value.Value);
    //         Console.Write(p);
    //         return p;
    //     }

    //     static public bool ExpectValue(string expected, List<Token> tokens)
    //     {
    //         string actual = tokens[0].strValue;
    //         if (actual == expected)
    //         {
    //             tokens.RemoveAt(0);
    //             return true;
    //         }
    //         else { return false; }
    //     }
    //     static public bool ExpectValue(string expected, List<Token> tokens, string errorMessage)
    //     {
    //         string actual = tokens[0].strValue;
    //         if (actual == expected)
    //         {
    //             tokens.RemoveAt(0);
    //             return true;
    //         }
    //         else { throw new Exception(errorMessage); }
    //     }

    //     static public bool CheckType(string expected, List<Token> tokens)
    //     {
    //         string actual = tokens[0].TokenType;
    //         if (actual == expected)
    //             return true;
    //         return false;
    //     }

    //     static public IntNode intNode(List<Token> tokens) {
    //         IntNode i;
    //         Token temp = tokens[0];
    //         //if (tokens[0].TokenType == "constant") {
    //         if (CheckType("constant", tokens)) {
    //             try {
    //                 i = new IntNode(Int32.Parse(tokens[0].strValue));
    //                 tokens.Remove(tokens[0]);
    //                 return i;
    //             }
    //             catch (Exception e) {
    //                 Console.WriteLine ("Problem parsing constant as integer. {0}", temp);
    //                 Console.WriteLine (e.Message);
    //                 throw;
    //             }
    //         }
    //         throw new Exception("IntNode Parsing invoked on Non integer/non constant.");
    //     }
    //     static public IdentifierNode identifierNode(List<Token> tokens) {
    //         //if (tokens[0].TokenType == "identifier") {
    //         if (CheckType("identifier", tokens)) {
    //             IdentifierNode i = new IdentifierNode(tokens[0].strValue);
    //             tokens.Remove(tokens[0]);
    //             return i;
    //         }
    //         throw new Exception("Identifier parser called on non identifier");
    //     }

    //     static public ExpNode expNode(List<Token> tokens) {
    //         //if (tokens[0].TokenType == "constant")
    //         if (CheckType("constant", tokens))
    //         {
    //             return new ExpNode(Parser.intNode(tokens),"constant") ;
    //         }
                
    //         throw new Exception ("Exp Node parser called on non constant token");
    //     }

    //     static public StatementNode statementNode(List<Token> tokens) {
    //         if (tokens.Count<3)
    //             throw new Exception("Not enough tokens in Statement Parse");
            
    //         if(ExpectValue("return", tokens))
    //         //else if(tokens[0].TokenType=="return_keyword" && tokens[2].TokenType=="semicolon")
    //         {
    //             //tokens.Remove(tokens[0]);
    //             StatementNode s = new StatementNode(Parser.expNode(tokens),"Return");
                
    //             //tokens.Remove(tokens[0]);
    //             if(ExpectValue(";", tokens, "Semicolon expected"))
    //                 return s;
    //         }
    //         throw new Exception("Statement Parsing did return Exp Node correctly.");
                
    //     }

    //     static public FunctionNode functionNode(List<Token> tokens) {
    //         IdentifierNode i;
    //         StatementNode s;
    //         if (tokens.Count < 8)
    //             throw new Exception("Not enough tokens in Function parse");
    //         //if(tokens[0].TokenType=="int_keyword")
    //         if(ExpectValue("int", tokens, "Expected int keyword."))
    //         {
    //             //tokens.Remove(tokens[0]);
    //             i = Parser.identifierNode(tokens);
    //         }
    //         else
    //         { throw new Exception("Bad Identifier in Function Parse");}

    //         //if(tokens[0].TokenType=="open_paren" &&
    //         //   tokens[1].TokenType=="void_keyword" &&
    //         //   tokens[2].TokenType=="close_paren" &&
    //         //   tokens[3].TokenType=="open_brace")
    //         if(ExpectValue("(", tokens, "Expected Open Paren") &&
    //            ExpectValue("void", tokens, "Void Keyword Expected") &&
    //            ExpectValue(")", tokens, "Close Paren expected") &&
    //            ExpectValue("{", tokens, "Open Brace Expected") )
    //         {
    //             //tokens.RemoveRange(0,4);
    //             s = Parser.statementNode(tokens);
    //         }
    //         else
    //         {
    //             throw new Exception("Bad format for function contents");
    //         }
    //         //tokens.Remove(tokens[0]);
    //         ExpectValue("}", tokens, "Close Brace Expected");
    //         return new FunctionNode(i.Value, s);
               
    //     }
    // }
    
    // abstract class ASDLConstruct {}
    // class ProgramConstruct : ASDLConstruct {
    //     public FunctionConstruct FunctionConstruct;
        
    //     public ProgramConstruct(FunctionConstruct fc)
    //     {
    //         FunctionConstruct = fc;
    //     }
    //     public override string ToString(){
    //         return "Program(\n\t"+FunctionConstruct.ToString()+"\n)";
    //     }  
    // }

    // class FunctionConstruct : ASDLConstruct {
    //     public StatementConstruct StatementConstruct;
    //     public FunctionConstruct (StatementConstruct sc)
    //     {
    //         StatementConstruct = sc;
    //     }
    //     public override string ToString(){
    //         return "Function(\n\t\t"+StatementConstruct.ToString()+"\n\t)";
    //     }
    // }

    // class StatementConstruct : ASDLConstruct {
    //     public List<InstructionConstruct> InstructionConstructs;
    //     public StatementConstruct (List <InstructionConstruct> ics)
    //     {
    //         InstructionConstructs = ics;
    //     }
    //     public override string ToString() {
    //         string temp = "Statement(";
    //         foreach(InstructionConstruct ins in InstructionConstructs) 
    //             temp += ins.ToString();
    //         temp += "\n\t\t)";
    //         return temp;
    //     }
    // }

    // class InstructionConstruct : ASDLConstruct {
    //     public string instruction;
    //     public List<string>? operands;
    //     public InstructionConstruct(string instruction, List<string> operands) {
    //         this.instruction = instruction;
    //         this.operands = operands;
    //     }
    //     public InstructionConstruct(string instruction) {
    //         this.instruction=instruction;
    //     }
    //     public override string ToString()
    //     {
    //         string temp =  "\n\t\t\t"+instruction;
    //         if(operands!=null)
    //         {foreach (string operand in operands)
    //             {temp += "operand "+operand;}}
    //         //temp += "\n";
    //         return temp;
    //     }
    // }
    
    // static class ASDLGenerator {
    //     public static List<InstructionConstruct> AST2ASDLi(StatementNode sn) {
    //         List<InstructionConstruct> ic = new List<InstructionConstruct>();

    //         if (sn.Body=="Return")
    //         {
    //             ic.Add(new InstructionConstruct("Mov("+sn.Value.Value+",EAX)"));
    //             ic.Add(new InstructionConstruct("RET"));
    //         }
    //         return ic;
    //     }

    //     public static ProgramConstruct Run(ProgramNode pn) {
    //         ProgramConstruct pc = new ProgramConstruct(
    //             new FunctionConstruct(
    //                 new StatementConstruct(AST2ASDLi(pn.FunctionNode.StatementNode))
    //                 )
    //             );
    //         return pc;
    //     }
    // }
    class Program
    {
        static void Main(string[] args)
        {
            
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