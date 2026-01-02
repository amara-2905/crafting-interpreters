using System.Net.Http.Headers;

public class LoxClass : LoxCallable
{
    public readonly string name;
    public readonly LoxClass superclass;
    private readonly Dictionary<string,LoxFunction> methods;    
    public LoxClass(string name, LoxClass superclass, Dictionary<string,LoxFunction> methods){
        this.name = name;
        this.superclass = superclass;
        this.methods = methods;
    }

    public LoxFunction FindMethod(string name){
        if (methods.ContainsKey(name)){
            return methods[name];
        }
        if (superclass != null){
            return superclass.FindMethod(name);
        }
        return null;
    }

    public override string ToString(){
        return name;
    }

    public object Call(Interpreter interpreter, List<object> arguments){
        LoxInstance instance = new LoxInstance(this);
        LoxFunction Initializer = FindMethod("init");
        if(Initializer != null){
            Initializer.Bind(instance).Call(interpreter, arguments);
        }
        return instance;
    }

    public int Arity(){
        LoxFunction Initializer = FindMethod("init");
        if (Initializer == null) return 0;
        return Initializer.Arity();
    }
}
