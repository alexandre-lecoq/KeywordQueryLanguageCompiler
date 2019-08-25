namespace KeywordQueryLanguageCompiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A keyword search lexer.
    /// </summary>
    public static class Lexer
    {
        /// <summary>
        /// Tokenize the input string.
        /// </summary>
        /// <param name="input">The string to tokenize.</param>
        /// <returns>An array of lexical elements.</returns>
        public static LexicalElement[] Tokenize(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "String cannot be null.");
            }

            var parts = Split(input);
            var fixedParts = FixLiterals(parts);
            var cleanedParts = RemoveWhiteSpaces(fixedParts);
            var lexicalElements = ToLexicalElements(cleanedParts);
            var mergeLexicalElements = MergeFollowingTerms(lexicalElements);

            return mergeLexicalElements;
        }

        private static Tuple<string, int>[] Split(string input)
        {
            var list = new List<Tuple<string, int>>(input.Length / 2);
            var token = new StringBuilder(input.Length / 2);
            var isInWhiteSpaceAggregation = false;
            var isInOtherAggregation = false;
            var aggregationStartPosition = -1;

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (Char.IsWhiteSpace(c))
                {
                    if (isInOtherAggregation)
                    {
                        list.Add(new Tuple<string, int>(token.ToString(), aggregationStartPosition));
                        token.Clear();
                        isInOtherAggregation = false;
                    }

                    token.Append(c);

                    if (isInWhiteSpaceAggregation == false)
                    {
                        isInWhiteSpaceAggregation = true;
                        aggregationStartPosition = i;
                    }
                }
                else if (IsOther(c))
                {
                    if (isInWhiteSpaceAggregation)
                    {
                        list.Add(new Tuple<string, int>(token.ToString(), aggregationStartPosition));
                        token.Clear();
                        isInWhiteSpaceAggregation = false;
                    }

                    token.Append(c);

                    if (isInOtherAggregation == false)
                    {
                        isInOtherAggregation = true;
                        aggregationStartPosition = i;
                    }
                }
                else
                {
                    if (isInWhiteSpaceAggregation || isInOtherAggregation)
                    {
                        list.Add(new Tuple<string, int>(token.ToString(), aggregationStartPosition));
                        token.Clear();
                        isInWhiteSpaceAggregation = false;
                        isInOtherAggregation = false;
                    }

                    list.Add(new Tuple<string, int>(c.ToString(CultureInfo.InvariantCulture), i));
                }
            }

            if (isInWhiteSpaceAggregation || isInOtherAggregation)
            {
                list.Add(new Tuple<string, int>(token.ToString(), aggregationStartPosition));
                token.Clear();
            }

            return list.ToArray();
        }

        private static bool IsOther(char c)
        {
            return !Char.IsWhiteSpace(c) && c != '(' && c != ')' && c != '\"' && c != ',';
        }

        private static Tuple<string, int>[] FixLiterals(Tuple<string, int>[] input)
        {
            var list = new List<Tuple<string, int>>(input.Length / 2);
            var temp = new List<Tuple<string, int>>(input.Length / 2);
            var isInLiteral = false;

            foreach (var tuple in input)
            {
                if (tuple.Item1 == "\"")
                {
                    if (isInLiteral == false)
                    {
                        isInLiteral = true;
                    }
                    else
                    {
                        if (temp.Count > 0)
                        {
                            var aggregation = "\"" + string.Join("", temp.Select(t => t.Item1)) + "\"";
                            var startPosition = temp[0].Item2;
                            list.Add(new Tuple<string, int>(aggregation, startPosition));
                            temp.Clear();
                        }

                        isInLiteral = false;
                    }

                    continue;
                }

                if (isInLiteral)
                {
                    temp.Add(tuple);
                }
                else
                {
                    list.Add(tuple);
                }
            }

            if (isInLiteral)
            {
                throw new ParsingException("Unmatched double quote", ErrorKind.UnmatchedDoubleQuote);
            }

            return list.ToArray();
        }

        private static Tuple<string, int>[] RemoveWhiteSpaces(Tuple<string, int>[] input)
        {
            return input.Where(tuple => tuple.Item1.Trim() != "").ToArray();
        }

        private static LexicalElement[] ToLexicalElements(Tuple<string, int>[] input)
        {
            var result = new LexicalElement[input.Length];
            var i = 0;

            foreach (var tuple in input)
            {
                var type = GetLexicalElementType(tuple.Item1);
                var text = RemoveQuotes(tuple.Item1);
                // Adding 1 to the position let us switch from the 0-based character array index, to the 1-based column index of text editors.
                var position = tuple.Item2 + 1;

                var element = new LexicalElement(type, text, position);

                if (element.Type == LexicalElementType.Term && !IsAsteriskFreeOrAsteriskEnded(element.Text))
                {
                    throw new ParsingException("Lexical term element contains misplaced asterisk", element, ErrorKind.MisplacedAsteriskInTerm);
                }

                result[i++] = element;
            }

            return result;
        }

        private static string RemoveQuotes(string text)
        {
            if (text.Length > 1 && text.StartsWith("\"") && text.EndsWith("\""))
            {
                return text.Substring(1, text.Length - 2);
            }

            return text;
        }

        private static bool IsAsteriskFreeOrAsteriskEnded(string text)
        {
            var length = text.Length;
            var index = text.IndexOf('*', 0, length);

            return index == -1 || index == length - 1;
        }

        private static LexicalElementType GetLexicalElementType(string text)
        {
            switch (text.Length)
            {
                case 1:
                {
                    var c = text[0];

                    if (c == '(')
                    {
                        return LexicalElementType.LeftParenthesis;
                    }
                    if (c == ')')
                    {
                        return LexicalElementType.RightParenthesis;
                    }
                    if (c == ',')
                    {
                        return LexicalElementType.And;
                    }

                    return LexicalElementType.Term;
                }

                case 2:
                    if (text.Equals("OR", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LexicalElementType.Or;
                    }

                    return LexicalElementType.Term;

                case 3:
                    if (text.Equals("NOT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LexicalElementType.Not;
                    }
                    if (text.Equals("AND", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LexicalElementType.And;
                    }

                    return LexicalElementType.Term;

                case 4:
                    if (text.Equals("NEAR", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LexicalElementType.Near;
                    }

                    return LexicalElementType.Term;

                case 6:
                    if (text.Equals("ANDNOT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LexicalElementType.Not;
                    }

                    return LexicalElementType.Term;

                default:
                    return LexicalElementType.Term;
            }
        }

        private static LexicalElement[] MergeFollowingTerms(LexicalElement[] lexicalElements)
        {
            var result = new List<LexicalElement>(lexicalElements.Length);

            LexicalElement tempTerm = null;

            foreach (var lexicalElement in lexicalElements)
            {
                if (lexicalElement.Type == LexicalElementType.Term)
                {
                    if (tempTerm == null)
                    {
                        tempTerm = lexicalElement;
                    }
                    else
                    {
                        var newText = $"{tempTerm.Text} {lexicalElement.Text}";
                        tempTerm = new LexicalElement(LexicalElementType.Term, newText, tempTerm.Position);
                    }
                }
                else
                {
                    if (tempTerm != null)
                    {
                        result.Add(tempTerm);
                        tempTerm = null;
                    }

                    result.Add(lexicalElement);
                }
            }

            if (tempTerm != null)
            {
                result.Add(tempTerm);
            }

            return result.ToArray();
        }
    }
}

