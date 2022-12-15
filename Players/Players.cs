using BattleCardsEngine;

    public class Move //Esto es el tipo de lo que devuelve el jugador cuando decide hacer una acción
    {
        private ActionsEnum action;
        private List<Card> cardsInTheAction = new List<Card>();
        public Move(ActionsEnum action, List<Card> cardsInTheAction)
        {
            this.action = action;
            this.cardsInTheAction = cardsInTheAction;
        }
        public Move(ActionsEnum action)
        {
            this.action = action;
        }
        public ActionsEnum Action { get { return this.action; } }
        public List<Card> CardsInTheAction { get { return this.cardsInTheAction; } }
    }
    public class Player
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
        public string Name { get { return this.name; } }
        public int ID { get { return this.ID_; } }
        public Move Move_ { get { return this.move; } }
        public virtual BattleCardsEngine.Move Jugada(State state) //las jugadas que hará el jugador
        {
            return this.strategies[0].Play(state);
        }
    }
    public class Greedy : IStrategy
    {
        //private State state;
        private Rules rules;
        private int playerID;
        public Greedy(Rules rules, int playerID)
        {
            this.rules = rules;
            this.playerID = playerID;
        }

        public Move Play(State state)
        {
            bool yaInvocó = false;
            if (state.ActualPhase == PhasesEnum.mainPhase)
            {
                if (state.GetHand(playerID).Count! > 0)
                {
                    foreach (Card card in state.GetHand(playerID))
                    {
                        List<Card> invokedCard = new List<Card> { card };
                        if (this.rules.IsValidInvoke(this.playerID, state, new BattleCardsEngine.Move(ActionsEnum.invoke, invokedCard)))
                        {
                            yaInvocó = true;
                            return new Move(ActionsEnum.invoke, invokedCard);
                        }
                    }
                    if (!yaInvocó) return new Move(ActionsEnum.endPhase);
                }
            }
            if (state.ActualPhase == PhasesEnum.battlePhase)
            {
                if (state.Table.GetMonsterCardsInvokeds(playerID).Count == 0) return new Move(ActionsEnum.endPhase);
                foreach (List<Card> cards in state.Table.MonsterCardsInvokeds)
                {
                    if (state.Table.MonsterCardsInvokeds.IndexOf(cards) != playerID)
                    {
                        for (int i = 0; i < state.Table.GetMonsterCardsInvokeds(this.playerID).Count; i++)
                        {
                            if (!state.GetYaAtacó(this.playerID)[i])
                            {
                                if (cards.Count == 0)
                                {
                                    List<Card> actionCards = new List<Card> { state.Table.GetMonsterCardsInvokeds(this.playerID)[i] };
                                    if (this.rules.IsValidAttackPointsLife(state, this.playerID, new BattleCardsEngine.Move(ActionsEnum.attackLifePoints, actionCards))) return new Move(ActionsEnum.attackLifePoints, actionCards);
                                    else continue;
                                }
                                else
                                {
                                    List<Card> actionCards = new List<Card> { state.Table.GetMonsterCardsInvokeds(this.playerID)[i], cards[cards.Count - 1] };
                                    if (this.rules.IsValidAttack(state, this.playerID, new BattleCardsEngine.Move(ActionsEnum.attackCard, actionCards))) return new Move(ActionsEnum.attackCard, actionCards);
                                }
                            }
                        }
                        if (this.rules.IsValidMove(new BattleCardsEngine.Move(ActionsEnum.endPhase), state.ActualPhase, this.playerID, state)) return new Move(ActionsEnum.endPhase);
                    }
                }
            }
            if (state.ActualPhase == PhasesEnum.endPhase)
            {
                if (state.GetHand(playerID).Count! > 0)
                {
                    foreach (Card card in state.GetHand(playerID))
                    {
                        List<Card> actionCard = new List<Card> { card };
                        if (this.rules.IsValidInvoke(this.playerID, state, new BattleCardsEngine.Move(ActionsEnum.invoke, actionCard)))
                        {
                            yaInvocó = true;
                            List<Card> invokedCard = new List<Card> { card };
                            return new Move(ActionsEnum.invoke, invokedCard);
                        }
                    }
                    if (!yaInvocó) return new Move(ActionsEnum.endPhase);
                }
            }
            return new Move(ActionsEnum.endPhase);
        }
    }
