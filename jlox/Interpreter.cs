public class Interpreter : Expr.Visitor<object>{
    public object VisitLiteralExpr(Expr.Literal expr){
        return expr.value;
    }

    public object VisitGroupingExpr(Expr.Grouping expr){
        return Evaluate(expr.expression);
    }

    private object Evaluate(Expr expr){
        return expr.Accept(this);
    }

    public object VisitBinaryExpr(Expr.Binary expr){
        object left = Evaluate(expr.left);
        object right = Evaluate(expr.right);
        switch (expr.op.type){
            case TokenType.MINUS: 
                CheckNumberOperands(expr.op, left, right);
                return (double)left - (double)right;
            case TokenType.PLUS:
                if (left is double l && right is double r){
                    return l + r;
                }
                if (left is string ls && right is string rs){
                    return ls + rs;
                }
                throw new RuntimeError(expr.op,"Operands must be two numbers or two strings.");
            case TokenType.SLASH: 
                CheckNumberOperands(expr.op, left, right);
                return (double)left / (double)right;
            case TokenType.STAR: 
                CheckNumberOperands(expr.op, left, right);
                return (double)left * (double)right;
            case TokenType.GREATER: 
                CheckNumberOperands(expr.op, left, right);
                return (double)left > (double)right;
            case TokenType.GREATER_EQUAL: 
                CheckNumberOperands(expr.op, left, right);
                return (double)left >= (double)right;
            case TokenType.LESS: 
                CheckNumberOperands(expr.op, left, right);
                return (double)left < (double)right;
            case TokenType.LESS_EQUAL: 
                CheckNumberOperands(expr.op, left, right);
                return (double)left <= (double)right;
            case TokenType.BANG_EQUAL: return !IsEqual(left,right);
            case TokenType.EQUAL_EQUAL: return IsEqual(left,right);
        }
        return null;
    }

    public object VisitUnaryExpr(Expr.Unary expr){
        object right = Evaluate(expr.right);
        switch (expr.op.type){
            case TokenType.MINUS: 
                CheckNumberOperand(expr.op,right);
                return -(double)right;
            case TokenType.BANG: return IsTruthy(right);
        }
        return null;
    }

    private void CheckNumberOperand(Token op, object operand){
        if (operand is double)
            return;
        throw new RuntimeError(op, "Operand must be a number.");
    }

    private void CheckNumberOperands(Token op, object left, object right){
        if (left is double && right is double) return;
        throw new RuntimeError(op, "Operands must be numbers.");
    }

    private bool IsTruthy(object obj){
        if (obj == null) return false;
        if (obj is bool booleanValue) return booleanValue;
        return true;
    }

    private bool IsEqual(object a, object b){
        if (a == null && b == null) return true;
        if (a == null) return false;
        return Equals(a,b);
    }

    public void Interpret(Expr expression)
    {
        try
        {
            object value = Evaluate(expression);
            Console.WriteLine(Stringify(value));
        }
        catch (RuntimeError error)
        {
            Lox.RuntimeError(error);
        }
    }

    private string Stringify(object obj)
    {
        if (obj == null) return "nil";
        if (obj is double)
        {
            string text = obj.ToString();
            if (text.EndsWith(".0"))
            {
                text = text.Substring(0,text.Length - 2);
            }
            return text;
        }
        return obj.ToString();
    }

}
