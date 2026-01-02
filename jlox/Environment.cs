public class Environment{
    private readonly Dictionary<string,object> Values = new Dictionary<string, object>();
    public readonly Environment Enclosing;

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

    public object GetAt(int Distance, string name)
    {
        return Ancestor(Distance).Values[name];
    }

    public Environment Ancestor(int Distance)
    {
        Environment environment = this; 
        for (int i = 0; i < Distance; i++)
        {
            environment = environment.Enclosing;
        }
        return environment;
    }

    public void AssignAt(int Distance, Token name, object value)
    {
        Ancestor(Distance).Values[name.lexeme] = value;
    }
}