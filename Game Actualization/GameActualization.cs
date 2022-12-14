using BattleCardsEngine;
  public class Game : IEnumerable<State> //Falta terminar automáticamente la MainPhase cuando ya no queden cartas por invocar*****
    {
        private Rules rules;
        private State state;
        private Actions actions;
        int gameID;
        private List<PhasesEnum> phases = new List<PhasesEnum> { PhasesEnum.drawPhase, PhasesEnum.mainPhase, PhasesEnum.battlePhase, PhasesEnum.endPhase };
        public Game(Rules rules, Actions actions, State state, int gameID)
        {
            this.rules = rules;
            this.state = state;
            this.actions = actions;
            this.gameID = gameID;
        }

        public IEnumerable<State> Game_()
        {
            bool end = false;
            bool endPhase = false;
            bool endTurn = false;
            bool[] maskAttack;
            while (!this.rules.IsEndGame(this.state))
            {
                foreach (Player player in this.state.Players)
                {
                    endTurn = false;
                    maskAttack = new bool[this.state.Table.GetMonsterCardsInvokeds(player.ID).Count];

                    foreach (PhasesEnum phase in this.phases)
                    {
                        endPhase = false;
                        if (phase == PhasesEnum.drawPhase)
                        {
                            this.state.SetActualPhase(this.gameID, phase);
                            yield return this.state;
                            this.actions.Draw(player.ID, this.gameID, state);
                            yield return this.state;
                            continue;
                        }
                        while (!endPhase || !endTurn)
                        {
                            this.state.SetActualPhase(this.gameID, phase);
                            yield return this.state;

                            Move jugada = player.Move_;
                            if (jugada.Action == ActionsEnum.endPhase) { endPhase = true; continue; }
                            if (jugada.Action == ActionsEnum.endTurn) { endTurn = true; continue; }
                            if (this.rules.IsValidMove(jugada, phase, player, this.state))
                            {
                                if (jugada.Action == ActionsEnum.endPhase) { endPhase = true; }
                                if (jugada.Action == ActionsEnum.endTurn) { endTurn = true; }
                                else
                                {
                                    if (jugada.Action == ActionsEnum.attackCard || jugada.Action == ActionsEnum.attackLifePoints)
                                    {
                                        if (IsFullCardAttack(maskAttack)) { endPhase = true; continue; }
                                    }
                                    if (jugada.Action == ActionsEnum.invoke)
                                    {
                                        if (jugada.CardsInTheAction[0].GetType() == typeof(MagicCard))
                                        {
                                            if (this.state.Table.GetMagicCardsInvokeds(player.ID).Count == this.state.Table.MaximumCardsInvokeds) { endPhase = true; continue; }
                                        }

                                        if (jugada.CardsInTheAction[0].GetType() == typeof(MonsterCard))
                                        {
                                            if (this.state.Table.GetMonsterCardsInvokeds(player.ID).Count == this.state.Table.MaximumCardsInvokeds) { endPhase = true; continue; }
                                        }
                                    }
                                    ActionToDo(player, jugada, this.state);
                                }
                                yield return this.state;
                                if (this.rules.IsEndGame(this.state)) { end = true; break; }
                            }
                            else throw new Exception("Jugada no válida."); //Cómo darle a conocer esto al player sin lanzar exception?**???
                        }
                        if (endTurn) break;
                    }
                    endTurn = true;

                    if (end) break;
                    this.state.SetTurnsByPlayer(this.gameID, player.ID, this.state.TurnsByPlayer[player.ID] + 1);
                    yield return this.state;
                    if (this.rules.IsEndGame(this.state)) { end = true; break; }
                }
                if (end) break;
                this.state.SetGameTurns(this.gameID, this.state.GameTurns + 1);
                yield return this.state;
                if (this.rules.IsEndGame(this.state)) break;
            }
        }
        public IEnumerator<State> GetEnumerator()
        {
            return Game_().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ActionToDo(Player player, Move jugada, State state) //Realiza la acción en el juego de acuerdo a la entrada del jugador
        {
            switch (jugada.Action)
            {
                case ActionsEnum.mix:
                    this.state = this.actions.Mix(player.ID, this.gameID, state);
                    break;
                case ActionsEnum.invoke:
                    this.state = this.actions.Invoke(jugada.CardsInTheAction[0], player.ID, this.gameID, state);
                    break;
                case ActionsEnum.attackCard:

                    this.state = this.actions.AttackCard(this.gameID, player.ID, jugada.CardsInTheAction[0] as MonsterCard, jugada.CardsInTheAction[1] as MonsterCard, state);
                    break;
                case ActionsEnum.attackLifePoints:
                    this.state = this.actions.AttackLifePoints(this.gameID, player.ID, jugada.CardsInTheAction[0] as MonsterCard, state);
                    break;
                case ActionsEnum.discard:
                    this.state = this.actions.Discard(this.gameID, player.ID, jugada.CardsInTheAction[0], state);
                    break;

            }
            return;
        }
        private bool IsFullCardAttack(bool[] cardsQueAtacaron)
        {
            foreach (bool card in cardsQueAtacaron)
            {
                if (!card) return false;
            }
            return true;
        }
    }

    public class Actions
    {
        private int gameID;
        public Actions(int gameID)
        {
            this.gameID = gameID;
        }
        public State Draw(int playerID, int gameID, State state)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede robar una carta del mazo.");
            MyExceptions.InvalidPlayerIDException(playerID, state.Table.Decks.Count - 1);
            MyExceptions.EmptyCollectionCardsException(state.Table.GetDeck(playerID), "No hay cartas en el mazo.");
            int countDeck = state.Table.GetDeck(playerID).Count;
            Card newcard = state.Table.GetDeck(playerID)[countDeck - 1];
            State newState = new State(state.GameTurns, state.TurnsByPlayer, state.ActualPhase, state.Players, state.Hands, state.YaAtacó, state.Table, this.gameID, state.LifePoints);
            newState.AddCardsToHand(this.gameID, playerID, newcard);
            newState.Table.RemoveCardToDeck(playerID, gameID, newcard);
            return newState;
        }
        public State Mix(int playerID, int gameID, State state)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede barajar el Deck.");
            MyExceptions.InvalidPlayerIDException(playerID, state.Table.Decks.Count - 1);
            MyExceptions.EmptyCollectionCardsException(state.Table.GetDeck(playerID), "No hay cartas en el mazo.");

            List<Card> oldDeck = new List<Card>(state.Table.GetDeck(playerID).Count);
            oldDeck = state.Table.GetDeck(playerID);
            List<Card> newDeck = new List<Card>(oldDeck.Count);
            Random r = new Random();
            for (int i = 0; i < newDeck.Count; i++)
            {
                int n = r.Next(oldDeck.Count);
                newDeck[i] = oldDeck[n - 1];
                oldDeck.RemoveAt(n - 1);
            }
            State newState = new State(state.GameTurns, state.TurnsByPlayer, state.ActualPhase, state.Players, state.Hands, state.YaAtacó, state.Table, this.gameID, state.LifePoints);
            newState.Table.SetDeck(playerID, gameID, newDeck);
            return newState;
        }
        public State Invoke(Card card, int playerID, int gameID, State state)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID inconrrecto. No puede invocar cartas.");
            MyExceptions.InvalidPlayerIDException(playerID, state.Table.Decks.Count - 1);
            State newState = new State(state.GameTurns, state.TurnsByPlayer, state.ActualPhase, state.Players, state.Hands, state.YaAtacó, state.Table, this.gameID, state.LifePoints);
            if (card is MonsterCard)
            {
                int actualCount = state.Table.GetMonsterCardsInvokeds(playerID).Count;
                int maximumCount = state.Table.MaximumCardsInvokeds;
                MyExceptions.FullInvokedCardsException(actualCount, maximumCount, "Máximo de cartas alcanzado. No se pueden invocar más cartas de monstruo.");
                MyExceptions.NoFoundedCardException(state.GetHand(playerID), card, "No se pueden invocar cartas que no estén en la mano del jugador.");
                newState.RemoveCardsToHand(this.gameID, playerID, card);
                newState.Table.SetMonsterCard(playerID, this.gameID, card as MonsterCard);
                newState.AddYaAtacó(this.gameID, playerID);
            }
            if (card is MagicCard)
            {
                int actualCount = state.Table.GetMagicCardsInvokeds(playerID).Count;
                int maximumCount = state.Table.MaximumCardsInvokeds;
                MyExceptions.FullInvokedCardsException(actualCount, maximumCount, "Máximo de cartas alcanzado. No se pueden invocar más cartas mágicas.");
                MyExceptions.NoFoundedCardException(state.GetHand(playerID), card, "No se pueden invocar cartas que no estén en la mano del jugador.");
                newState.RemoveCardsToHand(this.gameID, playerID, card);
                newState.Table.SetMagicCard(playerID, this.gameID, card as MagicCard);
            }
            if (card is FieldCard)
            {
                MyExceptions.NoFoundedCardException(state.GetHand(playerID), card, "No se pueden invocar cartas que no estén en la mano del jugador.");
                newState.Table.SetFieldCard(this.gameID, card as FieldCard);
            }
            return newState;
        }
        public State AttackCard(int gameID, int attackantPlayerID, Card cardAtacante, Card cardAtacada, State state)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No tiene permitido realizar ataques.");
            MyExceptions.InvalidPlayerIDException(attackantPlayerID, state.Table.Decks.Count - 1);
            MyExceptions.InvalidTypOfCardException(cardAtacada.GetType(), typeof(MonsterCard), "Solo pueden ser atacadas las  cartas de tipo monstruo.");
            MyExceptions.InvalidTypOfCardException(cardAtacante.GetType(), typeof(MonsterCard), "Solo pueden atacar las cartas de tipo monstruo.");
            MyExceptions.InvalidAttackantCardException(state.Table.GetMonsterCardsInvokeds(attackantPlayerID), cardAtacante as MonsterCard);
            bool yaAtacó = false;
            State newState = new State(state.GameTurns, state.TurnsByPlayer, state.ActualPhase, state.Players, state.Hands, state.YaAtacó, state.Table, this.gameID, state.LifePoints);

            foreach (List<Card> invokedMonsterCards in state.Table.MonsterCardsInvokeds)
            {
                if (newState.Table.MonsterCardsInvokeds.IndexOf(invokedMonsterCards) != attackantPlayerID)
                {
                    MyExceptions.NoFoundedCardException(invokedMonsterCards, cardAtacada, "Esta carta no está invocada por el jugador contrario.");
                    foreach (MonsterCard monsterCard in invokedMonsterCards)
                    {
                        if ((monsterCard == cardAtacada) && (!yaAtacó))
                        {
                            yaAtacó = true;
                            newState.MarkYaAtacó(this.gameID, attackantPlayerID, cardAtacante);
                            if (monsterCard.Defense <= 0)
                            {
                                newState.Table.AddCardToCemetery(this.gameID, monsterCard);
                                newState.RemoveYaAtacó(this.gameID, newState.Table.MonsterCardsInvokeds.IndexOf(invokedMonsterCards));
                                invokedMonsterCards.Remove(monsterCard);

                            }
                            else
                            {
                                float damage = (cardAtacante as MonsterCard).Attack * 1000 / monsterCard.Defense;
                                monsterCard.SetLife(this.gameID, monsterCard.Life - damage);
                                if (monsterCard.Life <= 0)
                                {
                                    newState.Table.AddCardToCemetery(this.gameID, monsterCard);
                                    invokedMonsterCards.Remove(monsterCard);
                                }
                            }
                        }
                    }
                }
            }
            return newState;
        }
        public State AttackLifePoints(int gameID, int playerID, Card attackantCard, State state)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede atacar directamente la vida del jugador contrario");
            MyExceptions.InvalidPlayerIDException(playerID, state.Hands.Count - 1);
            MyExceptions.InvalidTypOfCardException(attackantCard.GetType(), typeof(MonsterCard), "No puede atacar con cartas que no sean de tipo monstruo.");
            MyExceptions.InvalidAttackantCardException(state.Table.MonsterCardsInvokeds[playerID], (attackantCard as MonsterCard));

            State newState = new State(state.GameTurns, state.TurnsByPlayer, state.ActualPhase, state.Players, state.Hands, state.YaAtacó, state.Table, this.gameID, state.LifePoints);
            newState.SetLifePoints(gameID, playerID, state.GetLifePoints(playerID) - (attackantCard as MonsterCard).Attack);
            return newState;
        }
        public State Discard(int gameID, int playerID, Card card, State state) //descartar cartas de la mano y enviarlas al mazo del jugador
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede descartar cartas de la mano del jugador.");
            MyExceptions.InvalidPlayerIDException(playerID, state.Hands.Count - 1);
            MyExceptions.NoFoundedCardException(state.Hands[playerID], card, "No puede descartar una carta que no esté en la mano del jugador.");
            State newState = new State(state.GameTurns, state.TurnsByPlayer, state.ActualPhase, state.Players, state.Hands, state.YaAtacó, state.Table, this.gameID, state.LifePoints);
            newState.RemoveCardsToHand(this.gameID, playerID, card);
            return newState;
        }
        public bool EndPhase(int gameID, State state)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede cambiar de fase.");
            return true;
        }
        public bool EndTurn(int gameID, Player actualPlayer, State state)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede finalizar el turno.");
            return true;
        }
    }
    public struct State
    {
        private int gameTurns; //turnos totales del juego
        private List<int> turnsByPlayer;
        private PhasesEnum actualPhase;
        private List<Player> players;
        private List<List<Card>> hands; //cartas en mano de los jugadores
        private List<List<bool>> yaAtacó;//cartas que ya atacaron en una misma fase
        private List<float> lifePoints; //vidas de los jugadores 
        private Table table;
        private int gameID;

        public State(int gameTurns, List<int> turnsByPlayer, PhasesEnum actualPhase, List<Player> players, List<List<Card>> hands, List<List<bool>> yaAtacó, Table table, int gameID, List<float> lifePoints)
        {
            this.gameTurns = gameTurns;
            this.turnsByPlayer = turnsByPlayer;
            this.actualPhase = actualPhase;
            this.players = players;
            this.hands = hands;
            this.yaAtacó = yaAtacó;
            this.table = table;
            this.gameID = gameID;
            this.lifePoints = lifePoints;
        }
        public int GameTurns { get { return this.gameTurns; } }
        public List<int> TurnsByPlayer { get { return this.turnsByPlayer; } }
        public PhasesEnum ActualPhase { get { return this.actualPhase; } }
        public List<Player> Players { get { return this.players; } }
        public List<List<Card>> Hands { get { return this.hands; } }
        public List<List<bool>> YaAtacó { get { return this.yaAtacó; } }
        public Card FieldCard { get { return this.table.FieldCard; } }
        public List<Card> Cemetery { get { return this.table.Cemetery; } }
        public Table Table { get { return this.table; } }
        public List<float> LifePoints { get { return this.lifePoints; } }

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
        public void SetActualPhase(int gameID, PhasesEnum newPhase)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No tiene permiso para actualizar la fase del turno.");
            this.actualPhase = newPhase;
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
        public List<bool> GetYaAtacó(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.Hands.Count - 1);
            return this.yaAtacó[playerID];
        }
        public bool IsFullYaAtacó(int playerID)
        {
            MyExceptions.InvalidPlayerIDException(playerID, this.hands.Count);
            foreach (bool card in this.yaAtacó[playerID])
            {
                if (!card) return false;
            }
            return true;
        }
        public void AddYaAtacó(int gameID, int playerID)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede actualizar (añadir) cartas para la mask de cartas que atacaron.");
            MyExceptions.InvalidPlayerIDException(playerID, this.hands.Count - 1);
            this.yaAtacó[playerID].Add(false);
            return;
        }
        public void RemoveYaAtacó(int gameID, int playerID)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID inválido. No puede actualizar (remover) de la mask bool de las cartas que ya atacaron.");
            MyExceptions.InvalidPlayerIDException(playerID, this.hands.Count - 1);
            MyExceptions.EmptyCollectionCardsException(this.table.GetMonsterCardsInvokeds(playerID), "No puede actualizar (remover) de la mask de cartas que atacaron, pues está vacía.");
            this.yaAtacó.RemoveAt(this.yaAtacó.Count - 1);
            return;
        }

        public void MarkYaAtacó(int gameID, int playerID, Card attackantCard)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar la info de cartas que ya atacaron.");
            MyExceptions.InvalidPlayerIDException(playerID, this.hands.Count - 1);
            MyExceptions.InvalidTypOfCardException(attackantCard.GetType(), typeof(MonsterCard), "No puede atacar con cartas que no sean de tipo monstruo.");
            MyExceptions.NoFoundedCardException(this.table.GetMonsterCardsInvokeds(playerID), attackantCard, "No puede atacar con una carta que no haya sido invocada.");
            this.yaAtacó[playerID][this.table.GetMonsterCardsInvokeds(playerID).IndexOf(attackantCard)] = true;
            return;
        }
        public void ResetYaAtacó(int gameID, int playerID)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar la info de cartas que ya atacaron.");
            MyExceptions.InvalidPlayerIDException(playerID, this.hands.Count - 1);
            for (int i = 0; i < this.yaAtacó[playerID].Count; i++)
            {
                this.yaAtacó[playerID][i] = false;
            }
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
        public (int, int) IndividualDeck { get { return this.individualDeck; } }
        public int MaxPointsLife { get { return this.maxPointsLife; } }
        public int MaxHand { get { return this.maxHand; } }
        public State State { get { return this.state; } }

        public bool IsValidMove(Move jugada, PhasesEnum phase, Player player, State state)
        {
            switch (jugada.Action)
            {
                case ActionsEnum.mix://Falta cosiderar si hay fases en específico en las que se pueda (o no) barajar
                    return true;
                case ActionsEnum.invoke:
                    return (phase == PhasesEnum.mainPhase && IsValidInvoke(player, state)) ? true : (phase == PhasesEnum.endPhase && IsValidInvoke(player, state)) ? true : false;
                case ActionsEnum.attackCard:
                    return (phase == PhasesEnum.battlePhase && IsValidAttack(state, player, jugada)) ? true : false;
                case ActionsEnum.attackLifePoints:
                    return (phase == PhasesEnum.battlePhase && IsValidAttackPointsLife(state, player, jugada)) ? true : false;
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
        public bool IsValidAttack(State state, Player player, Move jugada)
        {
            if (!(jugada.CardsInTheAction[0] is MonsterCard) || !(jugada.CardsInTheAction[1] is MonsterCard)) return false;
            foreach (List<Card> invokedMonsterCards in state.Table.MonsterCardsInvokeds)
            {
                if (invokedMonsterCards.Count == 0) return false;
                if (state.Table.MonsterCardsInvokeds.IndexOf(invokedMonsterCards) != player.ID)
                {
                    if (!(invokedMonsterCards.Contains(jugada.CardsInTheAction[1])) || !(state.Table.MonsterCardsInvokeds[player.ID].Contains(jugada.CardsInTheAction[0]))) return false;
                }

                //Falta considerar que las cartas atacante y atacada se encuentren dentro de las invocadas
                //Falta considerar los ciclos de ataques (1 jugador puede atacar varias veces en una misma fase, cada vez)
            }
            return true;
        }
        public bool IsValidAttackPointsLife(State state, Player attackantPlayer, Move jugada)
        {
            if (!(jugada.CardsInTheAction[0] is MonsterCard)) return false;
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
        public List<Player>? GetWinner(State state)
        {
            List<Player>? result = null;
            if (IsEndGame(state))
            {
                result = new List<Player>();
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

