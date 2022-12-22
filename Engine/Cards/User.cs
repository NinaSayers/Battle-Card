using ProeliumEngine;

public class User : IStrategy
{
    private Rules rules;
        private int playerID;
        public User(Rules rules, int playerID)
        {
            this.rules = rules;
            this.playerID = playerID;
        }
    public Move Play(State state)
    {
        if(state.ActualPhase == PhasesEnum.mainPhase)
        {
            GameModes.Clear();
            int i = PrintMainPhase();
            if(i == 1)
            {
                var card = GetCardFromHand(state);
                return new Move(ActionsEnum.invoke, new List<Card>{card});

            }
            if(i == 2)
            {
                var card = GetCardFromTable(state, typeof(MagicCard));
                return new Move(ActionsEnum.activateEffect, new List<Card>{card});
            }
            // if(i == 3)
            // {
            //     return new ShowTableState();
            // }
            if(i == 4)
            {
                return new Move(ActionsEnum.endPhase, new List<Card>());
            }
        }
        if(state.ActualPhase == PhasesEnum.battlePhase)
        {
            GameModes.Clear();

            int i = PrintBattlePhase();
            if(i == 1)
            {
                var card = GetCardFromTable(state, typeof(MonsterCard));
                bool directAttack = false;

                var cards = state.Table.MonsterCardsInvokeds;
                for(int j = 0; j < cards.Count; j++)
                {
                    if(j != playerID && state.Table.GetMonsterCardsInvokeds(j).Count == 0)
                    {
                        directAttack = true;
                        break;
                    }
                }

                if(directAttack)
                {
                    return new Move(ActionsEnum.attackLifePoints, new List<Card>{card});
                }
                else
                {
                    for(int j = 0; j < cards.Count; j++)
                    {
                        if(j != playerID)
                        {
                            var targets = cards[j];

                            var objetive = SetObjetive(targets);

                            return new Move(ActionsEnum.attackCard,new List<Card>{card, objetive});
                        }
                    }
                }

            }
            // if(i == 2)
            // {
            //     return new ShowTableState();
            // }
            if(i == 3)
            {
                return new Move(ActionsEnum.endPhase, new List<Card>());

            }
        }
        if(state.ActualPhase == PhasesEnum.endPhase)
        {
            GameModes.Clear();
            int i = PrintEndPhase();
            if(i == 1)
            // {
            //     return new SummonMonster();
            // }
            if(i == 2)
            {
                var card = GetCardFromTable(state, typeof(MagicCard));
                return new Move(ActionsEnum.activateEffect, new List<Card>{card});
            }
            // if(i == 3)
            // {
            //     return new ShowTableState();
            // }
            // if(i == 4)
            {
                return new Move(ActionsEnum.endPhase, new List<Card>());
            }
        }
        return new Move(ActionsEnum.endTurn, new List<Card>());
    }

    private Card SetObjetive(List<Card> targets)
    {
        GameModes.Clear();
        int i = 0;
        foreach(var Card in targets)
        {
            Console.SetCursorPosition(65,4+i);
            System.Console.WriteLine(i+1+" Name: "+(Card as MonsterCard)!.Name+" Life: " +(Card as MonsterCard)!.Attack +" Life: "+(Card as MonsterCard)!.Life +" Defense: "+(Card as MonsterCard)!.Defense);
            i++;
        }
        Console.SetCursorPosition(65,i+5);
        Console.Write("> ");
        int j = int.Parse(Console.ReadLine());
        return targets[j-1];
    }

    private int PrintMainPhase()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("1 Invoke");
        Console.SetCursorPosition(65,5);
        Console.WriteLine("2 Activate a spell effect");
        Console.SetCursorPosition(65,6);
        Console.WriteLine("3 Show table state");
        Console.SetCursorPosition(65,7);
        Console.WriteLine("4 End phase");
        Console.SetCursorPosition(65,8);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return i;
    }
    private int PrintBattlePhase()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("1 Attack");
        Console.SetCursorPosition(65,5);
        Console.WriteLine("2 Show table state");
        Console.SetCursorPosition(65,6);
        Console.WriteLine("3 End phase");
        Console.SetCursorPosition(65,7);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return i;

    }
    private int PrintEndPhase()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("1 Summon");
        Console.SetCursorPosition(65,5);
        Console.WriteLine("2 Activate an effect");
        Console.SetCursorPosition(65,6);
        Console.WriteLine("3 Show table state");
        Console.SetCursorPosition(65,7);
        Console.WriteLine("4 End turn");
        Console.SetCursorPosition(65,8);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return i;
        
    }

    

    private Card GetCardFromHand(State state)
    {
        GameModes.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("Choose a card from your hand");
        Console.SetCursorPosition(65,5);
        int j = 0;
        var hand = state.GetHand(playerID);
        GameModes.Clear();
        foreach(var card in hand)
        {
            if(card is MonsterCard)
            {
                Console.SetCursorPosition(65,j+4);
                Console.WriteLine(j+1 + " Monster : " + (card as MonsterCard)!.Name);
                j++;
            }
            if(card is MagicCard)
            {
                Console.SetCursorPosition(65,4+j);
                Console.WriteLine(j+1 + " Magic : " + (card as MagicCard)!.Name);
                j++;
            }
        }
        Console.SetCursorPosition(65,j +5);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return hand[i-1];
    }

    private Card GetCardFromTable(State state, Type type)
    {
        GameModes.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(65,4);
        Console.WriteLine("Choose a card from your Table");
        Console.SetCursorPosition(65,5);
        int j = 0;
        List<Card> cards = new List<Card>();
        if(type.Name == "MonsterCard")
        {
          cards = state.Table.GetMonsterCardsInvokeds(playerID);   
        }
        else if(type.Name == "MagicCard")
        {
            cards = state.Table.GetMagicCardsInvokeds(playerID);
        }

        GameModes.Clear();
        foreach(var card in cards)
        {
            if(card.GetType() == type)
            {
                Console.SetCursorPosition(65,4+j);
                string name = (card is MonsterCard) ? (card as MonsterCard)!.Name : (card as MagicCard)!.Name;
                Console.WriteLine(j+1 + " " + name);
                j++;
            }
        }
        Console.SetCursorPosition(65,j+5);
        Console.Write("> ");
        int i = int.Parse(Console.ReadLine());
        return cards[i-1];
    }
}