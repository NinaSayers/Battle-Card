using System.Collections;
using System.Collections.Generic;
using ExpEvaluator;
namespace Cards;

public abstract class CardInfo
{
    
}

public class MonsterCardInfo : CardInfo
{
    public string? CardName {get; set;}
    public int Health {get; set;}
    public int Attack {get; set;}
    public int Defense {get; set;}
    public string? strEffect {get; set;}
}

 public class SpellCardInfo : CardInfo
{
    public string? CardName{ get; set; }
    public string? strEffect { get; set; }
}
