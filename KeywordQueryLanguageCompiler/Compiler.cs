namespace KeywordQueryLanguageCompiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Compiler
    {
        /// <summary>
        /// Compiles a given postfix notation expression into a CONTAINSTABLE search condition.
        /// </summary>
        /// <param name="searchExpression">The input search expression.</param>
        /// <param name="type">The FORMSOF type to generate.</param>
        /// <returns>The CONTAINSTABLE search condition.</returns>
        public static string Translate(string searchExpression, FormsOfType type = FormsOfType.Both)
        {
            if (searchExpression == null)
            {
                throw new ArgumentNullException(nameof(searchExpression), "String cannot be null.");
            }

            var lexemes = Lexer.Tokenize(searchExpression);

            if (lexemes.Length == 0)
            {
                throw new ParsingException("The tokenized search expression contains no lexical elements.", ErrorKind.InvalidEmptyExpression);
            }

            lexemes = FixAndKeywordCommonIssuesForUserFriendliness(lexemes);

            var postfixNotation = Parser.GetPostfixNotation(lexemes);

            postfixNotation = FixApostrophesForUserFriendliness(postfixNotation);

            var translation = CodeGenerator.Generate(postfixNotation, type);

            return translation;
        }

        private static LexicalElement[] FixAndKeywordCommonIssuesForUserFriendliness(LexicalElement[] lexicalElements)
        {
            lexicalElements = RemoveBeginningAndEndingOrphanAndKeyword(lexicalElements);
            lexicalElements = RemoveDuplicateAndKeyword(lexicalElements);

            return lexicalElements;
        }

        private static LexicalElement[] FixApostrophesForUserFriendliness(LexicalElement[] lexicalElements)
        {
            var result = new List<LexicalElement>(lexicalElements.Length * 2);

            foreach (var lexicalElement in lexicalElements)
            {
                if (lexicalElement.Type == LexicalElementType.Term && (lexicalElement.Text.Contains("'") || lexicalElement.Text.Contains("’")))
                {
                    var version1 = new LexicalElement(LexicalElementType.Term, lexicalElement.Text.Replace("'", "’"), lexicalElement.Position);
                    result.Add(version1);
                    var version2 = new LexicalElement(LexicalElementType.Term, lexicalElement.Text.Replace("’", "'"), lexicalElement.Position);
                    result.Add(version2);
                    var orKeyword = new LexicalElement(LexicalElementType.Or, "OR", lexicalElement.Position);
                    result.Add(orKeyword);
                }
                else
                {
                    result.Add(lexicalElement);
                }
            }

            return result.ToArray();
        }

        private static LexicalElement[] RemoveBeginningAndEndingOrphanAndKeyword(LexicalElement[] lexicalElements)
        {
            if (lexicalElements.Length == 1)
            {
                return lexicalElements;
            }

            var hasBeginningOrphan = lexicalElements[0].Type == LexicalElementType.And;
            var hasEndingOrphan = lexicalElements[lexicalElements.Length - 1].Type == LexicalElementType.And;

            if (!hasBeginningOrphan && !hasEndingOrphan)
            {
                return lexicalElements;
            }

            if (hasBeginningOrphan && hasEndingOrphan && lexicalElements.Length == 2)
            {
                return lexicalElements;
            }

            var startPosition = 0;
            var resultLength = lexicalElements.Length;

            if (hasBeginningOrphan)
            {
                resultLength--;
                startPosition++;
            }

            if (hasEndingOrphan)
            {
                resultLength--;
            }

            var result = new LexicalElement[resultLength];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = lexicalElements[startPosition + i];
            }

            return result;
        }

        private static LexicalElement[] RemoveDuplicateAndKeyword(LexicalElement[] lexicalElements)
        {
            var result = new List<LexicalElement>(lexicalElements.Length);

            var precedingElementIsAnd = false;

            foreach (var lexicalElement in lexicalElements)
            {
                if (precedingElementIsAnd && lexicalElement.Type == LexicalElementType.And)
                {
                    continue;
                }

                result.Add(lexicalElement);

                precedingElementIsAnd = lexicalElement.Type == LexicalElementType.And;
            }

            return result.ToArray();
        }

        public static List<string> GetSearchedTerms(string searchExpression)
        {
            if (searchExpression == null)
            {
                throw new ArgumentNullException(nameof(searchExpression), "String cannot be null.");
            }

            var lexemes = Lexer.Tokenize(searchExpression);

            // We're not supposed to use UserFriendlinessParserLevelHeuristics() right here in that way.
            // But that doesn't matter since we get rid of anything but terms just next.
            var lexemesWithFixedQuotes = FixApostrophesForUserFriendliness(lexemes);
            var terms = lexemesWithFixedQuotes.Where(l => l.Type == LexicalElementType.Term).Select(l => l.Text).ToList();

            return terms;
        }
    }
}
