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

        if((int)list[i].id >= 2 && (int)list[i].id <= 6)
        {
            int newTop = 0;
            newTop = top;
            for(int j = i; j < top; j++)
            {
                if(!((int)list[j].id >=2 && (int)list[j].id <= 7))
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
                List<Node> newList = new List<Node>();
                newList.Add(new Node(Symbol.T, result, new Token("null")));
                result = newList;
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

        if(list[i].id == Symbol.i)
        {
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

        return Parse(list, i+1, result, top);
    }
}