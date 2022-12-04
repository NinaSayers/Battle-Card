using ExpEvaluator;
using Cards;

public class Program
{
    public static Dictionary<string, double> variables = new Dictionary<string, double>()
    {
        {"Player1.life",200},
        {"Player2.life",200},
        {"x",0}

    };
    public static  void Main()
    {
        Creator.Manager();
    }
}