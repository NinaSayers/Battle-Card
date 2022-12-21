using ProeliumEngine;
public class Greedy : IStrategy
{
    public Move Play(State state, Rules rules, int playerID)
    {
        bool yaInvocó = false;
        if (state.ActualPhase == PhasesEnum.mainPhase)
        {
            if (state.GetHand(playerID).Count! > 0)
            {
                foreach (Card card in state.GetHand(playerID))
                {
                    List<Card> invokedCard = new List<Card> { card };
                    if (rules.IsValidInvoke(playerID, state, new Move(ActionsEnum.invoke, invokedCard)))
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
                    if (cards.Count == 0)
                    {
                        for (int i = 0; i < state.Table.GetMonsterCardsInvokeds(playerID).Count; i++)
                        {
                            if (!state.GetYaAtacó(playerID)[i])
                            {
                                List<Card> actionCards = new List<Card> { state.Table.GetMonsterCardsInvokeds(playerID)[i] };
                                if (rules.IsValidAttackPointsLife(state, playerID, new Move(ActionsEnum.attackLifePoints, actionCards))) return new Move(ActionsEnum.attackLifePoints, actionCards);
                                else continue;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < state.Table.GetMonsterCardsInvokeds(playerID).Count; i++)
                        {
                            if (!state.GetYaAtacó(playerID)[i])
                            {
                                List<Card> actionCards = new List<Card> { state.Table.GetMonsterCardsInvokeds(playerID)[i], cards[cards.Count - 1] };
                                if (rules.IsValidAttack(state, playerID, new Move(ActionsEnum.attackCard, actionCards))) return new Move(ActionsEnum.attackCard, actionCards);
                            }
                        }
                    }
                    if (rules.IsValidMove(new Move(ActionsEnum.endPhase), state.ActualPhase, playerID, state)) return new Move(ActionsEnum.endPhase);
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
                    if (rules.IsValidInvoke(playerID, state, new Move(ActionsEnum.invoke, actionCard)))
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