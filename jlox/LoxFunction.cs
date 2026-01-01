using System.Net.Http.Headers;
using System.Reflection;

public class LoxFunction : LoxCallable{
    private readonly Stmt.Function Declaration;
    private readonly Environment Closure;
    public LoxFunction(Stmt.Function Declaration, Environment Closure){
        this.Declaration = Declaration;
        this.Closure = Closure;
    }

    public LoxFunction Bind(LoxInstance instance){
        Environment environment = new Environment(Closure);
        environment.Define("this",instance);
        return new LoxFunction(Declaration,environment);
    }
    public object Call(Interpreter interpreter, List<object> arguments){
        Environment environment = new Environment(Closure);
        for (int i = 0; i < Declaration.parameters.Count(); i++){
            environment.Define(Declaration.parameters[i].lexeme, arguments[i]);
        }
        try{
            interpreter.ExecuteBlock(Declaration.body,environment);
        }
        catch (Return returnValue){
            return returnValue.value;
        }
        return null;
    }

    public int Arity(){
        return Declaration.parameters.Count();
    }

    public override string ToString(){
        return "<fn " + Declaration.name.lexeme + ">";
    }
}