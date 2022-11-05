using System.Collections.Generic;

namespace ExpEvaluator;

public enum Symbol { E, T, Add, Sub, Mult, Div, i, Invalid}
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
        }
        else if(children.Count() == 1)
        {
            return children[0].GetAST();
        }
        else if(children.Count() == 0 && id == Symbol.i)
        {
            return new Constant(Token.GetNumber(token));
        }

        throw new Exception("The Parse Tree Is Invalid");
    }
    
}