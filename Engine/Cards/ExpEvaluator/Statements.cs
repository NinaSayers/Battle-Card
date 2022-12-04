namespace ExpEvaluator;

public class Statement
{
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
                Program.variables[left] = val;
                break;
            case "+=":
                Program.variables[left] += val;
                break;
            case "-=":
                Program.variables[left] -= val;
                break;
            case "*=":
                Program.variables[left] *= val;
                break;
            case "/=":
                Program.variables[left] /= val;
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