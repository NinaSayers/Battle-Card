public struct EstadoDelJuego
{
    // - Vidas de ambos jugadores
    // - Cartas jugadas (tener en cuenta la posiciín de las cartas en el tablero)
    // - Cantidad de cartas en el mazo individual de cada jugador
    // - * Cantidad de cartas en el mazo central (evolución de las cartas) ***EXTRA
    // - Cartas en la mano de cada jugador (hay un límite)
    // - Conteo de turnos individuales
    // - Conteo de turnos del juego;
    // - Cementerio de cartas

    /* **** ESTADO INICIAL **** */
    // - 0 cartas jugadas (0 cartas en el campo y en el cementerio) 
    // y
    // - 0 turnos
    // y
    // - vidas llenas (jugadores y cartas)
    // y
    // - mazos y manos llenas
    

    /* **** ESTADO FINAL **** */
    // - Uno de los jugadores sin vida
    // o
    // - Uno de los jugadores sin cartas en su mazo individual
    // o
    // - Empate: ambos jugadores pierden todas las vidas (solamente posible con el contraataque)
    // o
    // Por efecto de una carta (ej: "el juego termina en el turno 20");
}
