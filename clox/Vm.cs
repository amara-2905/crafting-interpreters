#define DEBUG_TRACE_EXECUTION
using Value = System.Double;

public class VM{
    const int STACK_MAX = 256;
    public Chunk chunk;
    public int ip;
    public Value[] stack = new Value[STACK_MAX];
    public int stackTop = 0;
}

public enum InterpretResult{
    INTERPRET_OK,
    INTERPRET_COMPILE_ERROR,
    INTERPRET_RUNTIME_ERROR
}

public class VirtualMachine{
    public static VM vm;

    public static void InitVM(){
        vm = new VM();
        vm.stack = new Value[256]; 
        ResetStack();
    }

    public static void ResetStack(){
        vm.stackTop = 0;
    }


    public static void FreeVM(){
        
    }

    public static void Push(Value value){
        vm.stack[vm.stackTop] = value;
        vm.stackTop++;
    }

    public static Value Pop(){
        vm.stackTop--;
        return vm.stack[vm.stackTop];
    }

    public static void BinaryAdd(){
        Value b = Pop();
        Value a = Pop();
        Push(a + b);
    }

    public static void BinarySubtract(){
        Value b = Pop();
        Value a = Pop();
        Push(a - b);
    }

    public static void BinaryMultiply(){
        Value b = Pop();
        Value a = Pop();
        Push(a * b);
    }

    public static void BinaryDivide(){
        Value b = Pop();
        Value a = Pop();
        Push(a / b);
    }


    public static InterpretResult Run(){
        for (;;){
            #if DEBUG_TRACE_EXECUTION
                Console.Write("          ");
                for (int i = 0; i < vm.stackTop; i++)
                {
                    Console.Write("[ ");
                    ValueArray.PrintValue(i);
                    Console.Write(" ]");
                }
                Console.WriteLine();
                Debug.DisassembleInstruction(vm.chunk, vm.ip);
            #endif
            byte instruction = vm.chunk.Code![vm.ip++];
            switch ((OpCode)instruction){
                case OpCode.OP_CONSTANT:
                    Value Constant = vm.chunk.Constants.Values[vm.ip++];
                    Push(Constant);
                    break;
                case OpCode.OP_RETURN:
                    ValueArray.PrintValue(Pop());
                    Console.WriteLine();
                    return InterpretResult.INTERPRET_OK;
                case OpCode.OP_NEGATE:
                    Push(-Pop());
                    break;
                case OpCode.OP_ADD:
                    BinaryAdd();
                    break;
                case OpCode.OP_SUBTRACT:
                    BinarySubtract();
                    break;
                case OpCode.OP_MULTIPLY:
                    BinaryMultiply();
                    break;
                case OpCode.OP_DIVIDE:
                    BinaryDivide();
                    break;
            }
        }
    }

    public static InterpretResult Interpret(string source)
    {
        Compiler.Compile(source);
        return InterpretResult.INTERPRET_OK;
    }


}
