using System.Collections;
using System.Collections.Generic;
public class Environment{
    private readonly Dictionary<string,object> Values = new Dictionary<string, object>();
    readonly Environment Enclosing;

    public Environment()
    {
        Enclosing = null;
    }
    public Environment(Environment Enclosing)
    {
        this.Enclosing = Enclosing;
    }

    public object Get(Token name){
        if (Values.ContainsKey(name.lexeme)){
            return Values[name.lexeme];
        }
        if (Enclosing != null) return Enclosing.Get(name);
        throw new RuntimeError(name,"Undefined variable '" + name.lexeme + "'.");
    }

    public void Assign(Token name,object value){
        if (Values.ContainsKey(name.lexeme)){
            Values[name.lexeme] = value;
            return;
        }
        if (Enclosing != null){
            Enclosing.Assign(name,value);
            return;
        }
        throw new RuntimeError(name,"Undefined variable '" + name.lexeme + "'.");
    }

    public void Define(string name, object value){
        Values[name] = value;
    }
}