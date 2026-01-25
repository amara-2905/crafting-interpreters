# Crafting Interpreters
To run this project:
```bash
git clone https://github.com/amara-2905/crafting_interpreters.git 
cd crafting_interpreters
```

To generate the AST, run the `tools` project and pass the `jlox` folder as the output directory.

```bash
cd tools
dotnet run -- ../jlox
```

### Running the Tree-Walk Interpreter / Bytecode VM
```bash
cd jlox or clox
```

**Option A: REPL mode**
```bash
dotnet run
```

**Option B: File mode**
```bash
dotnet run -- path/to/file.lox
```
