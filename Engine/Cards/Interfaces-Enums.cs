namespace ProeliumEngine
{
    #region Game  
    public interface IRules 
    {
        public (int, int) IndividualDeck { get; } //intervalo de cantidad de cartas en el mazo individual
        public int MaxPointsLife { get; } //máxima cantidad de puntos de vida por jugador
        public int MaxHand { get; } //cantidad máxima de cartas en la mano de cada jugador
        public bool IsValidMove(Move jugada, PhasesEnum phase, int playerID, State state);//verifica la jugada del player actual (recibe el jugador activo, la carta a jugar, y la posición en que la va a jugar)
        public bool IsValidInvoke(int playerID, State state, Move jugada);
        public bool IsValidAttack(State state, int playerID, Move jugada);
        public bool IsValidAttackPointsLife(State state, int attackantPlayerID, Move jugada);
        public bool IsEndGame(State state); //verifica si el estado actual es el final
        public List<Player> GetWinner(State state); //si el método EndGame devuelve true pone fin al juego y declara al vencedor o el empate
    }
    public interface ITable
    {
        public List<List<Card>> MagicCardsInvokeds { get; }
        public List<List<Card>> MonsterCardsInvokeds { get; }
        public List<List<Card>> Decks { get; }
        public List<Card> Cemetery { get; }
        public Card FieldCard { get; }
        public int MaximumCardsInvokeds { get; }
    }
    public interface IAction
    {
        public State Draw(int playerID, int gameID, State state);
        public State Mix(int playerID, int gameID, State state);
        public State Invoke(Card card, int playerID, int gameID, State state);
        public State ActivateEffect(Card card, int playerID, int gameID, State state);
        public State AttackCard(int gameID, int attackantPlayerID, Card cardAtacante, Card cardAtacada, State state);
        public State AttackLifePoints(int gameID, int playerID, Card attackantCard, State state);
        public State Discard(int gameID, int playerID, Card card, State state);
        public bool EndPhase(int gameID, State state);
        public bool EndTurn(int gameID, Player actualPlayer, State state);
    }
    public enum PhasesEnum
    {
        drawPhase,
        mainPhase,
        battlePhase,
        endPhase
    }
    #endregion Game

    #region Cards
    public enum ActionsEnum
    {
        activateEffect,
        mix,
        invoke,
        attackCard,
        attackLifePoints,
        discard,
        endPhase,
        endTurn
    }
    // public interface ICardState //Estados de la carta (borracho, neutral, dormido...) {}  FUTURA MODIFICACION
    //public enum CardState { } //Puede que haga falta este enum para los estados de arriba
    #endregion Cards

    #region Players
    public interface IPlayer
    {
        public int id { get; }
    }
    public interface IStrategy 
    {        
        public Move Play(State state);
    }
    #endregion Players
}