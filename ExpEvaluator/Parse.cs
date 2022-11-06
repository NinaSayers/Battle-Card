namespace ExpEvaluator;

public class Parser
{
    public static Node Parse(List<Token> list)
    {
        return Parse(list, 0, new List<Node>());
    }
    private static Node Parse(List<Token> list, int i, List<Node> result)
    {
        if(i == list.Count())
        {
            return new Node(Symbol.T, result, new Token("null"));
        }
        if((int)list[i].id >= 2 && (int)list[i].id < 6)
        {
            
            result.Add(new Node(list[i].id, new List<Node>(), list[i]));
            Node temp = Parse(list, i+1, new List<Node>());
            if(temp.children.Count() > 1)
            {
                result.Add(temp);
                return new Node(Symbol.T, result, new Token("null"));
            }
        }
        else
        {
            result.Add(new Node(list[i].id, new List<Node>(), list[i]));
        }
        

        return Parse(list, i+1, result);
    }
}