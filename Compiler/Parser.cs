using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Parser;
using Lexer;

namespace Parser
{
    static class Parser {

        static public ProgramNode Run(List<Token> tokens)
        {
            ProgramNode p = new ProgramNode(Parser.functionNode(tokens));
            //Console.WriteLine("Program:");
            //Console.WriteLine("Function: {0}", p.FunctionNode.Identifier);
            //Console.WriteLine("Statement: {0}", p.FunctionNode.StatementNode.Value.Value.Value);
            Console.Write(p);
            return p;
        }

        static public bool ExpectValue(string expected, List<Token> tokens)
        {
            string actual = tokens[0].strValue;
            if (actual == expected)
            {
                tokens.RemoveAt(0);
                return true;
            }
            else { return false; }
        }
        static public bool ExpectValue(string expected, List<Token> tokens, string errorMessage)
        {
            string actual = tokens[0].strValue;
            if (actual == expected)
            {
                tokens.RemoveAt(0);
                return true;
            }
            else { throw new Exception(errorMessage); }
        }

        static public bool CheckType(string expected, List<Token> tokens)
        {
            string actual = tokens[0].TokenType;
            if (actual == expected)
                return true;
            return false;
        }

        static public IntNode intNode(List<Token> tokens) {
            IntNode i;
            Token temp = tokens[0];
            //if (tokens[0].TokenType == "constant") {
            if (CheckType("constant", tokens)) {
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
            //if (tokens[0].TokenType == "identifier") {
            if (CheckType("identifier", tokens)) {
                IdentifierNode i = new IdentifierNode(tokens[0].strValue);
                tokens.Remove(tokens[0]);
                return i;
            }
            throw new Exception("Identifier parser called on non identifier");
        }

        static public ExpNode expNode(List<Token> tokens) {
            //if (tokens[0].TokenType == "constant")
            if (CheckType("constant", tokens))
            {
                return new ExpNode(Parser.intNode(tokens),"constant") ;
            }
                
            throw new Exception ("Exp Node parser called on non constant token");
        }

        static public StatementNode statementNode(List<Token> tokens) {
            if (tokens.Count<3)
                throw new Exception("Not enough tokens in Statement Parse");
            
            if(ExpectValue("return", tokens))
            //else if(tokens[0].TokenType=="return_keyword" && tokens[2].TokenType=="semicolon")
            {
                //tokens.Remove(tokens[0]);
                StatementNode s = new StatementNode(Parser.expNode(tokens),"Return");
                
                //tokens.Remove(tokens[0]);
                if(ExpectValue(";", tokens, "Semicolon expected"))
                    return s;
            }
            throw new Exception("Statement Parsing did return Exp Node correctly.");
                
        }

        static public FunctionNode functionNode(List<Token> tokens) {
            IdentifierNode i;
            StatementNode s;
            if (tokens.Count < 8)
                throw new Exception("Not enough tokens in Function parse");
            //if(tokens[0].TokenType=="int_keyword")
            if(ExpectValue("int", tokens, "Expected int keyword."))
            {
                //tokens.Remove(tokens[0]);
                i = Parser.identifierNode(tokens);
            }
            else
            { throw new Exception("Bad Identifier in Function Parse");}

            //if(tokens[0].TokenType=="open_paren" &&
            //   tokens[1].TokenType=="void_keyword" &&
            //   tokens[2].TokenType=="close_paren" &&
            //   tokens[3].TokenType=="open_brace")
            if(ExpectValue("(", tokens, "Expected Open Paren") &&
               ExpectValue("void", tokens, "Void Keyword Expected") &&
               ExpectValue(")", tokens, "Close Paren expected") &&
               ExpectValue("{", tokens, "Open Brace Expected") )
            {
                //tokens.RemoveRange(0,4);
                s = Parser.statementNode(tokens);
            }
            else
            {
                throw new Exception("Bad format for function contents");
            }
            //tokens.Remove(tokens[0]);
            ExpectValue("}", tokens, "Close Brace Expected");
            return new FunctionNode(i.Value, s);
               
        }
    }
}