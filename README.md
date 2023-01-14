# Proelium

![](proelium.png)

> Proyecto de Programación II.
> Facultad de Matemática y Computación - Universidad de La Habana.
> Cursos 2021, 2022.

Proelium, cuya traducción al español es “batalla”, es un juego basado en el famoso Yu-hi-oh!

Se trata de un enfrentamiento entre dos jugadores y cuyo objetivo es llevar a 0 los puntos de vida del adversario, o dejar a este sin cartas que robar en su mazo personal.
Para ello utilizarán durante los enfrentamientos cartas de tipo monstruo y mágicas.

Cada turno está compuesto por una serie de fases dispuestas en este mismo orden:

- `Robo` tiene lugar, de forma automática, el robo de una carta del mazo individual.
- `Fase principal`  el jugador puede elegir invocar cartas que tenga en su mano y/o activar los efectos de las cartas que estén en el campo.
(Téngase en cuenta que existe un límite de cartas a invocar en el campo, y que las cartas mágicas tienen una duración efímera en el tablero, es decir, una vez invocadas se activa automáticamente su efecto y acto seguido es enviada al cementerio.)
- `Fase de batalla` se realizan los ataques a través de las cartas de monstruo. El jugador activo puede elegir si realiza o no un ataque a una carta de monstruo. En caso de que el oponente no tenga invocadas cartas de tipo monstruo, el jugador activo podrá realizar ataques directos y así restarle puntos de vida al adversario. 
- `Fase final` se puede invocar cartas y/o activar el efecto de algunas de las barajas que estén invocadas y cuyo efecto no haya sido activado previamente en el turno.

En cualquier fase el jugador activo puede decidir finalizar la fase actual o su turno.

El juego finaliza una vez que alguno de los jugadores (o los dos) se haya quedado sin puntos de vida o sin cartas en su mazo individual en el momento de robar. El 1er jugador en esta situación resultará ser el perdedor y el otro el vencedor. El juego se declara empate si ambos jugadores se quedan sin vidas al mismo tiempo.

# Sobre los efectos

Cada carta tiene un efecto de tipo `Satement` que se instancia a través de un texto de tipo `string` escrito por el usuario.

Las palabras clave del lenguaje estan definidas de la siguiente forma:

- `if` representa una instriccion condicional que va acompañada de `then` para indicar la accion a realizar.
- `minus`, `plus`, `mult`, `div` represantan las operaciones de adición, substracción, multiplicación y división.
- `creace`, `decreace`, `idem` son equivalentes a `+=`, `-=` y `=`.
- `minor`, `maior`, `idemI` son equivalentes a `<`, `>`y `==`.
- `draw` representa la acción de robar una carta en el juego y va acopañada a su derecha del número de cartas a robar.
- `mix` representa la acción de mezclar el mazo del jugador.
- Los literales numéricos como `22, 34, ...`

> Un ejemplo de como utilizar esto

Se quiere crear que si el oponente tiene menos de 2 puntos de vida, se roben 2 cartas.
Para esto la instrucción sería:
```cs
if PlayerID idemI 0 then
    if Player2.life minor 2 then
        draw 2;

if PlayerID idemI 1 then
    if Player1.life minor 2 then
        draw 2;
```

*Nota que en el ejemplo se utiliza la palabra clave `PlayerID` para referirse al jugador activo, y `Player1` y `Player2` para referirse al jugador 1 y 2 respectivamente. Asi mismo, se utiliza `Player1.life` para referirse a los puntos de vida del jugador 1.*
