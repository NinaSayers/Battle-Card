using System.Collections.Generic;

namespace ExpEvaluator;

public enum Symbol { E, T, Add, Sub, Mult, Div, Asg, i, If, Then, Menor, Mayor, MenorI, MayorI, Igual, Dist, Invalid}
public class Node
{
    public Symbol id;
    public List<Node> children {get; private set;}
    public Token token;
    public Node(Symbol symbol, List<Node> children, Token token)
    {
        id = symbol;
        this.children = children;
        this.token = token;
    } 

    public Expression GetAST()
    {
        // System.Console.WriteLine(id);
        // System.Console.WriteLine(children.Count);
        // foreach(Node n in children)
        // {
        //     System.Console.WriteLine(" ");
        //     System.Console.WriteLine(n.id);
        //     System.Console.WriteLine(n.children.Count);
        //     if(n.children.Count() > 1)
        //     {
        //         foreach(Node m in n.children)
        //         {
        //             System.Console.WriteLine(" ");
        //             System.Console.WriteLine(m.id);
        //             System.Console.WriteLine(m.children.Count);
        //         }
        //     }
        // }
        
        if(children.Count == 3 && ((int)children[1].id) >= 2)
        {
            if(children[1].id == Symbol.Add)
            {
                return new Add(children[0].GetAST(), children[2].GetAST());
            }
            if(children[1].id == Symbol.Sub)
            {
                return new Substract(children[0].GetAST(), children[2].GetAST());
            }
            if(children[1].id == Symbol.Mult)
            {
                return new Multiply(children[0].GetAST(), children[2].GetAST());
            }
            if(children[1].id == Symbol.Div)
            {
                return new Divide(children[0].GetAST(), children[2].GetAST());
            }
            if(children[1].id == Symbol.Asg)
            {
                return new Assign(children[0].GetAST(), children[2].GetAST());
            }
            if(children[1].id == Symbol.Menor)
            {
                return new BooleanExpression(children[0].GetAST(),children[2].GetAST(),"<");
            }
            if(children[1].id == Symbol.Mayor)
            {
                return new BooleanExpression(children[0].GetAST(),children[2].GetAST(),">");
            }
            if(children[1].id == Symbol.MenorI)
            {
                return new BooleanExpression(children[0].GetAST(),children[2].GetAST(),"<=");
            }
            if(children[1].id == Symbol.MayorI)
            {
                return new BooleanExpression(children[0].GetAST(),children[2].GetAST(),">=");
            }
            if(children[1].id == Symbol.Igual)
            {
                return new BooleanExpression(children[0].GetAST(),children[2].GetAST(),"==");
            }
            if(children[1].id == Symbol.Dist)
            {
                return new BooleanExpression(children[0].GetAST(),children[2].GetAST(),"!=");
            }
        }
        else if(children.Count() == 1)
        {
            return children[0].GetAST();
        }
        else if(id == Symbol.If)
        {
            if(children[0].GetAST().Evaluate() == 1)
            {
                return children[1].GetAST();
            }
            else
            {
                return new Constant(0);
            }
        }
        else if(children.Count() == 0 && id == Symbol.i)
        {
            return new Constant(Token.GetNumber(token));
        }

        throw new Exception("The Parse Tree Is Invalid");
    }
    
}