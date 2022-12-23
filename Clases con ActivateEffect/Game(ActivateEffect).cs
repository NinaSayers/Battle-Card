using ProeliumEngine;
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
                state.ResetYaAtacó(gameID, player.ID);
                maskAttack = new bool[this.state.Table.GetMonsterCardsInvokeds(player.ID).Count];

                foreach (PhasesEnum phase in this.phases)
                {
                    endPhase = false;
                    if (this.rules.IsEndGame(this.state)) { end = true; break; }
                    if (phase == PhasesEnum.drawPhase)
                    {
                        this.state.SetActualPhase(this.gameID, phase);
                        yield return this.state;
                        this.actions.Draw(player.ID, this.gameID, state);
                        yield return this.state;
                        continue;
                    }
                    while (!endPhase && !endTurn)
                    {
                        this.state.SetActualPhase(this.gameID, phase);
                        yield return this.state;

                        Move jugada = player.Jugada(state);
                        if (jugada.Action == ActionsEnum.endPhase) { endPhase = true; continue; }
                        if (jugada.Action == ActionsEnum.endTurn) { endTurn = true; continue; }
                        if (this.rules.IsValidMove(jugada, phase, player.ID, this.state))
                        {
                            if (jugada.Action == ActionsEnum.endPhase) { endPhase = true; }
                            if (jugada.Action == ActionsEnum.endTurn) { endTurn = true; }
                            else
                            {
                                if (jugada.Action == ActionsEnum.attackCard || jugada.Action == ActionsEnum.attackLifePoints)
                                {
                                    if (IsFullCardAttack(maskAttack)) { endPhase = true; continue; }
                                    ActionToDo(player, jugada, this.state);
                                    yield return state;

                                }
                                if (jugada.Action == ActionsEnum.invoke)
                                {

                                    if (jugada.CardsInTheAction[0].GetType() == typeof(MagicCard))
                                    {
                                        if (this.state.Table.GetMagicCardsInvokeds(player.ID).Count == this.state.Table.MaximumCardsInvokeds) { endPhase = true; continue; }
                                        else
                                        {
                                            ActionToDo(player, jugada, this.state);
                                            yield return this.state;
                                            yield return this.actions.ActivateEffect(jugada.CardsInTheAction[0], player.ID, this.gameID, state);
                                            this.state.Table.AddCardToCemetery(this.gameID, jugada.CardsInTheAction[0]);
                                            this.state.Table.RemoveMagicCard(player.ID, this.gameID, (jugada.CardsInTheAction[0] as MagicCard)!);
                                            yield return this.state;
                                            endPhase = true;
                                            continue;
                                        }
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
                        else throw new Exception("Jugada no válida.");
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
                return;
            case ActionsEnum.invoke:
                this.state = this.actions.Invoke(jugada.CardsInTheAction[0], player.ID, this.gameID, state);
                return;
            case ActionsEnum.attackCard:
                this.state = this.actions.AttackCard(this.gameID, player.ID, jugada.CardsInTheAction[0] as MonsterCard, jugada.CardsInTheAction[1] as MonsterCard, state);
                return;
            case ActionsEnum.attackLifePoints:
                this.state = this.actions.AttackLifePoints(this.gameID, player.ID, jugada.CardsInTheAction[0] as MonsterCard, state);
                return;
            case ActionsEnum.discard:
                this.state = this.actions.Discard(this.gameID, player.ID, jugada.CardsInTheAction[0], state);
                return;
            case ActionsEnum.activateEffect:
                this.state = this.actions.ActivateEffect(jugada.CardsInTheAction[0], player.ID, this.gameID, state);
                return;
        }
    }
    private bool IsFullCardAttack(bool[] cardsQueAtacaron)
    {
        foreach (bool card in cardsQueAtacaron)
        {
            if (card) return true;
        }
        return false;
    }
}