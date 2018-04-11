using System;
using CafeT.Text;
using org.mariuszgromada.math.mxparser;

namespace CafeT.Mathematics
{
    public class MathEngine
    {
        public bool IsMathExpression(string expr)
        {
            Expression expression = new Expression(expr);
            if (expression.getSyntaxStatus()) return true;
            return false;
        }

        public string Calc(string expr)
        {
            Expression expression = new Expression(expr);
            return expression.calculate().ToReadable();
        }
       
        public static bool IsCorrect(string input, string value)
        {
            if (input.ToLower() == value.ToLower())
                return true;
            return false;
        }

        public static object ToMathExpr(string lowerMessage)
        {
            throw new NotImplementedException();
        }

        public static object GetResult(object expr)
        {
            throw new NotImplementedException();
        }
    }
}
