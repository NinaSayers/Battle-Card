namespace BattleCardsEngine
{
    public static class MyExceptions
    {
        public static void InvalidPlayerIDException(int playerID, int maxCount)
        {
            if (playerID < 0 || playerID >= maxCount)
            {
                throw new Exception("Invalid player ID. Try again");
            }
            return;
        }
        public static void InvalidGameIDException(int gameID, int originalGameID, string message)
        {
            if (gameID != originalGameID)
            {
                throw new Exception(message);
            }
            return;
        }
        // public static void InvalidTypOfCardException(TypeOfCard theType, TypeOfCard originalType, string message)
        // {
        //     if (theType != originalType)
        //     {
        //         throw new Exception(message);
        //     }
        //     return;
        // }
        public static void EmptyDeckException(List<Card> deck)
        {
            if (deck.Count == 0)
            {
                throw new Exception("No hay cartas en el mazo.");
            }
            return;
        }
        public static void FullInvokedCardsException(int actualCount, int maxCount, string message)
        {
            if (actualCount == maxCount)
            {
                throw new Exception(message);
            }
            return;
        }
        public static void NoFoundedCardException(List<Card> collection, Card card, string message)
        {
            if (!collection.Contains(card))
            {
                throw new Exception(message);
            }
            return;
        }
        public static void InvalidAttackantCardException(List<Card> invokedsCardsCollection, MonsterCard card)
        {
            if (!invokedsCardsCollection.Contains(card))
            {
                throw new Exception("No se puede atacar con cartas que no hayan sido invocadas.");
            }
            return;
        }
        public static void InvalidInvokationOfMethodException(bool invokationCondition, string message)
        {
            if (!invokationCondition) throw new Exception(message);
            return;
        }
    }
}
