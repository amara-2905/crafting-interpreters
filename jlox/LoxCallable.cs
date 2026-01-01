public interface LoxCallable
{
    public int Arity();
    public object Call(Interpreter interpreter, List<object> arguments);
}
public class NativeFunction : LoxCallable{
    private readonly int arity;
    private readonly Func<Interpreter, List<object>, object> function;

    public NativeFunction(
        int arity,
        Func<Interpreter, List<object>, object> function){
        this.arity = arity;
        this.function = function;
        }

    public int Arity() => arity;
    public object Call(Interpreter interpreter, List<object> arguments) => function(interpreter, arguments);
    public override string ToString() => "<native fn>";
}