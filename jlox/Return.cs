public class Return : Exception{
    public readonly object value;

    public Return(object value) : base(null){
        this.value = value;
    }
}