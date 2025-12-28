using System;

abstract class Expr
{
    class Binary : Expr
    {
        public readonly Expr left;
        public readonly Token op;
        public readonly Expr right;

        public Binary(Expr left, Token op, Expr right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }

    class Grouping : Expr
    {
        public readonly Expr expression;

        public Grouping(Expr expression)
        {
            this.expression = expression;
        }
    }

    class Literal : Expr
    {
        public readonly object value;

        public Literal(object value)
        {
            this.value = value;
        }
    }

    class Unary : Expr
    {
        public readonly Token op;
        public readonly Expr right;

        public Unary(Token op, Expr right)
        {
            this.op = op;
            this.right = right;
        }
    }

}
