using BattleCardsEngine;
public class Actions /*: IAction*/ 
{
    private State state;
    private int gameID;

    public Actions(State state, int gameID)
    {
        this.state = state;
        this.gameID = gameID;
    }
    public State Draw(int playerID, int gameID)
    {
        MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede robar una carta del mazo.");
        MyExceptions.InvalidPlayerIDException(playerID, this.state.Table.Decks.Count - 1);
        MyExceptions.EmptyDeckException(this.state.Table.GetDeck(playerID));
        int countDeck = this.state.Table.GetDeck(playerID).Count;
        Card newcard = this.state.Table.GetDeck(playerID)[countDeck - 1];
        this.state.AddCardsToHand(this.gameID, playerID, newcard);
        this.state.Table.RemoveCardToDeck(playerID, gameID, newcard);
        return this.state;
    }
    public State Mix(int playerID, int gameID)
    {
        MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede barajar el Deck.");
        MyExceptions.InvalidPlayerIDException(playerID, this.state.Table.Decks.Count - 1);
        MyExceptions.EmptyDeckException(this.state.Table.GetDeck(playerID));

        List<Card> oldDeck = new List<Card>(this.state.Table.GetDeck(playerID).Count);
        oldDeck = this.state.Table.GetDeck(playerID);
        List<Card> newDeck = new List<Card>(oldDeck.Count);
        Random r = new Random();
        for (int i = 0; i < newDeck.Count; i++)
        {
            int n = r.Next(oldDeck.Count);
            newDeck[i] = oldDeck[n - 1];
            oldDeck.RemoveAt(n - 1);
        }
        this.state.Table.SetDeck(playerID, gameID, newDeck);
        return this.state;
    }

    public State Invoke(Card card, int playerID, int gameID)
    {
        MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID inconrrecto. No puede invocar cartas.");
        MyExceptions.InvalidPlayerIDException(playerID, this.state.Table.Decks.Count - 1);
        if (card is MonsterCard)
        {
            int actualCount = this.state.Table.GetMonsterCardsInvokeds(playerID).Count;
            int maximumCount = this.state.Table.MaximumCardsInvokeds;
            MyExceptions.FullInvokedCardsException(actualCount, maximumCount, "Máximo de cartas alcanzado. No se pueden invocar más cartas de monstruo.");
            MyExceptions.NoFoundedCardException(this.state.GetHand(playerID), card, "No se pueden invocar cartas que no estén en la mano del jugador.");
            this.state.RemoveCardsToHand(this.gameID, playerID, card);
            this.state.Table.SetMonsterCard(playerID, this.gameID, card as BattleCardsEngine.MonsterCard);
        }
        if (card is MagicCard)
        {
            int actualCount = this.state.Table.GetMagicCardsInvokeds(playerID).Count;
            int maximumCount = this.state.Table.MaximumCardsInvokeds;
            MyExceptions.FullInvokedCardsException(actualCount, maximumCount, "Máximo de cartas alcanzado. No se pueden invocar más cartas mágicas.");
            MyExceptions.NoFoundedCardException(this.state.GetHand(playerID), card, "No se pueden invocar cartas que no estén en la mano del jugador.");
            this.state.RemoveCardsToHand(this.gameID, playerID, card);
            this.state.Table.SetMagicCard(playerID, this.gameID, card as MagicCard);
        }
        if (card is FieldCard)
        {
            MyExceptions.NoFoundedCardException(this.state.GetHand(playerID), card, "No se pueden invocar cartas que no estén en la mano del jugador.");
            this.state.Table.SetFieldCard(this.gameID, card as BattleCardsEngine.FieldCard);
        }
        return this.state;
    }
    public State AttackCard(int gameID, int attackantPlayerID, BattleCardsEngine.MonsterCard cardAtacante, BattleCardsEngine.MonsterCard cardAtacada)
    {
        MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No tiene permitido realizar ataques.");
        MyExceptions.InvalidPlayerIDException(attackantPlayerID, this.state.Table.Decks.Count - 1);
        // MyExceptions.InvalidTypOfCardException(cardAtacada.Type, TypeOfCard.monster, "Solo pueden ser atacadas las  cartas de tipo monstruo.");
        // MyExceptions.InvalidTypOfCardException(cardAtacante.Type, TypeOfCard.monster, "Solo pueden atacar las cartas de tipo monstruo.");
        MyExceptions.InvalidAttackantCardException(this.state.Table.GetMonsterCardsInvokeds(attackantPlayerID), cardAtacante);
        bool yaAtacó = false;

        foreach (List<Card> invokedMonsterCards in this.state.Table.MonsterCardsInvokeds)
        {
            if (this.state.Table.MonsterCardsInvokeds.IndexOf(invokedMonsterCards) != attackantPlayerID)
            {
                MyExceptions.NoFoundedCardException(invokedMonsterCards, cardAtacada, "Esta carta no está invocada por el jugador contrario.");
                foreach (BattleCardsEngine.MonsterCard monsterCard in invokedMonsterCards)
                {
                    if ((monsterCard == cardAtacada) && (!yaAtacó))
                    {
                        yaAtacó = true;
                        if (monsterCard.Defense <= 0)
                        {
                            this.state.Table.AddCardToCemetery(this.gameID, monsterCard);
                            invokedMonsterCards.Remove(monsterCard);
                        }
                        else
                        {
                            float damage = cardAtacante.Attack * 1000 / monsterCard.Defense;
                            monsterCard.SetLife(this.gameID, monsterCard.Life - damage);
                            if (monsterCard.Life <= 0)
                            {
                                this.state.Table.AddCardToCemetery(this.gameID, monsterCard);
                                invokedMonsterCards.Remove(monsterCard);
                            }
                        }
                    }
                }
            }
        }
        return this.state;
    }

    public State AttackLifePoints(int gameID, int playerID, BattleCardsEngine.MonsterCard attackantCard)
    {
        MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede atacar directamente la vida del jugador contrario");
        MyExceptions.InvalidPlayerIDException(playerID, this.state.Hands.Count - 1);
        MyExceptions.InvalidAttackantCardException(this.state.Table.MonsterCardsInvokeds[playerID], attackantCard);
        this.state.SetLifePoints(gameID, playerID, this.state.GetLifePoints(playerID) - attackantCard.Attack);
        return this.state;
    }

    public State Discard(int gameID, int playerID, Card card) //descartar cartas de la mano y enviarlas al mazo del jugador
    {
        MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede descartar cartas de la mano del jugador.");
        MyExceptions.InvalidPlayerIDException(playerID, this.state.Hands.Count - 1);
        MyExceptions.NoFoundedCardException(this.state.Hands[playerID], card, "No puede descartar una carta que no esté en la mano del jugador.");
        this.state.RemoveCardsToHand(this.gameID, playerID, card);
        return this.state;
    }
}
