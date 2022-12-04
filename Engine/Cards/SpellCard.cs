using ExpEvaluator;
namespace Cards
{
    public class SpellCard : Card
    {
        public string? CardName{ get; set; }
        public string? Description { get; set;}
        public string? strEffect { get; set; }

        public Statement Effect { get {return Parser.Construct(strEffect);} private set{} }

        public override void EffectExecute()
        {
            Effect.Execute();
        }
    }
}