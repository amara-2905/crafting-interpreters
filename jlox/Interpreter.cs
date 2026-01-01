public class Interpreter : Expr.Visitor<object>, Stmt.Visitor<object>{
    public readonly Environment globals;
    private Environment environment;
    private readonly Dictionary<Expr,int> locals = new Dictionary<Expr, int>();

    public Interpreter()
    {
        globals = new Environment();
        environment = globals;
        globals.Define("clock", new NativeFunction(0,(interpreter, arguments) => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0));
    }
    public object VisitLiteralExpr(Expr.Literal expr){
        return expr.value;
    }

    public object VisitLogicalExpr(Expr.Logical expr){
        object left = Evaluate(expr.left);
        if(expr.op.type == TokenType.OR){
            if (IsTruthy(left)) return left;
        } else{
            if(!IsTruthy(left)) return left;
        }
        return Evaluate(expr.right);
    }

    public object VisitGroupingExpr(Expr.Grouping expr){
        return Evaluate(expr.expression);
    }

    private object Evaluate(Expr expr){
        return expr.Accept(this);
    }

    private void Execute(Stmt stmt){
        stmt.Accept(this);
    }

    public void Resolve(Expr expr, int depth)
    {
        locals[expr] = depth;
    }

    public void ExecuteBlock(List<Stmt> statements, Environment environment){
        Environment previous = this.environment;
        try{
            this.environment = environment;
            foreach(Stmt statement in statements){
                Execute(statement);
            }
        }
        finally{
            this.environment = previous;
        }
    }

    public object VisitBlockStmt(Stmt.Block stmt){
        ExecuteBlock(stmt.statements,new Environment(environment));
        return null;
    }

    public object VisitExpressionStmt(Stmt.Expression stmt){
        Evaluate(stmt.expression);
        return null;
    }

    public object VisitFunctionStmt(Stmt.Function stmt){
        LoxFunction function = new LoxFunction(stmt,environment);
        environment.Define(stmt.name.lexeme, function);
        return null;
    }

    public object VisitIfStmt(Stmt.If stmt){
        if (IsTruthy(Evaluate(stmt.Condition))){
            Execute(stmt.thenBranch);
        } else if (stmt.elseBranch != null){
            Execute(stmt.elseBranch);
        }
        return null;
    }

    public object VisitPrintStmt(Stmt.Print stmt){
        object Value = Evaluate(stmt.expression);
        Console.WriteLine(Stringify(Value));
        return null;
    }

    public object VisitReturnStmt(Stmt.Return stmt){
        object value = null; 
        if (stmt.value != null) value = Evaluate(stmt.value);
        throw new Return(value);
    }
    public object VisitVarStmt(Stmt.Var stmt){
        object value = null;
        if (stmt.Initializer != null){
            value = Evaluate(stmt.Initializer);
        }
        environment.Define(stmt.name.lexeme,value);
        return null;
    }

    public object VisitWhileStmt(Stmt.While stmt){
        while (IsTruthy(Evaluate(stmt.condition))){
            Execute(stmt.body);
        }
        return null;
    }

    public object VisitAssignExpr(Expr.Assign expr){
        object value = Evaluate(expr.value);
        int Distance = locals[expr];
        if (Distance != null){
            environment.AssignAt(Distance,expr.name,value);
        }
        else{
            globals.Assign(expr.name,value);
        }
        return value;
    }

    public object VisitVariableExpr(Expr.Variable expr){
        return LookupVariable(expr.name, expr);
    }

    private object LookupVariable(Token name, Expr expr){
        if (locals.TryGetValue(expr, out int Distance)){
            return environment.GetAt(Distance, name.lexeme);
        }
        else{
            return globals.Get(name);
        }
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

    public object VisitCallExpr(Expr.Call expr)
    {
        object callee = Evaluate(expr.callee);
        List<object> arguments = new List<object>();
        foreach(Expr argument in expr.arguments){
            arguments.Add(Evaluate(argument));
        }
        if (!(callee is LoxCallable)){
            throw new RuntimeError(expr.paren, "Can only call functions and classes.");
        }
        LoxCallable Function = (LoxCallable)callee;
        if (arguments.Count() != Function.Arity()){
            throw new RuntimeError(expr.paren, "Expected " + Function.Arity() + " arguments but got " + arguments.Count() + ".");
        }
        return Function.Call(this,arguments);
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

    public void Interpret(List<Stmt> statements){
        try{
            foreach (Stmt statement in statements){
                Execute(statement);
            }
        }
        catch (RuntimeError error){
            Lox.RuntimeError(error);
        }
    }

    private string Stringify(object obj){
        if (obj == null) return "nil";
        if (obj is double){
            string text = obj.ToString();
            if (text.EndsWith(".0")){
                text = text.Substring(0,text.Length - 2);
            }
            return text;
        }
        return obj.ToString();
    }

}
