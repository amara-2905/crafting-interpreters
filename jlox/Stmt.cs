using System;

public abstract class Stmt
{
    public interface Visitor<R>
    {
    R VisitExpressionStmt(Expression stmt);
    R VisitPrintStmt(Print stmt);
    }
    public class Expression : Stmt
    {
        public readonly Expr expression;

        public Expression(Expr expression)
        {
            this.expression = expression;
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitExpressionStmt(this);
        }
    }

    public class Print : Stmt
    {
        public readonly Expr expression;

        public Print(Expr expression)
        {
            this.expression = expression;
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitPrintStmt(this);
        }
    }


    public abstract R Accept<R>(Visitor<R> visitor);
}
