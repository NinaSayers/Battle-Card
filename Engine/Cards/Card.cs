using System.Collections;
using System.Collections.Generic;
using ExpEvaluator;
namespace Cards;
public interface IEffect<T>
{

}
public interface ICard<T> : IEffect<T>
{
    
}

public abstract class Card
{
    public abstract void EffectExecute();
    
}
