using System.Collections;

namespace BattleCardsEngine
{
    //PLANIFICACION***!!!!!! (esto es para el análisis de cómo pensar la "organización" de las funcionalidades del programa) ESTADO
    //MAQUINA DE ESTADO FINITA***!!!! (esto es para la secuencia de los turnos) JUEGO
    //IMPLEMENTAR iSTRATEGY (son las estrategias de juego que pueden implementar los jugadores/ son las que intervienen directamente con las decisioness a tomar respecto a las jugadas)
    //ACCEDER A LA INFO DEL ESTADO MEDIANTE METODOS que este tendrá y que, dadas infos específicas (identificadores en caso de acceder a las manos de los jugadores, por ej), dé la info qeu se le pida
    #region Game
    //****AQUí VAN LAS POSIBLES ACCIONES A REALIZAR EN EL JUEGO (modifican el estado)
    public class Game : IEnumerable<State>
    {
        private Rules rules;
        private State state;
        private Actions actions;
        int gameID;
        private List<PhasesEnum> phases = new List<PhasesEnum> { PhasesEnum.drawPhase, PhasesEnum.mainPhase, PhasesEnum.battlePhase, PhasesEnum.endPhase};
        public Game(Rules rules, Actions actions, int gameID)
        {
            this.rules = rules;
            this.state = actions.State_;
            this.actions = actions;
            this.gameID = gameID;
        }
        public IEnumerator<State> GetEnumerator()
        {
            while (!this.rules.IsEndGame(this.actions.State_))
            {
                foreach (Player player in this.actions.State_.Players)
                {
                    foreach (PhasesEnum phase in this.phases)
                    {
                        Move jugada = player.Move_;
                        if (this.rules.IsValidMove(jugada.Action, phase, player, this.actions.State_))
                        {
                            ActionToDo(player, jugada); //Falta considerar los ciclos de ataques (atacar varias veces)
                            yield return this.actions.State_;
                            if (this.rules.IsEndGame(this.actions.State_)) break;
                        }
                    }
                    yield return this.actions.State_;
                    if (this.rules.IsEndGame(this.actions.State_)) break;
                    this.actions.State_.SetTurnsByPlayer(this.gameID, player.ID, this.actions.State_.TurnsByPlayer[player.ID] + 1);
                }
                if (this.rules.IsEndGame(this.actions.State_)) break;
                this.actions.State_.SetGameTurns(this.gameID, this.state.GameTurns + 1);
                yield return this.actions.State_;
            }
            this.rules.GetWinner(this.actions.State_);  //Este método dice el jugador que ganó y el que perdió (en caso de empate devuelve 1 solo jugador, si no retorna la lista vacía)
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ActionToDo(Player player, Move jugada) //Realiza la acción en el juego de acuerdo a la entrada del jugador
        {
            switch (jugada.Action)
            {
                case ActionsEnum.draw:
                    this.state = this.actions.Draw(player.ID, this.gameID);
                    break;
                case ActionsEnum.mix:
                    this.state = this.actions.Mix(player.ID, this.gameID);
                    break;
                case ActionsEnum.invoke:
                    this.state = this.actions.Invoke(jugada.CardsInTheAction[0], player.ID, this.gameID);
                    break;
                case ActionsEnum.attackCard:
                    this.state = this.actions.AttackCard(this.gameID, player.ID, jugada.CardsInTheAction[0] as MonsterCard, jugada.CardsInTheAction[1] as MonsterCard);
                    break;
                case ActionsEnum.attackLifePoints:
                    this.state = this.actions.AttackLifePoints(this.gameID, player.ID, jugada.CardsInTheAction[0] as MonsterCard);
                    break;
                case ActionsEnum.discard:
                    this.state = this.actions.Discard(this.gameID, player.ID, jugada.CardsInTheAction[0]);
                    break;
            }
            return;
        }
    }

    public class Actions /*: IAction*/
    {
        private State state;
        private int gameID;
        public Actions(State state, int gameID)
        {
            this.state = state;
            this.gameID = gameID;
        }
        public State State_ { get { return this.state; } private set {; } }
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
                this.state.Table.SetMonsterCard(playerID, this.gameID, card as MonsterCard);
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
                this.state.Table.SetFieldCard(this.gameID, card as FieldCard);
            }
            return this.state;
        }
        public State AttackCard(int gameID, int attackantPlayerID, MonsterCard cardAtacante, MonsterCard cardAtacada)
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
                    foreach (MonsterCard monsterCard in invokedMonsterCards)
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
        public State AttackLifePoints(int gameID, int playerID, MonsterCard attackantCard)
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
    public struct State
    {
        private int gameTurns; //turnos totales del juego
        private List<int> turnsByPlayer;
        private List<Player> players;
        private List<List<Card>> hands; //cartas en mano de los jugadores
        private List<float> lifePoints; //vidas de los jugadores 
        private Table table;
        private int gameID;

        public State(int gameTurns, List<int> turnsByPlayer, List<Player> players, List<List<Card>> hands, Table table, int gameID, List<float> lifePoints)
        {
            this.gameTurns = gameTurns;
            this.turnsByPlayer = turnsByPlayer;
            this.players = players;
            this.hands = hands;
            this.table = table;
            this.gameID = gameID;
            this.lifePoints = lifePoints;
        }
        public int GameTurns { get { return this.gameTurns; } private set {; } }
        public List<int> TurnsByPlayer { get { return this.turnsByPlayer; } private set {; } }
        public List<Player> Players { get { return this.players; } private set {; } }
        public List<List<Card>> Hands { get { return this.hands; } private set {; } }
        public Card FieldCard { get { return this.table.FieldCard; } private set {; } }
        public List<Card> Cemetery { get { return this.table.Cemetery; } private set {; } }
        public Table Table { get { return this.table; } private set {; } }
        public List<float> LifePoints { get { return this.lifePoints; } private set {; } }

        public void SetGameTurns(int gameID, int newGameTurn)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "El ID del Game es incorrecto.");
            this.gameTurns = newGameTurn;
            return;
        }
        public void SetTurnsByPlayer(int gameID, int playerID, int newTurn)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar los turnos de los jugadores.");
            MyExceptions.InvalidPlayerIDException(playerID, this.hands.Count - 1);
            this.turnsByPlayer[playerID] = newTurn;
            return;
        }
        public List<Card> GetHand(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.Table.Decks.Count - 1);
            return this.hands[playerID];
        }
        public void AddCardsToHand(int gameID, int playerID, Card newCard)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "No tiene permitido modificar la mano del jugador.");
            MyExceptions.InvalidPlayerIDException(playerID, this.Table.Decks.Count - 1);
            this.hands[playerID].Add(newCard);
            return;
        }
        public void RemoveCardsToHand(int gameID, int playerID, Card oldCard)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, " ID incorrecto. No tiene permitido modificar la mano del jugador.");
            MyExceptions.InvalidPlayerIDException(playerID, this.Table.Decks.Count - 1);
            this.hands[playerID].Remove(oldCard);
            return;
        }
        public float GetLifePoints(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.hands.Count - 1);
            return this.lifePoints[playerID];
        }
        public void SetLifePoints(int gameID, int playerID, float newLife)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID inválido. No puede modificar la vida de un jugador.");
            MyExceptions.InvalidPlayerIDException(playerID, this.hands.Count - 1);
            this.lifePoints[playerID] = newLife;
            return;
        }

    }
    public class Table
    {
        private List<List<Card>> magicCardsInvokeds;
        private List<List<Card>> monsterCardsInvokeds;
        private List<List<Card>> decks;
        private List<Card> principalDeck;
        private List<Card> cemetery;
        private Card fieldCard;
        private int gameID;
        private int maximumCardsInvokeds;

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
        public List<List<Card>> MagicCardsInvokeds { get { return this.magicCardsInvokeds; } private set {; } }
        public List<List<Card>> MonsterCardsInvokeds { get { return this.monsterCardsInvokeds; } private set {; } }
        public List<List<Card>> Decks { get { return this.decks; } private set {; } }
        public List<Card> Cemetery { get { return this.cemetery; } private set {; } }
        public Card FieldCard { get { return this.fieldCard; } private set {; } }
        public int MaximumCardsInvokeds { get { return this.maximumCardsInvokeds; } private set {; } }
        public List<Card> GetMagicCardsInvokeds(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            return this.magicCardsInvokeds[playerID];
        }
        public void SetMagicCard(int playerID, int gameID, MagicCard magicCard)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede invocar una carta mágica.");
            MyExceptions.FullInvokedCardsException(this.magicCardsInvokeds[playerID].Count, this.maximumCardsInvokeds, "Máximo de cartas alcanzado. No se pueden invocar más cartas mágicas");
            this.magicCardsInvokeds[playerID].Add(magicCard);
            return;
        }
        public List<Card> GetMonsterCardsInvokeds(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            return this.monsterCardsInvokeds[playerID];
        }
        public void SetMonsterCard(int playerID, int gameID, MonsterCard monsterCard)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.decks.Count - 1);
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede invocar un monstruo.");
            MyExceptions.FullInvokedCardsException(this.monsterCardsInvokeds[playerID].Count, this.maximumCardsInvokeds, "Máximo de cartas alcanzado. No se pueden invocar más cartas de monstruo.");
            this.monsterCardsInvokeds[playerID].Add(monsterCard);
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
    public class Rules
    {
        private (int, int) individualDeck;
        private int maxPointsLife;
        private int maxHand;
        //private ITurn structOfTurn;
        private State state;
        public Rules((int, int) individualDeck, int maxPointsLife, int maxHand, State state)
        {
            this.individualDeck = individualDeck;
            this.maxPointsLife = maxPointsLife;
            this.maxHand = maxHand;
            this.state = state;
        }
        public (int, int) IndividualDeck { get { return this.individualDeck; } private set {; } }
        public int MaxPointsLife { get { return this.maxPointsLife; } private set {; } }
        public int MaxHand { get { return this.maxHand; } private set {; } }
        public State State { get { return this.state; } private set {; } }
        // public List<bool> Phases
        // {
        //     get
        //     {
        //         List<bool> phases = new List<bool>();
        //         bool drawPhase = false;
        //         bool mainPhase = false;
        //         bool battlePhase = false;
        //         phases.Add(drawPhase);
        //         phases.Add(mainPhase);
        //         phases.Add(battlePhase);
        //         return phases;
        //     }
        // }
        public bool IsValidMove(ActionsEnum action, PhasesEnum phase, Player player, State state)
        {
            switch (action)
            {
                case ActionsEnum.draw:
                    return (phase == PhasesEnum.drawPhase) ? true : false;
                case ActionsEnum.mix://Falta cosiderar si hay fases en específico en las que se pueda (o no) barajar
                    return true;
                case ActionsEnum.invoke:
                    return (phase == PhasesEnum.mainPhase && IsValidInvoke(player, state)) ? true : (phase == PhasesEnum.endPhase && IsValidInvoke(player, state)) ? true : false;
                case ActionsEnum.attackCard:
                    return (phase == PhasesEnum.battlePhase && IsValidAttack(state)) ? true : false;
                case ActionsEnum.attackLifePoints:
                    return (phase == PhasesEnum.battlePhase && IsValidAttackPointsLife(state, player)) ? true : false;
                case ActionsEnum.discard: //Falta considerar lo mismo que con el drawPhase
                    return (phase == PhasesEnum.endPhase) ? true : false;
            }
            return true;
        }

        public bool IsValidInvoke(Player player, State state)
        {
            if (state.Table.MonsterCardsInvokeds[player.ID].Count == state.Table.MaximumCardsInvokeds) return false;

            if (state.Table.MagicCardsInvokeds[player.ID].Count == state.Table.MaximumCardsInvokeds) return false;

            return true;
        }
        public bool IsValidAttack(State state)
        {
            foreach (List<Card> invokedMonsterCards in state.Table.MonsterCardsInvokeds)
            {
                if (invokedMonsterCards.Count == 0) return false;
                //Falta considerar que las cartas atacante y atacada se encuentren dentro de las invocadas
                //Falta considerar los ciclos de ataques (1 jugador puede atacar varias veces en una misma fase, cada vez)
            }
            return true;
        }
        public bool IsValidAttackPointsLife(State state, Player attackantPlayer)
        {
            foreach (List<Card> invokedMonsterCards in state.Table.MonsterCardsInvokeds)
            {
                if (state.Table.MonsterCardsInvokeds.IndexOf(invokedMonsterCards) != attackantPlayer.ID)
                {
                    if (invokedMonsterCards.Count > 0) return false;
                }
                if (state.Table.MonsterCardsInvokeds.IndexOf(invokedMonsterCards) == attackantPlayer.ID)
                {
                    if (invokedMonsterCards.Count <= 0) return false;
                }
            }
            return true;
        }
        public bool IsEndGame(State state)
        {
            foreach (float lifepoints in state.LifePoints)
            {
                if (lifepoints <= 0) return true;
            }
            foreach (List<Card> deck in state.Table.Decks)
            {
                if (deck.Count <= 0) return true;
            }
            return false;
        }
        /* public Player GetWinner(State state)
         {
             Player playerDeRepuesto = new Player("Extra",5);
             int loserID;
             MyExceptions.InvalidInvokationOfMethodException(IsEndGame(state), "El juego aún no acaba. No hay ganador.");
             foreach (float lifepoints in state.LifePoints)
             {
                 if (lifepoints <= 0)
                 {
                     loserID = state.LifePoints.IndexOf(lifepoints);
                     foreach (Player player in state.Players)
                     {
                         if (state.Players.IndexOf(player) != loserID) return player;
                     }
                     break;
                 }
             }
             foreach (List<Card> deck in state.Table.Decks)
             {
                 if (deck.Count <= 0)
                 {
                     loserID = state.Table.Decks.IndexOf(deck);
                     foreach (Player player in state.Players)
                     {
                         if (state.Players.IndexOf(player) != loserID) return player;
                     }
                     break;
                 }
             }
             return

         }*/
        /* public Player GetLoser(State state)
         {
             MyExceptions.InvalidInvokationOfMethodException(IsEndGame(state), "El juego aún no acaba. No hay ganador.");
             foreach (float lifepoints in state.LifePoints)
             {
                 if (lifepoints <= 0) return state.Players[state.LifePoints.IndexOf(lifepoints)];
             }
             foreach (List<Card> deck in state.Table.Decks)
             {
                 if (deck.Count <= 0) return true;
             }

         }*/
        public List<Player> GetWinner(State state)
        {
            List<Player> result = new List<Player>();
            if (IsEndGame(state))
            {
                float lifesPlayer_1 = state.LifePoints[0];
                float lifesPlayer_2 = state.LifePoints[1];
                int countDeckPlayer_1 = state.Table.Decks[0].Count;
                int countDeckPlayer_2 = state.Table.Decks[1].Count;

                if ((lifesPlayer_1 == 0 || countDeckPlayer_1 == 0) && (lifesPlayer_2 > 0 && countDeckPlayer_2 != 0))
                {
                    // result = new List<Player>();
                    result.Add(state.Players[0]);
                    result.Add(state.Players[1]);
                }
                else if ((lifesPlayer_2 == 0 || countDeckPlayer_2 == 0) && (lifesPlayer_1 > 0 && countDeckPlayer_1 != 0))
                {
                    // result = new List<Player>();
                    result.Add(state.Players[1]);
                    result.Add(state.Players[0]);
                }
                else
                {
                    // result = new List<Player>();
                    result.Add(state.Players[0]);
                }
                return result;
            }
            return result;
        }
    }




    // public class StandarReferee : IReferee  //IMPLEMENTR***

    // {
    //     public IRules Reglas => throw new NotImplementedException();

    //     public List<Player> Players => throw new NotImplementedException();

    //     public State Estado => throw new NotImplementedException();

    //     public Table Tablero => throw new NotImplementedException();
    // }

    // public class StandarTurn : ITurn //Este turno está compuesto por 3 fases básicas (DrawPhase, MainPhase, BattlePhase)
    // {
    //     public List<IPhases> fases
    //     {
    //         get
    //         {
    //             List<IPhases> phases_ = new List<IPhases>();
    //             DrawPhase drawPhase = new DrawPhase();
    //             phases_.Add(drawPhase);
    //             MainPhase mainPhase = new MainPhase();
    //             phases_.Add(mainPhase);
    //             BattlePhase battlePhase = new BattlePhase();
    //             phases_.Add(battlePhase);
    //             return phases_;
    //         }
    //     }
    // }

    // public class DrawPhase : IPhases //fase de robo
    // {
    //     public List<ActionsEnum> acciones
    //     {
    //         get
    //         {
    //             List<ActionsEnum> actions_ = new List<ActionsEnum>();
    //             actions_.Add(ActionsEnum.robar);
    //             return actions_;
    //         }
    //     }
    // }
    // public class MainPhase : IPhases //fase donde se activan las cartas
    // {
    //     public List<ActionsEnum> acciones
    //     {
    //         get
    //         {
    //             List<ActionsEnum> actions_ = new List<ActionsEnum>();
    //             actions_.Add(ActionsEnum.colocar);
    //             actions_.Add(ActionsEnum.invocar);
    //             return actions_;
    //         }
    //     }
    // }
    // public class BattlePhase : IPhases //fase de batalla, cálculo de daño, el fin de esta fase implica el fin del turno
    // {
    //     public List<ActionsEnum> acciones
    //     {
    //         get
    //         {
    //             List<ActionsEnum> actions_ = new List<ActionsEnum>();
    //             actions_.Add(ActionsEnum.atacar);
    //             return actions_;
    //         }
    //     }
    // }



    #endregion Game



    #region Cards
    //CONSIDERAR UN BOOL IfACTIVE,POR CARTA, PARA QUE LA CARTA SE ENTERE DE SI ESTA O NO EN JUEGO,
    // LO CUAL IMPLICARIA QUE SUS VALORES SE PUEDEN MODIFICAR, DE LO CONTRARIO PERMANECERIAN INVARIABLES
    public abstract class Card
    {
        public string Name { get; private set; }
    }
    public class MonsterCard : Card
    {
        private string name;
        private CardState cardState;
        private float life; //por puntos (programar a parte para la futura creación de un monstruo random)
        private float attack; //número para la estadística del cálculo de daño (programar a parte para la futura creación de un monstruo random)
        private float defense; //número para la estadística del cálculo de daño (programar a parte para la futura creación de un monstruo random)
        private int gameID = -1;

        //private T cardState; //normal, borracho, lento, óptimo, moral... por efecto de alguna carta de hechizo (programar a parte para la futura creación de un monstruo random)

        public MonsterCard(string name, CardState cardState, float life, float attack, float defense)
        {
            this.name = name;
            this.cardState = cardState;
            this.life = life;
            this.attack = attack;
            this.defense = defense;
        }
        public string Name { get { return this.name; } private set {; } }
        public CardState CardState { get { return this.cardState; } private set {; } }
        public float Life { get { return this.life; } private set {; } }
        public float Attack { get { return this.attack; } private set {; } }
        public float Defense { get { return this.defense; } private set {; } }

        public void SetLife(int gameID, float modifiedLife)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar la vida del monstruo " + this.name + ".");
            this.life = modifiedLife;
            return;
        }
        public void SetAttack(int gameID, float modifiedAttack)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar el ataque del monstruo " + this.name + ".");
            this.attack = modifiedAttack;
            return;
        }
        public void SetDefense(int gameID, float modifiedDefense)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar la defensa del monstruo " + this.name + ".");
            this.defense = modifiedDefense;
            return;
        }
        //public T condicionesDeEvolucion, ***EXTRA***
        // ***CONDICION FENIX*** (exigencias que pide la carta a cambio de su resucitación: sacrificar 3 monstruos, estar en un campo específico, tener en juego 10 cartas x...)
        // Valorar que al resucitar una carta su estado sea ***"VETERANO"*** lo que se traduce a una cierta inmunidad hacia la carta que la envió al cementerio 
    }
    public class MagicCard : Card
    {
        public string name; //implementación de la interface 
        public string Name { get; private set; }
    }
    public class FieldCard : Card
    {
        public string Name { get; private set; }
    }
    #endregion Cards

    #region Players
    public class Move //Esto es el tipo de lo que devuelve el jugador cuando decide hacer una acción
    {
        private ActionsEnum action;
        private List<Card> cardsInTheAction = new List<Card>();
        public Move(ActionsEnum action, List<Card> cardsInTheAction)
        {
            this.action = action;
            this.cardsInTheAction = cardsInTheAction;
        }
        public ActionsEnum Action { get { return this.action; } private set {; } }
        public List<Card> CardsInTheAction { get { return this.cardsInTheAction; } private set {; } }
    }
    public class Player  //IMPLEMENTAR****
    {
        private List<IStrategy> strategies;
        private string name;
        private int ID_;
        private Move move;
        private State state;

        protected Player(string name, int ID, State state, List<IStrategy> strategies)
        {
            this.name = name;
            this.ID_ = ID;
            this.state = state;
            this.strategies = strategies;
        }
        public string Name { get { return this.name; } private set {; } }
        public int ID { get { return this.ID_; } private set {; } }
        public Move Move_ { get { return this.move; } private set {; } }
        public virtual Move Jugada(ActionsEnum action, List<Card> cardsInTheAction) //las jugadas que hará el jugador
        {
            throw new NotImplementedException();
        }
        /*draw,
       mix,
       invoke,
       attackCard,
       attackLifePoints,
       discard*/
    }
    #endregion Players

}
