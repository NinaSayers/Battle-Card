namespace ProeliumEngine
{
    public class MyExceptions
    { 
        public static void InvalidPlayerIDException(int playerID, int maxCount)
        {
            if (playerID < 0 || playerID > maxCount)
            {
                throw new InvalidPlayerIDException("Invalid player ID. Try again");
            } 
            return;
        }
        public static void InvalidGameIDException(int gameID, int originalGameID, string message)
        {
            if (gameID != originalGameID)
            {
                throw new InvalidGameIDException(message);
            }
            return;
        }
        public static void InvalidTypOfCardException(Type theType, Type originalType, string message)
        {
            if (theType != originalType)
            {
                throw new InvalidTypOfCardException(message);
            }
            return;
        }
        public static void EmptyCollectionCardsException(List<Card> collection, string message)//"No hay cartas en el mazo."
        {
            if (collection.Count == 0)
            {
                throw new EmptyCollectionCardsException(message);
            }
            return;
        }
        public static void FullInvokedCardsException(int actualCount, int maxCount, string message)
        {
            if (actualCount == maxCount)
            {
                throw new FullInvokedCardsException(message);
            }
            return;
        }
        public static void NoFoundedCardException(List<Card> collection, Card card, string message)
        {
            if (!collection.Contains(card))
            {
                throw new NoFoundedCardException(message);
            }
            return;
        }
        public static void InvalidAttackantCardException(List<Card> invokedsCardsCollection, MonsterCard card)
        {
            if (!invokedsCardsCollection.Contains(card))
            {
                throw new InvalidAttackantCardException("No se puede atacar con cartas que no hayan sido invocadas.");
            }
            return;
        }
        public static void InvalidInvokationOfMethodException(bool invokationCondition, string message)
        {
            if (!invokationCondition) throw new InvalidInvokationOfMethodException(message);
            return;
        }
    }

    public class InvalidPlayerIDException : Exception
    {
        public InvalidPlayerIDException(string message) : base(message) { }
    }

    public class InvalidGameIDException : Exception
    {
        public InvalidGameIDException(string message) : base(message) { }
    }
    public class InvalidTypOfCardException : Exception
    {
        public InvalidTypOfCardException(string message) : base(message) { }
    }
    public class EmptyCollectionCardsException : Exception
    {
        public EmptyCollectionCardsException(string message) : base(message) { }
    }
    public class FullInvokedCardsException : Exception
    {
        public FullInvokedCardsException(string message) : base(message) { }
    }
    public class NoFoundedCardException : Exception
    {
        public NoFoundedCardException(string message) : base(message) { }
    }

    public class InvalidAttackantCardException : Exception
    {
        public InvalidAttackantCardException(string message) : base(message) { }
    }
    public class InvalidInvokationOfMethodException : Exception
    {
        public InvalidInvokationOfMethodException(string message) : base(message) { }
    }
}
