namespace ProeliumEngine
{
    #region Game 
 
    public interface IRules //LA CLASE RULES NO IMPLEMENTA ESTA INTERFAZ, pero buee.. nadie sabe
    {
        public (int, int) IndividualDeck { get; } //intervalo de cantidad de cartas en el mazo individual
        public int MaxPointsLife { get; } //máxima cantidad de puntos de vida por jugador
        public int MaxHand { get; } //cantidad máxima de cartas en la mano de cada jugador
        List<bool> Phases { get; } //fases por turno por jugador. donde si una fase está en true indica la fase actual, si todas están en false, indica que es el turno del contrario 

        //public bool IsValidMove(Player player, Card cartToPlay, (int, int) position); ???*** //verifica la jugada del player actual (recibe el jugador activo, la carta a jugar, y la posición en que la va a jugar)

        public bool IsEndGame(); //verifica si el estado actual es el final
        public List<IPlayer> EndGame(); //si el método EndGame devuelve true pone fin al juego y declara al vencedor

        //public void DynamicPhases();????*** //método que dice las posibles acciones a realizar por fase
    }

    /* public interface ITable //TABLERO (guarda las posiciones de cada elemento del juego)  --> NADA DE ESTO NO SE UTILIZA, pero quizás en un futuro.. nadie sabe
     {
         public bool[,] table { get; set; }
     }
     public interface IAction
     {
         public void Action();
     }
     public interface IReferee 
    {
        public IRules Reglas { get; }
        public List<IPlayer> Players { get; } 
        public State Estado { get; }
        public ITable Tablero { get; }
    }*/


    public interface ITurn //Cómo está compuesto un turno (conjunto de acciones) (mmm... esto no lo utilizo, pero bue)
    {
        public List<IPhases> fases { get; } //fases por turno
    }

    public interface IPhases //Estructutar de una fase de un turno (mmm... esto no lo he utilizado, pero buee)
    {
        public List<ActionsEnum> acciones { get; } 
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
       mix,
       invoke,
       attackCard,
       attackLifePoints,
       discard,
       endPhase,
       endTurn
    }
   
    public interface ICard
    {
        string Name { get; }
    }
    //***
    public interface ICardState //Estados de la carta (borracho, neutral, dormido...) 
    {
        //****IMPLEMENTARRRRRR!!!!!
    }
     public enum CardState { } //Puede que haga falta este enum para los estados de arriba
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