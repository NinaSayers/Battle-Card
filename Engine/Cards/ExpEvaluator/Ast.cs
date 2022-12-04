using System.Collections.Generic;

namespace ExpEvaluator;

public enum Symbol { E, T, Add, Sub, Mult, Div, Asg, i, If, Then, Menor, Mayor, MenorI, MayorI, Igual, Dist, Invalid, Open, Close, Creace, Decreace , While, Do, X, Concat, S}
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

    public Statement GetAST()
    {
        
        if(children.Count == 3 && children[1].id >= Symbol.Add)
        {
            if(children[1].id >= Symbol.Add && children[1].id <= Symbol.Div)
            {
                return new BinaryExpression(children[0].GetAST() as Expression, children[2].GetAST() as Expression, SymbolNames[children[1].id]);
            }
            if(children[1].id >= Symbol.Menor && children[1].id <= Symbol.Dist)
            {
                return new BinaryBooleanExpression(children[0].GetAST() as Expression,children[2].GetAST() as Expression,SymbolNames[children[1].id]);
            }
            if(children[1].id == Symbol.Asg || children[1].id >= Symbol.Creace && children[1].id <= Symbol.Decreace)
            {
                return new Assign(children[0].token.Value, children[2].GetAST() as Expression, SymbolNames[children[1].id]);
            }
        }
        else if(id == Symbol.S)
        {
            List<Statement> statements = new List<Statement>();
            foreach (var child in children)
            {
                statements.Add(child.GetAST());
            } 
            return new Concat(statements);
        }
        else if(children.Count() == 1)
        {
            return children[0].GetAST();
        }
        else if(id == Symbol.If)
        {
            return new Conditional(children[0].GetAST() as BooleanExpression, children[1].GetAST());
        }
        else if(id == Symbol.While)
        {
            return new While(children[0].GetAST() as BooleanExpression,children[1].GetAST());
        }
        else if(children.Count() == 0 && id == Symbol.i)
        {
            return new Constant(Token.GetNumber(token));
        }
        else if(children.Count() == 0 && id == Symbol.X)
        {
            return new Variable(token.Value);
        }

        System.Console.WriteLine($"this node id = {id} and children count = {children.Count()}");
        System.Console.WriteLine($"the children id = {children[1].id}");
        foreach (var n in children)
        {
             System.Console.WriteLine($"this child node id = {id} and children count = {children.Count()}");
            System.Console.WriteLine($"the children id = {children[1].id}");
        }

        throw new Exception("The Parse Tree Is Invalid");
    }

    public readonly Dictionary<Symbol, string> SymbolNames = new Dictionary<Symbol, string>()
    {
        {Symbol.Add, "+"},
        {Symbol.Sub, "-"},
        {Symbol.Mult, "*"},
        {Symbol.Div, "/"},
        {Symbol.Asg, "="},
        {Symbol.Menor, "<"},
        {Symbol.Mayor, ">"},
        {Symbol.MenorI, "<="},
        {Symbol.MayorI, ">="},
        {Symbol.Igual, "=="},
        {Symbol.Dist, "!="},
        {Symbol.Creace, "+="},
        {Symbol.Decreace, "-="}
    };   
}