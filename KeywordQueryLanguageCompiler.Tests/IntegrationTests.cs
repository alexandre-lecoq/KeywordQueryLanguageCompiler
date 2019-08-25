namespace KeywordQueryLanguageCompiler.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Xunit;

    public class IntegrationTests
    {
        [Fact]
        public void CompilerTranslateThrowsWhenExpressionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Compiler.Translate(null));
        }

        [Fact]
        public void CompilerTranslateThrowsWhenExpressionIsEmpty()
        {
            Assert.Throws<ParsingException>(() => Compiler.Translate(string.Empty));
        }

        [Fact]
        public void CompilerTranslateGoodSuccessRateInSampleList()
        {
            string[] keywordArray;

            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream(@"KeywordQueryLanguageCompiler.Tests.Resources.keywords-sample.txt"))
            {
                using (var reader = new StreamReader(resourceStream ?? throw new InvalidOperationException(), System.Text.Encoding.UTF8))
                {
                    var content = reader.ReadToEnd();
                    keywordArray = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            var successfulKeywords = new List<string>();
            var failingKeywords = new List<string>();

            foreach (var keyword in keywordArray)
            {
                try
                {
                    var output = Compiler.Translate(keyword, FormsOfType.None);
                    successfulKeywords.Add(keyword);
                }
                catch (ParsingException)
                {
                    failingKeywords.Add(keyword);
                }
            }

            var failureRate = failingKeywords.Count / (double)keywordArray.Length * 100.0;

            Assert.True(failureRate < 0.4,
                $"Failure rate on sample list is expected to be less than 0.4%. It is {failureRate:0.000}% with {failingKeywords.Count} failures out of {keywordArray.Length} samples.");
        }

        [Fact]
        public void CompilerTranslateSuccessBothFormsOf()
        {
            var input = "android AND (oracl* OR C++ OR C99) NOT iphone OR   \"  hey  baby  *\"   AND phone NEAR appl*";
            var expectedOutput = "((((FORMSOF(INFLECTIONAL, \"android\") OR FORMSOF(THESAURUS, \"android\")) AND ((\"oracl*\" OR (FORMSOF(INFLECTIONAL, \"C++\") OR FORMSOF(THESAURUS, \"C++\"))) OR (FORMSOF(INFLECTIONAL, \"C99\") OR FORMSOF(THESAURUS, \"C99\")))) AND NOT (FORMSOF(INFLECTIONAL, \"iphone\") OR FORMSOF(THESAURUS, \"iphone\"))) OR (\"  hey  baby  *\" AND (NEAR((\"phone\", \"appl*\"), 2000, FALSE))))";

            var output = Compiler.Translate(input, FormsOfType.Both);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerTranslateSuccessInflectionalFormsOf()
        {
            var input = "android AND (oracl* OR C++ OR C99) NOT iphone OR   \"  hey  baby  *\"   AND phone ONEAR appl*";
            var expectedOutput = "(((FORMSOF(INFLECTIONAL, \"android\") AND ((\"oracl*\" OR FORMSOF(INFLECTIONAL, \"C++\")) OR FORMSOF(INFLECTIONAL, \"C99\"))) AND NOT FORMSOF(INFLECTIONAL, \"iphone\")) OR (\"  hey  baby  *\" AND (NEAR((\"phone\", \"appl*\"), 2000, TRUE))))";

            var output = Compiler.Translate(input, FormsOfType.Inflectional);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerTranslateSuccessThesaurusFormsOf()
        {
            var input = "android AND (oracl* OR C++ OR C99) NOT iphone OR   \"  hey  baby  *\"   AND phone ONEAR appl*";
            var expectedOutput = "(((FORMSOF(THESAURUS, \"android\") AND ((\"oracl*\" OR FORMSOF(THESAURUS, \"C++\")) OR FORMSOF(THESAURUS, \"C99\"))) AND NOT FORMSOF(THESAURUS, \"iphone\")) OR (\"  hey  baby  *\" AND (NEAR((\"phone\", \"appl*\"), 2000, TRUE))))";

            var output = Compiler.Translate(input, FormsOfType.Thesaurus);

            Assert.Equal(output, expectedOutput);
        }
        
        [Fact]
        public void CompilerTranslateSuccessNoneFormsOf()
        {
            var input = "android AND (oracl* OR C++ OR C99) NOT iphone OR   \"  hey  baby  *\"   AND phone NEAR appl*";
            var expectedOutput = "(((\"android\" AND ((\"oracl*\" OR \"C++\") OR \"C99\")) AND NOT \"iphone\") OR (\"  hey  baby  *\" AND (NEAR((\"phone\", \"appl*\"), 2000, FALSE))))";

            var output = Compiler.Translate(input, FormsOfType.None);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerTranslateSuccessNestedParentheses()
        {
            var input = "a , (( b AND c) OR (d AND e))";
            var expectedOutput = "(\"a\" AND ((\"b\" AND \"c\") OR (\"d\" AND \"e\")))";

            var output = Compiler.Translate(input, FormsOfType.None);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerTranslateFailure()
        {
            var input = "yUv0wW8JDD ( NEAR ) * AND OR NOT Hx3kUd1ARl";
            Assert.Throws<ParsingException>(() => Compiler.Translate(input));
        }
        
        [Fact]
        public void CompilerTranslateSuccessSimpleSpacedTerm()
        {
            var input = "chef de projet";
            var expectedOutput = "FORMSOF(INFLECTIONAL, \"chef de projet\")";

            var output = Compiler.Translate(input, FormsOfType.Inflectional);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerTranslateSuccessComplexSpacedTerm()
        {
            var input = "chef de projet AND assistante  de  diretion";
            var expectedOutput = "(FORMSOF(INFLECTIONAL, \"chef de projet\") AND FORMSOF(INFLECTIONAL, \"assistante de diretion\"))";

            var output = Compiler.Translate(input, FormsOfType.Inflectional);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerGetSearchedTermsThrowsWhenExpressionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Compiler.GetSearchedTerms(null));
        }

        [Fact]
        public void CompilerGetSearchedTermsReturnsAllTerms()
        {
            var input = "a , (( b AND c) OR (d AND e))";

            var output = Compiler.GetSearchedTerms(input);

            Assert.Equal(5, output.Count);
            Assert.Contains("a", output);
            Assert.Contains("b", output);
            Assert.Contains("c", output);
            Assert.Contains("d", output);
            Assert.Contains("e", output);
        }

        [Fact]
        public void CompilerGetSearchedTermsOnlyReturnsTerms()
        {
            var input = "a , (( b AND c) OR (d AND e))";

            var output = Compiler.GetSearchedTerms(input);

            Assert.Equal(5, output.Count);
            Assert.DoesNotContain("(", output);
            Assert.DoesNotContain(")", output);
            Assert.DoesNotContain("AND", output);
            Assert.DoesNotContain("OR", output);
            Assert.DoesNotContain(",", output);
        }

        [Fact]
        public void CompilerDuplicateCommasSuccessfullyCompiles()
        {
            var input = "a ,, b";
            var expectedOutput = "(\"a\" AND \"b\")";

            var output = Compiler.Translate(input, FormsOfType.None);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerAnyApostrophesWorkWithFirstKind()
        {
            var input = "a'b";
            var expectedOutput = "(\"a’b\" OR \"a'b\")";

            var output = Compiler.Translate(input, FormsOfType.None);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerAnyApostrophesWorkWithSecondKind()
        {
            var input = "a’b";
            var expectedOutput = "(\"a’b\" OR \"a'b\")";

            var output = Compiler.Translate(input, FormsOfType.None);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerBeginningOrphanAndsWorks()
        {
            var input = ",a,b";
            var expectedOutput = "(\"a\" AND \"b\")";

            var output = Compiler.Translate(input, FormsOfType.None);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerEndingOrphanAndsWorks()
        {
            var input = "a,b,";
            var expectedOutput = "(\"a\" AND \"b\")";

            var output = Compiler.Translate(input, FormsOfType.None);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerBeginningAndEndingOrphanAndsWorks()
        {
            var input = ",a,b,";
            var expectedOutput = "(\"a\" AND \"b\")";

            var output = Compiler.Translate(input, FormsOfType.None);

            Assert.Equal(output, expectedOutput);
        }

        [Fact]
        public void CompilerEmptyBeginningAndEndingOrphanAndsWorks()
        {
            var input = ",,";
            Assert.Throws<ParsingException>(() => Compiler.Translate(input, FormsOfType.None));
        }
    }
}
