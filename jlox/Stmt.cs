using System;

public abstract class Stmt{
    public interface Visitor<R>{
        R VisitBlockStmt(Block stmt);
        R VisitClassStmt(Class stmt);
        R VisitExpressionStmt(Expression stmt);
        R VisitFunctionStmt(Function stmt);
        R VisitIfStmt(If stmt);
        R VisitPrintStmt(Print stmt);
        R VisitReturnStmt(Return stmt);
        R VisitVarStmt(Var stmt);
        R VisitWhileStmt(While stmt);
    }
    public class Block : Stmt{
        public readonly List<Stmt> statements;

        public Block(List<Stmt> statements){
            this.statements = statements;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitBlockStmt(this);
        }
    }

    public class Class : Stmt{
        public readonly Token name;
        public readonly Expr.Variable SuperClass;
        public readonly List<Stmt.Function> methods;

        public Class(Token name, Expr.Variable SuperClass, List<Stmt.Function> methods){
            this.name = name;
            this.SuperClass = SuperClass;
            this.methods = methods;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitClassStmt(this);
        }
    }

    public class Expression : Stmt{
        public readonly Expr expression;

        public Expression(Expr expression){
            this.expression = expression;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitExpressionStmt(this);
        }
    }

    public class Function : Stmt{
        public readonly Token name;
        public readonly List<Token> parameters;
        public readonly List<Stmt> body;

        public Function(Token name, List<Token> parameters, List<Stmt> body){
            this.name = name;
            this.parameters = parameters;
            this.body = body;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitFunctionStmt(this);
        }
    }

    public class If : Stmt{
        public readonly Expr Condition;
        public readonly Stmt thenBranch;
        public readonly Stmt elseBranch;

        public If(Expr Condition, Stmt thenBranch, Stmt elseBranch){
            this.Condition = Condition;
            this.thenBranch = thenBranch;
            this.elseBranch = elseBranch;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitIfStmt(this);
        }
    }

    public class Print : Stmt{
        public readonly Expr expression;

        public Print(Expr expression){
            this.expression = expression;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitPrintStmt(this);
        }
    }

    public class Return : Stmt{
        public readonly Token Keyword;
        public readonly Expr value;

        public Return(Token Keyword, Expr value){
            this.Keyword = Keyword;
            this.value = value;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitReturnStmt(this);
        }
    }

    public class Var : Stmt{
        public readonly Token name;
        public readonly Expr Initializer;

        public Var(Token name, Expr Initializer){
            this.name = name;
            this.Initializer = Initializer;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitVarStmt(this);
        }
    }

    public class While : Stmt{
        public readonly Expr condition;
        public readonly Stmt body;

        public While(Expr condition, Stmt body){
            this.condition = condition;
            this.body = body;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitWhileStmt(this);
        }
    }

    public abstract R Accept<R>(Visitor<R> visitor);
}
