using ProeliumEngine;
namespace ExpEvaluator;
public class Programa
{
    public static State state;

    public static Dictionary<string, double> doubleVar;
    public static Dictionary<string, string> stringVar;
    public static int PlayerID;
    public static void UpdateState()
    {
        state.LifePoints[0] = (float) doubleVar["Player1.life"];
        state.LifePoints[1] = (float) doubleVar["Player2.life"];
        (state.Table.MonsterCardsInvokeds[0][0] as MonsterCard)!.SetAttack(1, (float)doubleVar["Player1.Monsters[0].Attack"]);
        (state.Table.MonsterCardsInvokeds[0][1] as MonsterCard)!.SetAttack(1, (float)doubleVar["Player1.Monsters[1].Attack"]);
        (state.Table.MonsterCardsInvokeds[0][2] as MonsterCard)!.SetAttack(1, (float)doubleVar["Player1.Monsters[2].Attack"]);
        (state.Table.MonsterCardsInvokeds[1][0] as MonsterCard)!.SetAttack(1, (float)doubleVar["Player2.Monsters[0].Attack"]);
        (state.Table.MonsterCardsInvokeds[1][1] as MonsterCard)!.SetAttack(1, (float)doubleVar["Player2.Monsters[1].Attack"]);
        (state.Table.MonsterCardsInvokeds[1][2] as MonsterCard)!.SetAttack(1, (float)doubleVar["Player2.Monsters[2].Attack"]);
        (state.Table.MonsterCardsInvokeds[0][0] as MonsterCard)!.SetDefense(1, (float)doubleVar["Player1.Monsters[0].Defense"]);
        (state.Table.MonsterCardsInvokeds[0][1] as MonsterCard)!.SetDefense(1, (float)doubleVar["Player1.Monsters[1].Defense"]);
        (state.Table.MonsterCardsInvokeds[0][2] as MonsterCard)!.SetDefense(1, (float)doubleVar["Player1.Monsters[2].Defense"]);
        (state.Table.MonsterCardsInvokeds[1][0] as MonsterCard)!.SetDefense(1, (float)doubleVar["Player2.Monsters[0].Defense"]);
        (state.Table.MonsterCardsInvokeds[1][1] as MonsterCard)!.SetDefense(1, (float)doubleVar["Player2.Monsters[1].Defense"]);
        (state.Table.MonsterCardsInvokeds[1][2] as MonsterCard)!.SetDefense(1, (float)doubleVar["Player2.Monsters[2].Defense"]);
        (state.Table.MonsterCardsInvokeds[0][0] as MonsterCard)!.SetLife(1, (float)doubleVar["Player1.Monsters[0].Life"]);
        (state.Table.MonsterCardsInvokeds[0][1] as MonsterCard)!.SetLife(1, (float)doubleVar["Player1.Monsters[1].Life"]);
        (state.Table.MonsterCardsInvokeds[0][2] as MonsterCard)!.SetLife(1, (float)doubleVar["Player1.Monsters[2].Life"]);
        (state.Table.MonsterCardsInvokeds[1][0] as MonsterCard)!.SetLife(1, (float)doubleVar["Player2.Monsters[0].Life"]);
        (state.Table.MonsterCardsInvokeds[1][1] as MonsterCard)!.SetLife(1, (float)doubleVar["Player2.Monsters[1].Life"]);
        (state.Table.MonsterCardsInvokeds[1][2] as MonsterCard)!.SetLife(1, (float)doubleVar["Player2.Monsters[2].Life"]);

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
            ,{"Player1.Monsters[0].Attack", ((state.Table.MonsterCardsInvokeds[0][0] as MonsterCard)!).Attack}
            ,{"Player1.Monsters[1].Attack", ((state.Table.MonsterCardsInvokeds[0][1] as MonsterCard)!).Attack}
            ,{"Player1.Monsters[2].Attack", ((state.Table.MonsterCardsInvokeds[0][2] as MonsterCard)!).Attack}
            ,{"Player2.Monsters[0].Attack", ((state.Table.MonsterCardsInvokeds[1][0] as MonsterCard)!).Attack}
            ,{"Player2.Monsters[1].Attack", ((state.Table.MonsterCardsInvokeds[1][1] as MonsterCard)!).Attack}
            ,{"Player2.Monsters[2].Attack", ((state.Table.MonsterCardsInvokeds[1][2] as MonsterCard)!).Attack}
            ,{"Player1.Monsters[0].Defense", ((state.Table.MonsterCardsInvokeds[0][0] as MonsterCard)!).Defense}
            ,{"Player1.Monsters[1].Defense", ((state.Table.MonsterCardsInvokeds[0][1] as MonsterCard)!).Defense}
            ,{"Player1.Monsters[2].Defense", ((state.Table.MonsterCardsInvokeds[0][2] as MonsterCard)!).Defense}
            ,{"Player2.Monsters[0].Defense", ((state.Table.MonsterCardsInvokeds[1][0] as MonsterCard)!).Defense}
            ,{"Player2.Monsters[1].Defense", ((state.Table.MonsterCardsInvokeds[1][1] as MonsterCard)!).Defense}
            ,{"Player2.Monsters[2].Defense", ((state.Table.MonsterCardsInvokeds[1][2] as MonsterCard)!).Defense}
            ,{"Player1.Monsters[0].Life", ((state.Table.MonsterCardsInvokeds[0][0] as MonsterCard)!).Life}
            ,{"Player1.Monsters[1].Life", ((state.Table.MonsterCardsInvokeds[0][1] as MonsterCard)!).Life}
            ,{"Player1.Monsters[2].Life", ((state.Table.MonsterCardsInvokeds[0][2] as MonsterCard)!).Life}
            ,{"Player2.Monsters[0].Life", ((state.Table.MonsterCardsInvokeds[1][0] as MonsterCard)!).Life}
            ,{"Player2.Monsters[1].Life", ((state.Table.MonsterCardsInvokeds[1][1] as MonsterCard)!).Life}
            ,{"Player2.Monsters[2].Life", ((state.Table.MonsterCardsInvokeds[1][2] as MonsterCard)!).Life}

        };

        Programa.stringVar = new Dictionary<string, string>
        {
            {"Player1.Monsters[0].Name", state.Table.MonsterCardsInvokeds[0][0].Name}
            ,{"Player1.Monsters[1].Name", state.Table.MonsterCardsInvokeds[0][1].Name}
            ,{"Player1.Monsters[2].Name", state.Table.MonsterCardsInvokeds[0][2].Name}
            ,{"Player2.Monsters[0].Name", state.Table.MonsterCardsInvokeds[1][0].Name}
            ,{"Player2.Monsters[1].Name", state.Table.MonsterCardsInvokeds[1][1].Name}
            ,{"Player2.Monsters[2].Name", state.Table.MonsterCardsInvokeds[1][2].Name}

        };
        Execute();
        Programa.UpdateState();
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