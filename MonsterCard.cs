using BattleCardsEngine;
 public class MonsterCard<T> : ICard<T> where T : IEffect
    {
        private string name;
        private carta_type type;
        private T effect;
        private float life; //por puntos (programar a parte para la futura creación de un monstruo random)
        private float attack; //número para la estadística del cálculo de daño (programar a parte para la futura creación de un monstruo random)
        private float defense; //número para la estadística del cálculo de daño (programar a parte para la futura creación de un monstruo random)
        private int gameID;

        //private T cardState; //normal, borracho, lento, óptimo, moral... por efecto de alguna carta de hechizo (programar a parte para la futura creación de un monstruo random)

        public MonsterCard(string name, carta_type type, T effect, float life, float attack, float defense, int gameID)
        {
            this.name = name;
            MyExceptions<T>.InvalidTypOfCardException(type, carta_type.monster, "Una carta de monstruo debe ser creada como tipo 'monstruo'.");
            this.type = type;
            this.effect = effect;
            this.life = life;
            this.attack = attack;
            this.defense = defense;
            this.gameID = gameID;
        }
        public string Name { get { return this.name; } private set {; } }
        public carta_type Type { get { return this.type; } private set {; } }
        public T Effect { get; private set; }
        public float Life { get { return this.life; } private set {; } }
        public float Attack { get { return this.attack; } private set {; } }
        public float Defense { get { return this.defense; } private set {; } }

        public void SetLife(int gameID, int midifiedLife)
        {
            MyExceptions<T>.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar la vida del monstruo " + this.name + ".");
            this.life = midifiedLife;
            return;
        }
        public void SetAttack(int gameID, int modifiedAttack)
        {
            MyExceptions<T>.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar el ataque del monstruo " + this.name + ".");
            this.attack = modifiedAttack;
            return;
        }
        public void SetDefense(int gameID, int midifiedDefense)
        {
            MyExceptions<T>.InvalidGameIDException(gameID, this.gameID, "ID incorrecto. No puede modificar la defensa del monstruo " + this.name + ".");
            this.defense = midifiedDefense;
            return;
        }





        //public T condicionesDeEvolucion, ***EXTRA***
        // ***CONDICION FENIX*** (exigencias que pide la carta a cambio de su resucitación: sacrificar 3 monstruos, estar en un campo específico, tener en juego 10 cartas x...)
        // Valorar que al resucitar una carta su estado sea ***"VETERANO"*** lo que se traduce a una cierta inmunidad hacia la carta que la envió al cementerio 


    }