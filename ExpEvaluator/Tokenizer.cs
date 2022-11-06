namespace ExpEvaluator;

public class Token
{
    public string Value {get; private set;}
    public Symbol id {get; private set;}
    public Token(string Value)
    {
        this.Value = Value;
        id = Identify(Value);
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
        if(value == "aumenta")
        {
            return Symbol.Add;
        }
        if(value == "disminuye")
        {
            return Symbol.Sub;
        }
        if(value == "multiplica")
        {
            return Symbol.Mult;
        }
        if(value == "divid")
        {
            return Symbol.Div;
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
        string [] tokens = new string [] {"si","entonces",".","aumenta","disminuye","multiplica","divid","vida","ataque","defensa"};
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
            if(ch == n)
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