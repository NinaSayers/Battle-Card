using BattleCardsEngine;
public class MonsterCard<T> : ICard<T> where T : IEffect
    {
        public int life; //por puntos (programar a parte para la futura creación de un monstruo random)
        public double attack; //número para la estadística del cálculo de daño (programar a parte para la futura creación de un monstruo random)
        public double defense; //número para la estadística del cálculo de daño (programar a parte para la futura creación de un monstruo random)
        public T cardState; //normal, borracho, lento, óptimo, moral... por efecto de alguna carta de hechizo (programar a parte para la futura creación de un monstruo random)

        public string Name { get; private set; }

        public T Effect { get; private set; }

        public carta_type Type { get; private set; }

        //public T condicionesDeEvolucion, ***EXTRA***
        // ***CONDICION FENIX*** (exigencias que pide la carta a cambio de su resucitación: sacrificar 3 monstruos, estar en un campo específico, tener en juego 10 cartas x...)
        // Valorar que al resucitar una carta su estado sea ***"VETERANO"*** lo que se traduce a una cierta inmunidad hacia la carta que la envió al cementerio
    }