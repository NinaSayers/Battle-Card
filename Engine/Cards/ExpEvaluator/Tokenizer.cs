namespace ExpEvaluator;

public class Token
{
    public string Value {get; private set;}
    public Symbol id {get; private set;}
    public Token(string Value)
    {
        this.Value = Value;
        id = Identify(Value);
        if(!IsNumber() && id == Symbol.i)
        {  
            id = Symbol.X;
        }
    }

    public static List<Token> Tokenizar(string text)
    {
        string temp = "";
        bool cond = false;
        List<Token> list = new List<Token>();
        for(int i = 0; i < text.Length; i ++)
        {
            string memory = temp;
            temp += text[i];
            if(!IsValidToken(temp) && cond)
            {
                cond = false;
                list.Add(new Token(memory));
                temp = "";
                temp = temp + text[i];
            }
            if(text[i] + "" == " ")
            {
                temp = "";
            }
            if(IsValidToken(temp) && !string.IsNullOrWhiteSpace(temp))
            {
                cond = true;
                {
                    if(i == text.Length-1)
                    {
                        list.Add(new Token(temp));
                    }
                }
            }
        }
        return list;
    }
    private Symbol Identify(string value)
    {
        if(value == "plus")
        {
            return Symbol.Add;
        }
        if(value == "if")
        {
            return Symbol.If;
        }
        if(value == "then")
        {
            return Symbol.Then;
        }
        if(value == "minus")
        {
            return Symbol.Sub;
        }
        if(value == "mult")
        {
            return Symbol.Mult;
        }
        if(value == "div")
        {
            return Symbol.Div;
        }
        if(value == "idem")
        {
            return Symbol.Asg;
        }
        if(value == "maior")
        {
            return Symbol.Mayor;
        }
        if(value == "minor")
        {
            return Symbol.Menor;
        }
        if(value == "idemI")
        {
            return Symbol.Igual;
        }
        if(value == "maiorI")
        {
            return Symbol.MayorI;
        }
        if(value == "minorI")
        {
            return Symbol.MenorI;
        }
        if(value == "dist")
        {
            return Symbol.Dist;
        }
        if(value == "creace")
        {
            return Symbol.Creace;
        }
        if(value == "decreace")
        {
            return Symbol.Decreace;
        }
        if(value == "(")
        {
            return Symbol.Open;
        }
        if(value == ")")
        {
            return Symbol.Close;
        }
        if(value == "while")
        {
            return Symbol.While;
        }
        if(value == "do")
        {
            return Symbol.Do;
        }
        if(value == "draw")
        {
            return Symbol.Draw;
        }
        if(value == "mix")
        {
            return Symbol.Mix;
        }
        if(value == ",")
        {
            return Symbol.Concat;
        }
        if(value == ";")
        {
            return Symbol.EOI;
        }
        if(value == "null")
        {
            return Symbol.Invalid;
        }

        return Symbol.i;
    }
    public static bool IsValidToken(string text)
    {
        bool cond = true;
        string [] tokens = new string []
        {
            "if","then","idem","plus","minus","mult","div","maior","minor","idemI","dist","minorI","maiorI","(",")"
            ,"Player1.life","Player2.life","PlayerID","this.Attack","this.Defense","this.Life"
            ,"creace","decreace","while","do","draw","mix",
            ",",";"
        };
        foreach(string st in tokens)
        {
            if(st == text)
            {
                return true;
            }
        }
        foreach(char ch in text)
        {
            if(!IsNumber(ch))
            {
                cond = false;
            }
        }
        return cond;
    }

    private static bool IsNumber(char ch)
    {
        char [] array = new char [] {'0','1','2','3','4','5','6','7','8','9'};
        foreach(char n in array)
        {
            if(ch == n || ch == '-')
            {
                return true;
            }
        }
        return false;
    }
    private bool IsNumber()
    {
        string s = this.Value ;
        foreach (char item in s)
        {
            if(!IsNumber(item))
            {
                return false;
            }
        }
        return true;
    }
    
    public static double GetNumber(Token token)
    {
        if(token.IsNumber())
        {
            return Double.Parse(token.Value);
        }
        return 0;
    }
    public void Print()
    {
        System.Console.WriteLine(this.Value);
    }
}