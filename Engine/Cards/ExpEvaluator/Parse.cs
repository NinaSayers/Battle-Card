namespace ExpEvaluator;

public class Parser
{
    public static Node Parse(List<Token> list)
    {
        return Parse(list, 0, new List<Node>(), list.Count);
    }
    private static Node Parse(List<Token> list, int i, List<Node> result, int top)
    {
        if(i == top)
        {
            return new Node(Symbol.T, result, new Token("null"));
        }

        if((int)list[i].id == 2 || (int)list[i].id ==6 || (int)list[i].id >=19 && (int)list[i].id <= 20)
        {
            if(result.Count > 1)
            {
                List<Node> newList = new List<Node>();
                newList.Add(new Node(Symbol.T, result, new Token("null")));
                result = newList;
            }

            int newTop = top;
            for(int j = i; j < top; j++)
            {
                if(!((int)list[j].id >= 2 && (int)list[j].id <= 7 || list[j].id == Symbol.X || (int)list[j].id >=19 && (int)list[j].id <= 20))
                {
                    newTop = j;
                    break;
                }
            }
            result.Add(new Node(list[i].id, new List<Node>(), list[i]));
            Node temp = Parse(list, i+1, new List<Node>(), newTop);

            result.Add(temp);
            if(newTop == top)
            {
                return new Node(Symbol.T, result, new Token("null"));
            }
            else
            {
                List<Node> newList2 = new List<Node>();
                newList2.Add(new Node(Symbol.T, result, new Token("null")));
                result = newList2;
                i = newTop;
            }
        }

        if((int)list[i].id >= 10 && (int)list[i].id <= 15)
        {
            result.Add(new Node(list[i].id, new List<Node>(), list[i]));
            Node temp = Parse(list, i+1, new List<Node>(), top);
            
            result.Add(temp);
                
            return new Node(Symbol.T, result, new Token("null"));
        }

        if(list[i].id == Symbol.i || list[i].id == Symbol.X || list[i].id >= Symbol.Mult && list[i].id <= Symbol.Div)
        {
            if(result.Count == 3)
            {
                List<Node> newList = new List<Node>();
                newList.Add(new Node(Symbol.T, result, new Token("null")));
                result = newList;
            }
            result.Add(new Node(list[i].id, new List<Node>(), list[i]));
        }
        
        if(list[i].id == Symbol.If)
        {
            for(int j = i; j < list.Count; j++)
            {
                if(list[j].id == Symbol.Then)
                {
                    Node temp = Parse(list, i+1, new List<Node>(), j);
                    result.Add(temp);
                    temp = Parse(list, j+1, new List<Node>(), list.Count());
                    result.Add(temp);
                    return new Node(Symbol.If, result, new Token("null"));
                }
            }
        }
        if(list[i].id == Symbol.While)
        {
            for(int j = i; j < list.Count; j++)
            {
                if(list[j].id == Symbol.Do)
                {
                    Node temp = Parse(list, i+1, new List<Node>(), j);
                    result.Add(temp);
                    temp = Parse(list, j+1, new List<Node>(), list.Count());
                    result.Add(temp);
                    return new Node(Symbol.While, result, new Token("null"));
                }
            }
        }
        if(list[i].id == Symbol.Concat)
        {
            List<Node> newList = new List<Node>();

            Node actualResult = new Node(Symbol.T, result, new Token("null"));
            newList.Add(actualResult);
            Node temp = Parse(list, i+1, new List<Node>(), top);
            newList.Add(temp);
            
            return new Node(Symbol.S, newList, new Token("null"));
        }
        
        if(list[i].id == Symbol.Sub)
        {
            if(result.Count == 3)
            {
                List<Node> newList = new List<Node>();
                newList.Add(new Node(Symbol.T, result, new Token("null")));
                result = newList;
            }
            result.Add(new Node(list[i].id, new List<Node>(), list[i]));
            result.Add(new Node(list[i+1].id, new List<Node>(), list[i+1]));
            i ++;
        }

        return Parse(list, i+1, result, top);
    }

    public static Statement Construct(string enter)
    {
        var tokens = Token.Tokenizar(enter);
        var tree = Parse(tokens);

        return tree.GetAST();
    }
    
}