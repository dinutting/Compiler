 using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Lexer;
 
 namespace ASDLGenerator {
    abstract class ASDLConstruct {}
    class ProgramConstruct : ASDLConstruct {
        public FunctionConstruct FunctionConstruct;
        
        public ProgramConstruct(FunctionConstruct fc)
        {
            FunctionConstruct = fc;
        }
        public override string ToString(){
            return "Program(\n\t"+FunctionConstruct.ToString()+"\n)";
        }  
    }

    class FunctionConstruct : ASDLConstruct {
        public StatementConstruct StatementConstruct;
        public FunctionConstruct (StatementConstruct sc)
        {
            StatementConstruct = sc;
        }
        public override string ToString(){
            return "Function(\n\t\t"+StatementConstruct.ToString()+"\n\t)";
        }
    }

    class StatementConstruct : ASDLConstruct {
        public List<InstructionConstruct> InstructionConstructs;
        public StatementConstruct (List <InstructionConstruct> ics)
        {
            InstructionConstructs = ics;
        }
        public override string ToString() {
            string temp = "Statement(";
            foreach(InstructionConstruct ins in InstructionConstructs) 
                temp += ins.ToString();
            temp += "\n\t\t)";
            return temp;
        }
    }

    class InstructionConstruct : ASDLConstruct {
        public string instruction;
        public List<string>? operands;
        public InstructionConstruct(string instruction, List<string> operands) {
            this.instruction = instruction;
            this.operands = operands;
        }
        public InstructionConstruct(string instruction) {
            this.instruction=instruction;
        }
        public override string ToString()
        {
            string temp =  "\n\t\t\t"+instruction;
            if(operands!=null)
            {foreach (string operand in operands)
                {temp += "operand "+operand;}}
            //temp += "\n";
            return temp;
        }
    }
    
    static class ASDLGenerator {
        public static List<InstructionConstruct> AST2ASDLi(StatementNode sn) {
            List<InstructionConstruct> ic = new List<InstructionConstruct>();

            if (sn.Body=="Return")
            {
                ic.Add(new InstructionConstruct("Mov("+sn.Value.Value+",EAX)"));
                ic.Add(new InstructionConstruct("RET"));
            }
            return ic;
        }

        public static ProgramConstruct Run(ProgramNode pn) {
            ProgramConstruct pc = new ProgramConstruct(
                new FunctionConstruct(
                    new StatementConstruct(AST2ASDLi(pn.FunctionNode.StatementNode))
                    )
                );
            return pc;
        }
    }

 }
    