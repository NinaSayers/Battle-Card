using System.Text.Json;
using System.Text.Json.Serialization;
using Cards;
namespace ExpEvaluator;

public class Serialize
{
    public static List<Card> Deserial()
    {
        List<Card> cards = new List<Card>();

        DirectoryInfo di = new DirectoryInfo(@"./DataBase/CardsInfo/Spell");
        FileInfo[] files = di.GetFiles("*.json", SearchOption.AllDirectories);
        string [] path = new string[files.Length];
        for(int i =0 ; i< files.Length; i++)
        {
            path[i] = files[i].ToString();
            string text = File.ReadAllText(path[i]);
            var card = JsonSerializer.Deserialize<SpellCard>(text);
            cards.Add(card);
        }

        di = new DirectoryInfo(@"./DataBase/CardsInfo/Monster");
        files = di.GetFiles("*.json", SearchOption.AllDirectories);
        path = new string[files.Length];
        for(int i =0 ; i< files.Length; i++)
        {
            path[i] = files[i].ToString();
            string text = File.ReadAllText(path[i]);
            var card = JsonSerializer.Deserialize<SpellCard>(text);
            cards.Add(card);
        }
        return cards;
    }
}