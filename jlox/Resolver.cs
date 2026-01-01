public class Resolver : Expr.Visitor<object>, Stmt.Visitor<object>{
    private readonly Interpreter interpreter;
    private readonly Stack<Dictionary<string,bool>> scopes = new Stack<Dictionary<string, bool>>();
    
    private FunctionType currentFunction = FunctionType.NONE;
    public Resolver(Interpreter interpreter){
        this.interpreter = interpreter;
    }

    private enum FunctionType{NONE, FUNCTION}

    public void Resolve(List<Stmt> statements){
        foreach(Stmt statement in statements){
            Resolve(statement);
        }
    }

    public object VisitBlockStmt(Stmt.Block stmt){
        BeginScope();
        Resolve(stmt.statements);
        EndScope();
        return null;
    }

    private void Resolve(Stmt stmt){
        stmt.Accept(this);
    }

    private void Resolve(Expr expr){
        expr.Accept(this);
    }

    private void BeginScope(){
        scopes.Push(new Dictionary<string, bool>());
    }

    private void EndScope(){
        scopes.Pop();
    }

    public object VisitVarStmt(Stmt.Var stmt){
        Declare(stmt.name);
        if (stmt.Initializer != null){
            Resolve(stmt.Initializer);
        }
        Define(stmt.name);
        return null;
    }

    private void Declare(Token name){
        if (scopes.Count() == 0) return;
        Dictionary<string,bool> scope = scopes.Peek();
        if (scope.ContainsKey(name.lexeme)){
            Lox.Error(name, "Already a variable with this name in this scope.");
            return;
        }
        scope[name.lexeme] = false;
    }

    private void Define(Token name){
        if (scopes.Count() == 0) return;
        scopes.Peek()[name.lexeme] = true;
    }

    public object VisitVariableExpr(Expr.Variable expr)
    {
        if (!(scopes.Count() == 0) && scopes.Peek().ContainsKey(expr.name.lexeme) && scopes.Peek()[expr.name.lexeme] == false)
        {
            Lox.Error(expr.name,"Can't read local variable in its own initializer.");
        }
        ResolveLocal(expr,expr.name);
        return null;
    }

    private void ResolveLocal(Expr expr, Token name){
        int depth = 0;
        foreach (var scope in scopes){ // top → bottom{
            if (scope.ContainsKey(name.lexeme)){
                interpreter.Resolve(expr, depth);
                return;
            }
            depth++;
        }
    }

    public object VisitAssignExpr(Expr.Assign expr)
    {
        Resolve(expr.value);
        ResolveLocal(expr,expr.name);
        return null;
    }

    public object VisitBinaryExpr(Expr.Binary expr)
    {
        Resolve(expr.left);
        Resolve(expr.right);
        return null;
    }

    public object VisitFunctionStmt(Stmt.Function stmt)
    {
        Declare(stmt.name);
        Define(stmt.name);
        ResolveFunction(stmt, FunctionType.FUNCTION);
        return null;
    }

    private void ResolveFunction(Stmt.Function function, FunctionType type)
    {
        FunctionType enclosingFunction = currentFunction;
        currentFunction = type;
        BeginScope();
        foreach( Token param in function.parameters)
        {
            Declare(param);
            Define(param);
        }
        Resolve(function.body);
        EndScope();
        currentFunction = enclosingFunction;
    }

    public object VisitExpressionStmt(Stmt.Expression stmt)
    {
        Resolve(stmt.expression);
        return null;
    }

    public object VisitIfStmt(Stmt.If stmt)
    {
        Resolve(stmt.Condition);
        Resolve(stmt.thenBranch);
        if (stmt.elseBranch != null) Resolve(stmt.elseBranch);
        return null;
    }

    public object VisitPrintStmt(Stmt.Print stmt)
    {
        Resolve(stmt.expression);
        return null;
    }

    public object VisitReturnStmt(Stmt.Return stmt)
    {
        if (currentFunction == FunctionType.NONE)
        {
            Lox.Error(stmt.Keyword, "Can't return from top-level code.");
        }
        if (stmt.value != null)
        {
            Resolve(stmt.value);
        }
        return null;
    }

    public object VisitWhileStmt(Stmt.While stmt)
    {
        Resolve(stmt.condition);
        Resolve(stmt.body);
        return null;
    }

    public object VisitCallExpr(Expr.Call expr)
    {
        Resolve(expr.callee);
        foreach(Expr argument in expr.arguments)
        {
            Resolve(argument);
        }
        return null;
    }

    public object VisitGroupingExpr(Expr.Grouping expr)
    {
        Resolve(expr.expression);
        return null;
    }

    public object VisitLiteralExpr(Expr.Literal expr)
    {
        return null;
    }

    public object VisitLogicalExpr(Expr.Logical expr)
    {
        Resolve(expr.left);
        Resolve(expr.right);
        return null;
    }

    public object VisitUnaryExpr(Expr.Unary expr)
    {
        Resolve(expr.right);
        return null;
    }
}