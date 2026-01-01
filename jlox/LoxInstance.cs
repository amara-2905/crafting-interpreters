public class LoxInstance
{
    private LoxClass Class;
    private readonly Dictionary<string,object> fields = new Dictionary<string, object>();
    public LoxInstance(LoxClass Class){
        this.Class = Class;
    }

    public override string ToString(){
        return Class.name + " instance";
    }

    public object Get(Token name){
        if (fields.ContainsKey(name.lexeme)){
            return fields[name.lexeme];
        }
        LoxFunction method = Class.FindMethod(name.lexeme);
        if (method != null) return method.Bind(this);
        throw new RuntimeError(name,"Undefined property '" + name.lexeme + "'.");
    }

    public void Set(Token name, object value){
        fields[name.lexeme] = value;
    }
}