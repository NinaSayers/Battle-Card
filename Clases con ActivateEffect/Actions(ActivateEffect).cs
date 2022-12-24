using ProeliumEngine;
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

            // List<Card> oldDeck = new List<Card>(state.Table.GetDeck(playerID).Count);
            List<Card> oldDeck = state.Table.GetDeck(playerID);
            List<Card> newDeck = new List<Card>(oldDeck.Count);
            Random random = new Random();
            while (oldDeck.Count > 0)
            {
                int index = random.Next(0, oldDeck.Count);
                newDeck.Add(oldDeck[index]);
                oldDeck.RemoveAt(index);
            }
            State newState = new State(state.GameTurns, state.TurnsByPlayer, state.ActualPhase, state.Players, state.Hands, state.YaAtacó, state.Table, this.gameID, state.LifePoints);
            newState.Table.SetDeck(playerID, gameID, newDeck);
            return newState;
        }

        public State Invoke(Card card, int playerID, int gameID, State state) //Activar efecto en caso de ser carta mágica**** OJO, BARRANCO, PELIGRO
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
                newState.Table.AddMonsterCard(playerID, this.gameID, (card as MonsterCard)!);
                newState.AddYaAtacó(this.gameID, playerID);
            }
            if (card is MagicCard)
            {
                int actualCount = state.Table.GetMagicCardsInvokeds(playerID).Count;
                int maximumCount = state.Table.MaximumCardsInvokeds;
                MyExceptions.FullInvokedCardsException(actualCount, maximumCount, "Máximo de cartas alcanzado. No se pueden invocar más cartas mágicas.");
                MyExceptions.NoFoundedCardException(state.GetHand(playerID), card, "No se pueden invocar cartas que no estén en la mano del jugador.");
                newState.RemoveCardsToHand(this.gameID, playerID, card);
                newState.Table.AddMagicCard(playerID, this.gameID, (card as MagicCard)!);
            }
            if (card is FieldCard)
            {
                MyExceptions.NoFoundedCardException(state.GetHand(playerID), card, "No se pueden invocar cartas que no estén en la mano del jugador.");
                newState.Table.SetFieldCard(this.gameID, (card as FieldCard)!);
            }
            return newState;
        }

        public State ActivateEffect(Card card, int playerID, int gameID, State state)
        {
            MyExceptions.InvalidGameIDException(gameID, this.gameID, "ID inconrrecto. No puede activar el efecto de ninguna carta.");
            MyExceptions.InvalidPlayerIDException(playerID, state.Table.Decks.Count - 1);
            State newState = new State(state.GameTurns, state.TurnsByPlayer, state.ActualPhase, state.Players, state.Hands, state.YaAtacó, state.Table, this.gameID, state.LifePoints);
            if (card is MonsterCard)
            {
                MyExceptions.NoFoundedCardException(state.Table.GetMonsterCardsInvokeds(playerID), card, "No se puede activar el efecto de cartas que no han sido invocadas.");
                int index = state.Table.GetMonsterCardsInvokeds(playerID).IndexOf(card);
                newState.Table.MonsterCardsInvokeds[playerID][index].EffectExecute(newState, playerID);
            }
            if (card is MagicCard)
            {
                MyExceptions.NoFoundedCardException(state.Table.GetMagicCardsInvokeds(playerID), card, "No se puede activar el efecto de cartas que no han sido invocadas.");
                int index = state.Table.GetMagicCardsInvokeds(playerID).IndexOf(card);
                newState.Table.MagicCardsInvokeds[playerID][index].EffectExecute(newState, playerID);
            }
            if (card is FieldCard)//Considerar quién invocó la carta para saber quién puede activar el efecto durante el juego
            {
                MyExceptions.NoFoundedCardException(state.Table.FieldCard, card, "No se puede activar el efecto de cartas que no han sido invocadas.");
                newState.Table.SetFieldCard(this.gameID, (card as FieldCard)!);
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

            for (int i = 0; i < state.Table.MonsterCardsInvokeds.Count; i++)
            {
                List<Card> invokedMonsterCards = state.Table.MonsterCardsInvokeds[i];
                if (newState.Table.MonsterCardsInvokeds.IndexOf(invokedMonsterCards) != attackantPlayerID)
                {
                    MyExceptions.NoFoundedCardException(invokedMonsterCards, cardAtacada, "Esta carta no está invocada por el jugador contrario.");
                    for (int j = 0; j < invokedMonsterCards.Count; j++)
                    {
                        var monsterCard = invokedMonsterCards[j];
                        if ((monsterCard == cardAtacada) && (!yaAtacó))
                        {
                            yaAtacó = true;
                            newState.MarkYaAtacó(this.gameID, attackantPlayerID, cardAtacante);
                            if ((monsterCard as MonsterCard)!.Defense <= 0)
                            {
                                newState.Table.AddCardToCemetery(this.gameID, monsterCard);
                                newState.RemoveYaAtacó(this.gameID, newState.Table.MonsterCardsInvokeds.IndexOf(invokedMonsterCards));
                                invokedMonsterCards.Remove(monsterCard);

                            }
                            else
                            {
                                float damage = (cardAtacante as MonsterCard)!.Attack * 1000 / (monsterCard as MonsterCard)!.Defense;
                                (monsterCard as MonsterCard)!.SetLife(this.gameID, (monsterCard as MonsterCard)!.Life - damage);
                                if ((monsterCard as MonsterCard)!.Life <= 0)
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
            if (playerID == 0)
            {
                newState.SetLifePoints(gameID, 1, state.GetLifePoints(1) - 1);

            }
            else
            {
                newState.SetLifePoints(gameID, 0, state.GetLifePoints(0) - 1);
            }
            newState.MarkYaAtacó(gameID, playerID, attackantCard);
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