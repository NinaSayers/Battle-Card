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

}

public class Add : BinaryExpression
{
    public Add(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return left + right;
    }
}

public class Substract : BinaryExpression
{
    public Substract(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return left - right;
    }
}
public class Multiply : BinaryExpression
{
    public Multiply(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return left * right;
    }
}
public class Divide : BinaryExpression
{
    public Divide(Expression left, Expression right) : base(left, right){}

    protected override double Evaluate(double left, double right)
    {
        return left / right;
    }
}