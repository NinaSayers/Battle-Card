  using ProeliumEngine;
  public class Table
    {
        private List<List<Card>> magicCardsInvokeds;
        private List<List<Card>> monsterCardsInvokeds;
        private List<List<Card>> decks;
        private List<Card> principalDeck;
        private List<Card> cemetery;
        private Card? fieldCard;
        private int gameID;
        private int maximumCardsInvokeds;

        public Table(List<List<Card>> decks, List<Card> principalDeck, int gameID, int maximumCardsInvokeds = 3)
        {
            this.magicCardsInvokeds = new List<List<Card>> { new List<Card>(3), new List<Card>(3) };
            this.monsterCardsInvokeds = new List<List<Card>> { new List<Card>(3), new List<Card>(3) };
            this.decks = decks;
            this.principalDeck = principalDeck;
            this.cemetery = new List<Card>();
            this.gameID = gameID;
            this.maximumCardsInvokeds = maximumCardsInvokeds;
        }

        public Table(List<List<Card>> magicCardsInvokeds, List<List<Card>> monsterCardsInvokeds, List<List<Card>> decks, List<Card> principalDeck, List<Card> cemetery, Card fieldCard, int gameID, int maximumCardsInvokeds)
        {
            this.magicCardsInvokeds = magicCardsInvokeds;
            this.monsterCardsInvokeds = monsterCardsInvokeds;
            this.decks = decks;
            this.principalDeck = principalDeck;
            this.cemetery = cemetery;
            this.fieldCard = fieldCard;
            this.gameID = gameID;
            this.maximumCardsInvokeds = maximumCardsInvokeds;
        }
        public List<List<Card>> MagicCardsInvokeds { get { return this.magicCardsInvokeds; } }
        public List<List<Card>> MonsterCardsInvokeds { get { return this.monsterCardsInvokeds; } }
        public List<List<Card>> Decks { get { return this.decks; } }
        public List<Card> Cemetery { get { return this.cemetery; } }
        public Card FieldCard { get { return this.fieldCard; } }
        public int MaximumCardsInvokeds { get { return this.maximumCardsInvokeds; } }
        public List<Card> GetMagicCardsInvokeds(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            return this.magicCardsInvokeds[playerID];
        }
        public void AddMagicCard(int playerID, int gameID, MagicCard magicCard)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede invocar una carta mágica.");
            MyExceptions.FullInvokedCardsException(this.magicCardsInvokeds[playerID].Count, this.maximumCardsInvokeds, "Máximo de cartas alcanzado. No se pueden invocar más cartas mágicas");
            this.magicCardsInvokeds[playerID].Add(magicCard);
            return;
        }
        public void RemoveMagicCard(int playerID, int gameID, MagicCard magicCard)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede retirar esta carta mágica del campo.");
            MyExceptions.NoFoundedCardException(this.GetMagicCardsInvokeds(playerID), magicCard, "Esta carta mágica no está invocada, luego no puede retirarla del campo.");
            this.magicCardsInvokeds[playerID].Remove(magicCard);
            return;
        }
        public List<Card> GetMonsterCardsInvokeds(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            return this.monsterCardsInvokeds[playerID];
        }
        public void AddMonsterCard(int playerID, int gameID, MonsterCard monsterCard)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede invocar un monstruo.");
            MyExceptions.FullInvokedCardsException(this.monsterCardsInvokeds[playerID].Count, this.maximumCardsInvokeds, "Máximo de cartas alcanzado. No se pueden invocar más cartas de monstruo.");
            this.monsterCardsInvokeds[playerID].Add(monsterCard);
            return;
        }
        public void RemoveMonsterCard(int playerID, int gameID, MonsterCard monstercard)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede retirar esta carta monstruo del campo.");
            MyExceptions.NoFoundedCardException(this.GetMonsterCardsInvokeds(playerID), monstercard, "Esta carta monstruo no está invocada, luego no puede retirarla del campo.");
            this.MonsterCardsInvokeds[playerID].Remove(monstercard);
            return;
        }
        public List<Card> GetDeck(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            return this.decks[playerID];
        }
        public void SetDeck(int playerID, int gameID, List<Card> newDeck)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar las cartas del mazo.");
            this.decks[playerID] = newDeck;
            return;
        }
        public void AddCardToDeck(int playerID, int gameID, List<Card> newCards)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede agregar cartas en el mazo.");
            for (int i = 0; i < newCards.Count; i++)
            {
                this.decks[playerID].Add(newCards[i]);
            }
            return;
        }
        public void RemoveCardToDeck(int playerID, int gameID, Card oldCard)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede remover cartas del mazo.");
            MyExceptions.NoFoundedCardException(this.decks[playerID], oldCard, "Esta carta no se encuentra en el deck.");
            this.decks[playerID].Remove(oldCard);
            return;
        }
        public void AddCardToCemetery(int gameID, Card card)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede enviar esta carta al cementerio.");
            this.cemetery.Add(card);
            return;
        }
        public void SetFieldCard(int gameID, FieldCard newFieldCard)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede invocar esta carta de campo.");
            Card oldCard = this.fieldCard;
            this.fieldCard = newFieldCard;
            cemetery.Add(oldCard);
            return;
        }
    }
   