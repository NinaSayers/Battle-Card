using ProeliumEngine;
namespace ExpEvaluator;
public class Programa
{
    public static State state;

    public static Dictionary<string, double> doubleVar = new Dictionary<string, double>();
    public static Dictionary<string, string> stringVar = new Dictionary<string, string>();
    public static int PlayerID;
    public static void UpdateState()
    {
        state.LifePoints[0] = (float) doubleVar["Player1.life"];
        state.LifePoints[1] = (float) doubleVar["Player2.life"];
    }
}
public class Statement
{
    public State Execute(State state, int playerID)
    {
        Programa.state = state;
        Programa.PlayerID = playerID;
        Programa.doubleVar = new Dictionary<string, double>
        {
            {"Player1.life", state.LifePoints[0]}
            ,{"Player2.life", state.LifePoints[1]}
            ,{"PlayerID", playerID}
        };

        Execute();
        Programa.UpdateState();
        return Programa.state;

    }

    public State Execute(State state, int playerID, MonsterCard card)
    {
        Programa.state = state;
        Programa.PlayerID = playerID;
        Programa.doubleVar = new Dictionary<string, double>
        {
            {"Player1.life", state.LifePoints[0]}
            ,{"Player2.life", state.LifePoints[1]}
            ,{"PlayerID", playerID}
            ,{"this.Attack", card.Attack}
            ,{"this.Defense", card.Defense}
            ,{"this.Life", card.Life}
        };

        Execute();
        Programa.UpdateState();
        foreach(var monster in state.Table.GetMonsterCardsInvokeds(playerID))
        {
            if(monster == card)
            {
                (monster as MonsterCard)!.SetAttack(1, (float)Programa.doubleVar["this.Attack"]);
                (monster as MonsterCard)!.SetDefense(1, (float)Programa.doubleVar["this.Defense"]);
                (monster as MonsterCard)!.SetLife(1, (float)Programa.doubleVar["this.Life"]);
                break;
            }
        }
        

        return Programa.state;
    }
    public virtual void Execute()
    {
        return;
    }
}

public class Assign : Statement
{
    public string left;

    public Expression right;

    public string op;

    public Assign(string left, Expression right, string op)
    {
        this.left = left;
        this.right = right;
        this.op = op;
    }

    public override void Execute()
    {
        double val = right.Evaluate();
        switch (op)
        {
            case "=":
                Programa.doubleVar[left] = val;
                break;
            case "+=":
                Programa.doubleVar[left] += val;
                break;
            case "-=":
                Programa.doubleVar[left] -= val;
                break;
            case "*=":
                Programa.doubleVar[left] *= val;
                break;
            case "/=":
                Programa.doubleVar[left] /= val;
                break;
            default:
                throw new Exception("Invalid operator");
        }
    }
    public override string ToString()
    {
        return $"{left.ToString()} " + op + $" {right.ToString()}";
    }
}

public class Conditional : Statement
{
    public BooleanExpression Condition;
    public Statement Action;

    public Conditional(BooleanExpression Condition, Statement Action)
    {
        this.Condition = Condition;
        this.Action = Action;
    }

    public override void Execute()
    {
        if(Condition.Evaluate())
        {
            Action.Execute();
        }
    }

    public override string ToString()
    {
        return $"if {Condition.ToString()} then {Action.ToString()}";
    }
}

public class Concat : Statement
{
    public List<Statement> statements;

    public Concat(List<Statement> statements)
    {
        this.statements = statements;
    }
    public override void Execute()
    {
        foreach(var exp in statements)
        {
            exp.Execute();
        }
    }
    public override string ToString()
    {
        return $"";
    }
}

public class Draw : Statement
{
    public Expression expression;

    public Draw(Expression expression)
    {
        this.expression = expression;
    }

    public override void Execute()
    {
        double times = expression.Evaluate();
        Actions draw = new Actions(1);

        for (int i = 0; i < times; i++)
        {
            Programa.state = draw.Draw(Programa.PlayerID, 1, Programa.state);
        }
    }

    public override string ToString()
    {
        return "Draw " + expression.ToString();
    }
}

public class Mix : Statement
{

    public Mix(){}

    public override void Execute()
    {
        Actions mix = new Actions(1);

        Programa.state = mix.Mix(Programa.PlayerID, 1, Programa.state);
    }

    public override string ToString()
    {
        return "Mix";
    }
}

// Define the while Statement

public class While : Statement
{
    public BooleanExpression Condition;
    public Statement Action;

    public While(BooleanExpression Condition, Statement Action)
    {
        this.Condition = Condition;
        this.Action = Action;
    }

    public override void Execute()
    {
        while(Condition.Evaluate())
        {
            Action.Execute();
        }
    }

    public override string ToString()
    {
        return $"while {Condition.ToString()} do {Action.ToString()}";
    }
}