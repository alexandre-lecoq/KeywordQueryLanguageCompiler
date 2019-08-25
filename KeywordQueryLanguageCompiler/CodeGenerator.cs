namespace KeywordQueryLanguageCompiler
{
    using System;
    using System.Collections.Generic;

    public static class CodeGenerator
    {
        /// <summary>
        /// Generates CONTAINSTABLE search condition code for a given postfix notation expression.
        /// </summary>
        /// <param name="postfixNotation">The input postfix notation.</param>
        /// <param name="type">The FORMSOF type to generate.</param>
        /// <returns>The CONTAINSTABLE search condition.</returns>
        public static string Generate(LexicalElement[] postfixNotation, FormsOfType type = FormsOfType.Both)
        {
            if (postfixNotation == null)
            {
                throw new ArgumentNullException(nameof(postfixNotation), "Postfix notation array cannot be null.");
            }

            if (postfixNotation.Length == 0)
            {
                throw new ParsingException("Invalid empty expression.", ErrorKind.InvalidEmptyExpression);
            }

            var stack = new Stack<string>(postfixNotation.Length);

            foreach (var currentElement in postfixNotation)
            {
                if (currentElement.Type.IsOperator())
                {
                    var operandCount = currentElement.Type.GetOperandCount();

                    if (stack.Count < operandCount)
                    {
                        throw new ParsingException(
                            $"Missing operand for operator. Expected {operandCount} operands, found {stack.Count} potential operands",
                            currentElement, ErrorKind.MissingOperand);
                    }

                    switch (currentElement.Type)
                    {
                        case LexicalElementType.And:
                            ProcessAnd(stack, type);
                            break;

                        case LexicalElementType.Not:
                            ProcessNot(stack, type);
                            break;

                        case LexicalElementType.Or:
                            ProcessOr(stack, type);
                            break;

                        case LexicalElementType.Near:
                            ProcessNear(stack, currentElement, false);
                            break;

                        case LexicalElementType.OrderedNear:
                            ProcessNear(stack, currentElement, true);
                            break;
                    }
                }
                else
                {
                    if (currentElement.Type != LexicalElementType.Term)
                    {
                        throw new ParsingException("Expected \"Term\" token type. Found unexpected token",
                            currentElement, ErrorKind.UnexpectedTokenType);
                    }

                    stack.Push(Quote(currentElement.Text));
                }
            }

            if (stack.Count != 1)
            {
                throw new ParsingException($"Evaluation failed. The stack contains {stack.Count} items.",
                    ErrorKind.StackHasMoreOrLessThanOneItem);
            }

            var result = stack.Pop();
            result = FormsOf(result, type);

            return result;
        }

        private static void ProcessAnd(Stack<string> stack, FormsOfType type)
        {
            var o2 = stack.Pop();
            var o1 = stack.Pop();

            var result = $"({FormsOf(o1, type)} AND {FormsOf(o2, type)})";
            stack.Push(result);
        }

        private static void ProcessOr(Stack<string> stack, FormsOfType type)
        {
            var o2 = stack.Pop();
            var o1 = stack.Pop();
            var result = $"({FormsOf(o1, type)} OR {FormsOf(o2, type)})";
            stack.Push(result);
        }

        private static void ProcessNot(Stack<string> stack, FormsOfType type)
        {
            var o2 = stack.Pop();
            var o1 = stack.Pop();
            var result = $"({FormsOf(o1, type)} AND NOT {FormsOf(o2, type)})";
            stack.Push(result);
        }

        private static void ProcessNear(Stack<string> stack, LexicalElement element, bool matchOrder)
        {
            var o2 = stack.Pop();
            var o1 = stack.Pop();

            if (IsComplexExpression(o1) || IsComplexExpression(o2))
            {
                throw new ParsingException("NEAR keyword only supports simple terms or prefix terms. The keyword does not allow complex expressions as operands", element, ErrorKind.NearKeywordOperandError);
            }

            var matchOrderString = matchOrder.ToString().ToUpper();
            var result = $"(NEAR(({o1}, {o2}), 2000, {matchOrderString}))";
            stack.Push(result);
        }

        private static string Quote(string text)
        {
            return $"\"{text}\"";
        }

        private static bool IsComplexExpression(string text)
        {
            return text.StartsWith("(") && text.EndsWith(")");
        }

        private static string FormsOf(string text, FormsOfType type)
        {
            if (IsComplexExpression(text))
            {
                return text;
            }
            
            if (text[text.Length-2] == '*')
            {
                return text;
            }

            switch (type)
            {
                case FormsOfType.None:
                    return text;
                case FormsOfType.Inflectional:
                    return $"FORMSOF(INFLECTIONAL, {text})";
                case FormsOfType.Thesaurus:
                    return $"FORMSOF(THESAURUS, {text})";
                case FormsOfType.Both:
                    return $"(FORMSOF(INFLECTIONAL, {text}) OR FORMSOF(THESAURUS, {text}))";
                default:
                    throw new ParsingException("Unknown FormsOfType.", ErrorKind.UnknownFormsOfType);
            }
        }
    }
}
