using System;

public abstract class Stmt{
    public interface Visitor<R>{
        R VisitBlockStmt(Block stmt);
        R VisitExpressionStmt(Expression stmt);
        R VisitPrintStmt(Print stmt);
        R VisitVarStmt(Var stmt);
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

    public class Expression : Stmt{
        public readonly Expr expression;

        public Expression(Expr expression){
            this.expression = expression;
        }

        public override R Accept<R>(Visitor<R> visitor){
            return visitor.VisitExpressionStmt(this);
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

    public abstract R Accept<R>(Visitor<R> visitor);
}
