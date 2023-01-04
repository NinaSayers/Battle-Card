using System.Text.Json;
using System.Text.Json.Serialization;
using Cards;
using ProeliumEngine;
namespace ExpEvaluator;

public class Serialize
{
    public static List<Card> Deserial()
    {
        List<Card> cards = new List<Card>();
        System.Console.WriteLine(Directory.GetCurrentDirectory());
        DirectoryInfo di = new DirectoryInfo(@"./DataBase/CardsInfo/Spell");
        FileInfo[] files = di.GetFiles("*.json", SearchOption.AllDirectories);
        string [] path = new string[files.Length];
        for(int i =0 ; i< files.Length; i++)
        {
            path[i] = files[i].ToString();
            string text = File.ReadAllText(path[i]);
            var cardInfo = JsonSerializer.Deserialize<SpellCardInfo>(text);
            var card = CreateCard(cardInfo);
            cards.Add(card);
        }

        di = new DirectoryInfo(@"./DataBase/CardsInfo/Monster");
        files = di.GetFiles("*.json", SearchOption.AllDirectories);
        path = new string[files.Length];
        for(int i =0 ; i< files.Length; i++)
        {
            path[i] = files[i].ToString();
            string text = File.ReadAllText(path[i]);
            var cardInfo = JsonSerializer.Deserialize<MonsterCardInfo>(text);
            var card = CreateCard(cardInfo);
            cards.Add(card);
        }
        return cards;
    }

    public static Card CreateCard(CardInfo card)
    {
        if(card is MonsterCardInfo)
        {
            return CreateMonsterCard(card as MonsterCardInfo);
        }
        if(card is SpellCardInfo)
        {
            return CreateMagicCard(card as SpellCardInfo);
        }
        return null;
    }

    private static MonsterCard CreateMonsterCard(MonsterCardInfo card)
    {
        return new MonsterCard(card.CardName, card.Health, card.Attack, card.Defense, card.strEffect, card.Description);
    }

    private static MagicCard CreateMagicCard(SpellCardInfo card)
    {
        return new MagicCard(card.CardName, card.strEffect, card.Description);
    }
}