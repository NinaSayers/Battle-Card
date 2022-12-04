namespace ExpEvaluator;

public abstract class Expression : Statement
{
    public abstract double Evaluate();
}

public class BinaryExpression : Expression
{
    public Expression left;
    public Expression right;

    public string op;

    public BinaryExpression(Expression left, Expression right,string op)
    {
        this.left = left;
        this.right = right;
        this.op = op;
    }

    public override double Evaluate()
    {
        double leftVal = left.Evaluate();
        double rightVal = right.Evaluate();
        
        return this.Evaluate(leftVal, rightVal);
    }

    private double Evaluate(double leftVal, double rightVal)
    {
        switch (op)
        {
            case "+":
                return leftVal + rightVal;
            case "-":
                return leftVal - rightVal;
            case "*":
                return leftVal * rightVal;
            case "/":
                return leftVal / rightVal;
            default:
                throw new Exception("Invalid operator");
        }
    }

    public override string ToString()
    {
        return $"({left.ToString()} " + op + $" {right.ToString()})";
    }
}

public class Constant : Expression
{
    private double value;

    public Constant(double value)
    {
        this.value = value;
    }

    public override double Evaluate()
    {
        return value;
    }
    public override string ToString()
    {
        return value.ToString();
    }
}

public class Variable : Expression
{
    private string name;

    public Variable(string name)
    {
        this.name = name;
    }

    public override double Evaluate()
    {
        return Program.variables[name];
    }
    public override string ToString()
    {
        return name;
    }
}

public abstract class BooleanExpression : Statement
{

    public abstract bool Evaluate();

}
public class BinaryBooleanExpression : BooleanExpression
{
    public Expression left;
    public Expression right;
    public string op;

    public BinaryBooleanExpression(Expression left, Expression right, string op)
    {
        this.left = left;
        this.right = right;
        this.op = op;
    }

    public override bool Evaluate()
    {
        double leftVal = left.Evaluate();
        double rightVal = right.Evaluate();
        
        return this.Evaluate(leftVal, rightVal);
    }

    protected bool Evaluate(double leftVal, double rightVal)
    {
        if(op == "<")
        {
            return leftVal < rightVal;
        }
        if(op == ">")
        {
            return leftVal > rightVal;
        }
        if(op == "<=")
        {
            return leftVal <= rightVal;
        }
        if(op == ">=")
        {
            return leftVal >= rightVal;
        }
        if(op == "==")
        {
            return leftVal == rightVal;
        }
        if(op == "!=")
        {
            return leftVal != rightVal;
        }
        throw new Exception("Invalid Operator");
    }
    public override string ToString()
    {
        return $"({left.ToString()} " + op + $" {right.ToString()})";
    }
}

public class BooleanOperator : BooleanExpression
{
    public BooleanExpression left;
    public BooleanExpression right;        
    public string op;

    public BooleanOperator(BooleanExpression left, BooleanExpression right, string op)
    {
        this.left = left;
        this.right = right;
        this.op = op;
    }

    public override bool Evaluate()
    {
        bool leftVal = left.Evaluate();
        bool rightVal = right.Evaluate();
        
        return this.Evaluate(leftVal, rightVal);
    }

    protected bool Evaluate(bool leftVal, bool rightVal)
    {
        if(op == "&&")
        {
            return leftVal && rightVal;
        }
        if(op == "||")
        {
            return leftVal || rightVal;
        }
        throw new Exception("Invalid Operator");
    }
}

public class Bool : BooleanExpression
{
    public bool Value;

    public Bool(string Value)
    {
        if(Value == "true")
        {
            this.Value = true;
        }
        else if(Value == "false")
        {
            this.Value = false;
        }
        
        throw new Exception("Invalid Boolean Value");
    }

    public override bool Evaluate()
    {
        return Value;
    }
}
