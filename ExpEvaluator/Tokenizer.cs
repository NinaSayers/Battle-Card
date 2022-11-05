namespace ExpEvaluator;

public class Token
{
    string token;
    public Token(string token)
    {
        this.token = token;
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
            if(IsValidToken(temp))
            {
                cond = true;
                {
                    if(i == text.Length-1)
                    {
                        list.Add(new Token(temp));
                    }
                }
            }
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
        }
        return list;
    }
    public static bool IsValidToken(string text)
    {
        bool cond = true;
        string [] tokens = new string [] {"si","aumenta","disminuye","ataque","defensa"};
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
    public void Print()
    {
        System.Console.WriteLine(this.token);
    }
}