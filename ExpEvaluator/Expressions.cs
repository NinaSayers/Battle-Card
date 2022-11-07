namespace ExpEvaluator;

public abstract class Expression
{
    public abstract double Evaluate();
}

public abstract class BinaryExpression : Expression
{
    protected Expression left;
    protected Expression right;

    public BinaryExpression(Expression left, Expression right)
    {
        this.left = left;
        this.right = right;
    }

    public override double Evaluate()
    {
        double leftVal = left.Evaluate();
        double rightVal = right.Evaluate();
        
        return this.Evaluate(leftVal, rightVal);
    }

    protected abstract double Evaluate(double leftVal, double rightVal);
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

public class Add : BinaryExpression
{
    public Add(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return left + right;
    }
    public override string ToString()
    {
        return $"({left.ToString()}) + ({right.ToString()})";
    }
}

public class Substract : BinaryExpression
{
    public Substract(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return left - right;
    }
    public override string ToString()
    {
        return $"({left.ToString()}) - ({right.ToString()})";
    }
}
public class Multiply : BinaryExpression
{
    public Multiply(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return left * right;
    }
    public override string ToString()
    {
        return $"({left.ToString()}) * ({right.ToString()})";
    }
}
public class Divide : BinaryExpression
{
    public Divide(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return left / right;
    }
    public override string ToString()
    {
        return $"({left.ToString()}) / ({right.ToString()})";
    }
}
public class Assign : BinaryExpression
{
    public Assign(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return right;
    }
    public override string ToString()
    {
        return $"({left.ToString()}) = ({right.ToString()})";
    }
}
public class BooleanExpression
{
    public Expression left;
    public Expression right;
    public string op;

    public BooleanExpression(Expression left, Expression right, string op)
    {
        this.left = left;
        this.right = right;
        this.op = op;
    }

    public bool Evaluate()
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
}