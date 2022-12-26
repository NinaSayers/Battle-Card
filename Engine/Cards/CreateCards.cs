using System.Text.Json;
using System.Text.Json.Serialization;
using ExpEvaluator;
using Cards;


public class Creator
{
    public static void Manager()
    {
        while(true)
        {
            Console.Clear();

            MainMenu();
            System.Console.WriteLine();
            System.Console.Write("> ");
            string? input = Console.ReadLine();

            if(string.IsNullOrEmpty(input)) continue;

            int options = int.Parse(input);

            if(options == 4)
            {
                break;
            }

            CardCreator(options);
        }
    }

    static void MainMenu()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("Select an opcion\n");
        System.Console.WriteLine("𝟏 Create a Spell Card");
        System.Console.WriteLine("𝟐 Create a Monster Card");
        System.Console.WriteLine("𝟑 Search Card");
        System.Console.WriteLine("𝟒 Back");
    }

    static void CardsMenu()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("Select a Card Type\n");
        System.Console.WriteLine("𝟏 Spell Card");
        System.Console.WriteLine("𝟐 Monster Card");
        System.Console.WriteLine("𝟑 Back");
    }



    static void CardCreator(int entry)
    {
        if(entry == 1)
        {
            Console.Clear();

            System.Console.WriteLine();
            System.Console.Write("Enter the card name : ");
            string? cardName = Console.ReadLine();

            System.Console.WriteLine();
            System.Console.Write("Enter the card description : ");
            string? description = Console.ReadLine();
            
            Effect:
            System.Console.WriteLine();
            System.Console.Write("Enter the card effect : ");
            string? effect = Console.ReadLine();

            try
            {
                Parser.Construct(effect);
            }
            catch
            {
                System.Console.WriteLine("The effect given is incorrect, please try again : ");
                goto Effect;
            }

            var card = new SpellCardInfo
            {
                CardName = cardName,
                strEffect = effect
            };

            string filename = $"./DataBase/CardsInfo/Spell/{cardName}.json";
            string jsonString = JsonSerializer.Serialize(card);
            File.WriteAllText(filename, jsonString);
        }
        if(entry == 2)
        {
            Console.Clear();

            System.Console.WriteLine();
            System.Console.Write("Enter the card name : ");
            string? cardName = Console.ReadLine();

            System.Console.WriteLine();
            System.Console.Write("Enter the card attack : ");
            int attack = int.Parse(Console.ReadLine()??"0");

            System.Console.WriteLine();
            System.Console.Write("Enter the card defense : ");
            int defense = int.Parse(Console.ReadLine()??"0");

            System.Console.WriteLine();
            System.Console.Write("Enter the card life : ");
            int health = int.Parse(Console.ReadLine()??"0");

            // System.Console.WriteLine();
            // System.Console.Write("Enter the card description : ");
            // string? description = Console.ReadLine();
            
            Effect:
            System.Console.WriteLine();
            System.Console.Write("Enter the card effect : ");
            string? effect = Console.ReadLine();

            try
            {
                Parser.Construct(effect);
            }
            catch
            {
                System.Console.WriteLine("The effect given is incorrect, please try again : ");
                goto Effect;
            }

            var card = new MonsterCardInfo{
                CardName = cardName,
                Attack = attack,
                Defense = defense,
                Health = health,
                strEffect = effect
            };

            string filename = $"./DataBase/CardsInfo/Monster/{cardName}.json";
            string jsonString = JsonSerializer.Serialize(card);
            File.WriteAllText(filename, jsonString);
        }
        else if(entry == 3)
        {
            while(true)
            {
                Console.Clear();

                CardsMenu();
                System.Console.WriteLine();
                System.Console.Write("> ");
                string? input = Console.ReadLine();

                if(string.IsNullOrEmpty(input)) continue;

                int options = int.Parse(input);

                if(options == 3)
                {
                    break;
                }

                CardViewer(options);
            }
        }

        System.Console.WriteLine("Is not a valid option");
    }

    static void CardViewer(int entry)
    {
        if(entry == 1)
        {
            DirectoryInfo di = new DirectoryInfo(@"./DataBase/CardsInfo/Spell");
            FileInfo[] files = di.GetFiles("*.json", SearchOption.AllDirectories);
            string [] path = new string[files.Length];
            for(int i = 0 ; i< files.Length; i++)
            {
                path[i] = Path.GetFileNameWithoutExtension(files[i].ToString());
            }

            int j = 1;

            foreach(var str in path)
            {
                System.Console.WriteLine($"{j++} {str}");
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Press a key to go back");
            Console.ReadLine();

        }
        else if(entry == 2)
        {
            DirectoryInfo di = new DirectoryInfo(@"./DataBase/CardsInfo/Monster");
            FileInfo[] files = di.GetFiles("*.json", SearchOption.AllDirectories);
            string [] path = new string[files.Length];
            for(int i = 0 ; i< files.Length; i++)
            {
                path[i] = Path.GetFileNameWithoutExtension(files[i].ToString());
            }

            int j = 1;

            foreach(var str in path)
            {
                System.Console.WriteLine($"{j++} {str}");
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Press a key to go back");
            Console.ReadLine();
        }

        System.Console.WriteLine("Is not a valid option");
    }


}