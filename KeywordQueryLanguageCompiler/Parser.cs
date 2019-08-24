namespace KeywordQueryLanguageCompiler
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a parser.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Gets the postfix notation equivalent of the given infix notation input.
        /// </summary>
        /// <param name="infixNotation">The input infix notation.</param>
        /// <returns>The postfix notation.</returns>
        /// <remarks>
        /// This implements the shunting-yard algorithm invented by Edsger Dijkstra.
        /// </remarks>
        public static LexicalElement[] GetPostfixNotation(LexicalElement[] infixNotation)
        {
            if (infixNotation == null)
            {
                throw new ArgumentNullException(nameof(infixNotation), "Infix notation array cannot be null.");
            }

            ThrowIfEmptyParentheses(infixNotation);

            var queue = new Queue<LexicalElement>(infixNotation.Length);
            var stack = new Stack<LexicalElement>(infixNotation.Length);

            foreach (var currentElement in infixNotation)
            {
                if (currentElement.Type.IsOperator())
                {
                    while (stack.Count > 0)
                    {
                        var topElement = stack.Peek();

                        if (topElement.Type.IsOperator())
                        {
                            var currentOperatorAssociativity = currentElement.Type.GetAssociativity();
                            var currentOperatorPrecedence = currentElement.Type.GetPrecedence();
                            var stackTopOperatorPrecedence = topElement.Type.GetPrecedence();

                            if ((currentOperatorAssociativity == OperatorAssociativity.Left && currentOperatorPrecedence <= stackTopOperatorPrecedence) ||
                                (currentOperatorAssociativity == OperatorAssociativity.Right && currentOperatorPrecedence < stackTopOperatorPrecedence))
                            {
                                queue.Enqueue(stack.Pop());
                                continue;
                            }
                        }

                        break;
                    }

                    stack.Push(currentElement);
                }
                else if (currentElement.Type == LexicalElementType.Term)
                {
                    queue.Enqueue(currentElement);
                }
                else if (currentElement.Type == LexicalElementType.LeftParenthesis)
                {
                    stack.Push(currentElement);
                }
                else if (currentElement.Type == LexicalElementType.RightParenthesis)
                {
                    var foundLeftParenthesis = false;

                    while (stack.Count > 0)
                    {
                        var topElement = stack.Pop();

                        if (topElement.Type != LexicalElementType.LeftParenthesis)
                        {
                            queue.Enqueue(topElement);
                        }
                        else
                        {
                            foundLeftParenthesis = true;
                            break;
                        }
                    }

                    if (!foundLeftParenthesis)
                    {
                        throw new ParsingException("Parentheses mismatch. Missing left parenthesis", currentElement, ErrorKind.MissingLeftParenthesis);
                    }
                }
            }

            while (stack.Count > 0)
            {
                var topElement = stack.Pop();

                if (topElement.Type == LexicalElementType.LeftParenthesis)
                {
                    throw new ParsingException("Parentheses mismatch. Unmatched left parenthesis", topElement, ErrorKind.UnmatchedLeftParenthesis);
                }

                queue.Enqueue(topElement);
            }

            return queue.ToArray();
        }

        private static void ThrowIfEmptyParentheses(LexicalElement[] infixNotation)
        {
            for (var i = 1; i < infixNotation.Length; i++)
            {
                if (infixNotation[i - 1].Type == LexicalElementType.LeftParenthesis && infixNotation[i].Type == LexicalElementType.RightParenthesis)
                {
                    throw new ParsingException("Incorrect empty parentheses", infixNotation[i - 1], ErrorKind.IncorrectEmptyParentheses);
                }
            }
        }
    }
}
