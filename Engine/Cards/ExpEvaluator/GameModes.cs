using ExpEvaluator;
using ProeliumEngine;
using System.Threading;
public class GameModes
{
    public static void Player_vs_IA()
    {
        System.Console.Clear();
        System.Console.WriteLine("PLayer vs IA");

        Table table = new Table(new List<List<Card>>()
        {
            MixCards(Serialize.Deserial()), 
            MixCards(Serialize.Deserial())
        }, new List<Card>(), 1);
        Rules rules = new Rules((table.Decks[0].Count, table.Decks[1].Count), 3, 5, new State());
        Player player1 = new Player("Player1", 0, new List<IStrategy>{ new User(rules, 0)});
        Player player2 = new Player("Player2", 1, new List<IStrategy>{new Greedy(rules, 1)});
        State state = new State(1, new List<int>{1,0}, PhasesEnum.mainPhase,new List<Player>{player1, player2}, new List<List<Card>>{new List<Card>(), new List<Card>()}, new List<List<bool>>{ new List<bool>(), new List<bool>()}, table, 1, new List<float>{3, 3} );
        Game newGame = new Game(rules, new Actions(1), state, 1);

        State temp = new State();
        foreach(State actual in newGame)
        {
            temp = actual;
            PrintGame(actual);
        }

        PrintWinner(temp, rules);
    }

    public static void Player_vs_Player()
    {
        System.Console.Clear();
        System.Console.WriteLine("PLayer vs Player");

        Table table = new Table(new List<List<Card>>()
        {
            MixCards(Serialize.Deserial()), 
            MixCards(Serialize.Deserial())
        }, new List<Card>(), 1);
        Rules rules = new Rules((table.Decks[0].Count, table.Decks[1].Count), 3, 5, new State());
        Player player1 = new Player("Player1", 0, new List<IStrategy>{ new User(rules, 0)});
        Player player2 = new Player("Player2", 1, new List<IStrategy>{new User(rules, 1)});
        State state = new State(1, new List<int>{1,0}, PhasesEnum.mainPhase,new List<Player>{player1, player2}, new List<List<Card>>{new List<Card>(), new List<Card>()}, new List<List<bool>>{ new List<bool>(), new List<bool>()}, table, 1, new List<float>{3, 3} );
        Game newGame = new Game(rules, new Actions(1), state, 1);

        State temp = new State();
        foreach(State actual in newGame)
        {
            temp = actual;
            PrintGame(actual);
        }

        PrintWinner(temp, rules);
    }

    public static void IA_vs_IA()
    {
        System.Console.Clear();
        System.Console.WriteLine("IA vs IA");

        Table table = new Table(new List<List<Card>>()
        {
            MixCards(Serialize.Deserial()), 
            MixCards(Serialize.Deserial())
        }, new List<Card>(), 1);
        Rules rules = new Rules((table.Decks[0].Count, table.Decks[1].Count), 3, 5, new State());
        Player player1 = new Player("Player1", 0, new List<IStrategy>{ new Greedy(rules, 0)});
        Player player2 = new Player("Player2", 1, new List<IStrategy>{new Greedy(rules, 1)});
        State state = new State(1, new List<int>{1,0}, PhasesEnum.mainPhase,new List<Player>{player1, player2}, new List<List<Card>>{new List<Card>(), new List<Card>()}, new List<List<bool>>{ new List<bool>(), new List<bool>()}, table, 1, new List<float>{3, 3} );
        Game newGame = new Game(rules, new Actions(1), state, 1);

        State temp = new State();
        foreach(State actual in newGame)
        {
            temp = actual;
            PrintGame(actual);
        }

        PrintWinner(temp, rules);
        
    }
    
    public static List<Card> MixCards(List<Card> cards)
    {
        List<Card> newCards = new List<Card>();
        Random random = new Random();
        while(cards.Count > 0)
        {
            int index = random.Next(0, cards.Count);
            newCards.Add(cards[index]);
            cards.RemoveAt(index);
        }
        return newCards;
    }

    public static void Clear()
    {
        for(int i = 0; i< 10; i++)
        {
            Console.SetCursorPosition(65,4+i);
            Console.WriteLine("                                                   ");
        }
    }
    public static void PrintWinner(State state,Rules rules)
    {
        var win = rules.GetWinner(state);
        Console.SetCursorPosition(65,4);
        Console.ForegroundColor = ConsoleColor.Green;
        if(win.Count == 1)
        {
            System.Console.WriteLine($"{win[0].Name} ha ganado!!!");
            Thread.Sleep(5000);
        }
        else
        {
            System.Console.WriteLine("Hay empate");

        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(65,5);
        System.Console.WriteLine("Presiona enter para salir");
        var s = Console.ReadLine();

    }


    public static void PrintGame(State state)
    {
        Console.Clear();
        string player1Life ="";
        string player2Life ="";
        for(int i = 1; i<= state.LifePoints[0]; i++)
        {
            player1Life += "❤️";
        }
        for(int i = 1; i<= state.LifePoints[1]; i++)
        {
            player2Life += "❤️";
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(5, 0);
        Console.Write(state.Players[0].Name);
        Console.SetCursorPosition(26, 0);
        Console.Write("Turn");
        Console.SetCursorPosition(34, 0);
        Console.Write("Phase");
        Console.SetCursorPosition(60, 0);
        Console.Write(state.Players[1].Name);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(5, 1);
        Console.Write(player1Life);
        Console.SetCursorPosition(60, 1);
        Console.Write(player2Life);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(26, 1);
        Console.Write(state.GameTurns);
        Console.SetCursorPosition(34, 1);
        Console.Write(state.ActualPhase);

        
        if(state.Table.Decks[0] != null)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            System.Console.Write(" ");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Black;
            System.Console.Write(" ");
        }
            Console.ForegroundColor = ConsoleColor.Black;

        System.Console.Write(" ");
        foreach(var card in state.Table.MagicCardsInvokeds)
        {
            foreach(var s in card)
            {
                if(s != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    System.Console.Write(" ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    System.Console.Write(" ");
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
        for (int i = 0; i < 36; i++)
            {
               for(int j = 0; j < 18; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(i + 18, j + 3);
                    System.Console.WriteLine("█");

                }
            }
        
        DrawMagicCard(state);
        DrawMonstercCard(state);
        DrawDeck(state);
        DrawOther(state);
        
        Console.ForegroundColor = ConsoleColor.White;
        Thread.Sleep(1000);
    }

    public static void DrawMagicCard(State state)
    {
        for(int i = 0; i< state.Table.MagicCardsInvokeds[1].Count; i++)
        {
            int j = i*8;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(j + 26, 3);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 27, 3);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 28, 3);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 29, 3);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 26, 4);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 27, 4);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 28, 4);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 29, 4);
            System.Console.Write("█");
        }

        for(int i = 0; i< state.Table.MagicCardsInvokeds[0].Count; i++)
        {
            int j = i*8;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(j + 26, 19);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 27, 19);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 28, 19);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 29, 19);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 26, 20);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 27, 20);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 28, 20);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 29, 20);
            System.Console.Write("█");
        }

    }
    public static void DrawMonstercCard(State state)
    {
        for(int i = 0; i< state.Table.MonsterCardsInvokeds[1].Count; i++)
        {
            int j = i*8;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(j + 26, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 27, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 28, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 29, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 26, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 27, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 28, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 29, 8);
            System.Console.Write("█");
        }

        for(int i = 0; i< state.Table.MonsterCardsInvokeds[0].Count; i++)
        {
            int j = i*8;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(j + 26, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 27, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 28, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 29, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 26, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 27, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 28, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(j + 29, 16);
            System.Console.Write("█");
        }
    }
    public static void DrawDeck(State state)
    {
        if(state.Table.Decks[1].Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(18, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(19, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(20, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(21, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(18, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(19, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(20, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(21, 8);
            System.Console.Write("█");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(18, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(19, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(20, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(21, 7);
            System.Console.Write("█");

            Console.SetCursorPosition(18, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(19, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(20, 8);
            System.Console.Write("█");

            Console.SetCursorPosition(21, 8);
            System.Console.Write("█");
        }

        if(state.Table.Decks[0].Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(50, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(51, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(52, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(53, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(50, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(51, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(52, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(53, 16);
            System.Console.Write("█");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(50, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(51, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(52, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(53, 15);
            System.Console.Write("█");

            Console.SetCursorPosition(50, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(51, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(52, 16);
            System.Console.Write("█");

            Console.SetCursorPosition(53, 16);
            System.Console.Write("█");
        }
    }

    public static void DrawOther(State state)
    {
        if(state.Table.FieldCard != null)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(22, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(23, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(24, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(25, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(22, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(23, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(24, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(25, 12);
            System.Console.Write("█");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(22, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(23, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(24, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(25, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(22, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(23, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(24, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(25, 12);
            System.Console.Write("█");
        }

        if(state.Table.Cemetery.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(46, 11);
            System.Console.Write($"{state.Table.Cemetery.Count}");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(47, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(48, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(49, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(46, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(47, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(48, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(49, 12);
            System.Console.Write("█");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(46, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(47, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(48, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(49, 11);
            System.Console.Write("█");

            Console.SetCursorPosition(46, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(47, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(48, 12);
            System.Console.Write("█");

            Console.SetCursorPosition(49, 12);
            System.Console.Write("█");
        }
    }

}