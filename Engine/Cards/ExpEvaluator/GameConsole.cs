using Cards;
using ExpEvaluator;
public class Proelium
{
    public static void Start()
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

            if(options == 3)
            {
                break;
            }

            GameMenu(options);

        }
    }

    static void MainMenu()
    {
        string Tittle = @"
                      
                      
                        ▒███████████       ▒███████████          ▒██████████       ▒█████████████    ▒██               ▒████████████    ▒██           ▒██    ▒█████           ▒█████
                                   ▒██                ▒██     ▒██           ▒██                      ▒██                    ▒██         ▒██           ▒██    ▒██  ▒██       ▒██  ▒██
                        ▒██        ▒██     ▒██        ▒██     ▒██           ▒██    ▒██               ▒██                    ▒██         ▒██           ▒██    ▒██   ▒██     ▒██   ▒██
                        ▒██        ▒██     ▒██        ▒██     ▒██           ▒██    ▒██               ▒██                    ▒██         ▒██           ▒██    ▒██    ▒██   ▒██    ▒██
                        ▒███████████       ▒███████████       ▒██           ▒██    ▒██  ▒████        ▒██                    ▒██         ▒██           ▒██    ▒██     ▒██ ▒██     ▒██
                        ▒██                ▒██     ▒██        ▒██           ▒██    ▒██               ▒██                    ▒██         ▒██           ▒██    ▒██       ▒██       ▒██
                        ▒██                ▒██      ▒██       ▒██           ▒██    ▒██               ▒██                    ▒██         ▒██           ▒██    ▒██                 ▒██
                        ▒██                ▒██       ▒██      ▒██           ▒██    ▒██               ▒██                    ▒██         ▒██           ▒██    ▒██                 ▒██
                        ▒██                ▒██        ▒██        ▒██████████       ▒█████████████    ▒█████████████    ▒████████████       ▒██████████       ▒██                 ▒██
        ";
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(Tittle);

        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        System.Console.WriteLine("\n");
        System.Console.WriteLine("𝟏 PLAY");
        System.Console.WriteLine("𝟐 OPTIONS");
        System.Console.WriteLine("𝟑 EXIT");
    }

    static void OptionsMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine("\n");
        System.Console.WriteLine("𝟏 Manage Cards");
        System.Console.WriteLine("𝟐 Back");
    }

    static void GameMenu(int input)
    {
        if(input == 1)
        {
            while(true)
            {
                Console.Clear();

                ModesMenu();

                System.Console.WriteLine();
                System.Console.Write("> ");
                string? entry = Console.ReadLine();

                if(string.IsNullOrEmpty(entry)) continue;

                int options = int.Parse(entry);

                if(options == 4)
                {
                    break;
                }
                else if(options == 1)
                {
                    GameModes.Player_vs_IA();
                }
                else if(options == 2)
                {
                    GameModes.Player_vs_Player();
                }
                else if(options == 3)
                {
                    GameModes.IA_vs_IA();
                }
            }            

        }
        else if(input == 2)
        {

            while(true)
            {
                Console.Clear();

                OptionsMenu(); 
                System.Console.WriteLine();
                System.Console.Write("> ");
                string? entry = Console.ReadLine();

                if(string.IsNullOrEmpty(entry)) continue;

                int options = int.Parse(entry);

                if(options == 1)
                {
                    Creator.Manager();
                }
                else if(options == 2)
                {
                    break;
                }

            }
        }
    }

    static void ModesMenu()
    {
        System.Console.Clear();
        System.Console.WriteLine("\n");
        System.Console.WriteLine("𝟏 Player vs IA");
        System.Console.WriteLine("𝟐 Player vs Player");
        System.Console.WriteLine("𝟑 IA vs IA");
        System.Console.WriteLine("𝟒 Back");
    }
}