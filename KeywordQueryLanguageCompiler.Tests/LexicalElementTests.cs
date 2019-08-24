namespace KeywordQueryLanguageCompiler.Tests
{
    using Xunit;

    public class LexicalElementTests
    {
        [Fact]
        public void WorkingConstructorAndGetters()
        {
            var element = new LexicalElement(LexicalElementType.Term, "test", 12);

            Assert.Equal(LexicalElementType.Term, element.Type);
            Assert.Equal("test", element.Text);
            Assert.Equal(12, element.Position);
        }

        [Fact]
        public void TypedEquals_AllEquals()
        {
            var element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            var element2 = new LexicalElement(LexicalElementType.Term, "test", 12);

            Assert.True(element1.Equals(element2));
        }

        [Fact]
        public void TypedEquals_AllDifferent()
        {
            var element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            var element2 = new LexicalElement(LexicalElementType.Or, "nono", 19);

            Assert.False(element1.Equals(element2));
        }

        [Fact]
        public void TypedEquals_DifferentType()
        {
            var element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            var element2 = new LexicalElement(LexicalElementType.Or, "test", 12);

            Assert.False(element1.Equals(element2));
        }

        [Fact]
        public void TypedEquals_DifferentText()
        {
            var element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            var element2 = new LexicalElement(LexicalElementType.Term, "nono", 12);

            Assert.False(element1.Equals(element2));
        }

        [Fact]
        public void TypedEquals_DifferentPosition()
        {
            var element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            var element2 = new LexicalElement(LexicalElementType.Term, "test", 19);

            Assert.False(element1.Equals(element2));
        }

        [Fact]
        public void TypedEquals_DifferentFromNull()
        {
            var element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            var element2 = (LexicalElement) null;

            Assert.False(element1.Equals(element2));
        }

        [Fact]
        public void ObjectEquals_AllEquals()
        {
            object element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            object element2 = new LexicalElement(LexicalElementType.Term, "test", 12);

            Assert.True(element1.Equals(element2));
        }

        [Fact]
        public void ObjectEquals_AllDifferent()
        {
            object element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            object element2 = new LexicalElement(LexicalElementType.Or, "nono", 19);

            Assert.False(element1.Equals(element2));
        }

        [Fact]
        public void ObjectEquals_NotLexicalElement()
        {
            object element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            object element2 = -159;

            Assert.False(element1.Equals(element2));
        }

        [Fact]
        public void ToStringLengthGreaterThanZero1()
        {
            var element = new LexicalElement(LexicalElementType.Term, "test", 12);

            Assert.True(element.ToString().Length > 0);
        }

        [Fact]
        public void ToStringLengthGreaterThanZero2()
        {
            var element = new LexicalElement(LexicalElementType.Term, null, 12);

            Assert.True(element.ToString().Length > 0);
        }

        [Fact]
        public void ToStringContainsType()
        {
            var element = new LexicalElement(LexicalElementType.Term, null, 12);

            Assert.Contains("Term", element.ToString());
        }

        [Fact]
        public void ToStringContainsTypeAndText()
        {
            var element = new LexicalElement(LexicalElementType.Term, "test", 12);

            Assert.Contains("Term", element.ToString());
            Assert.Contains("test", element.ToString());
        }

        [Fact]
        public void GetHashcode()
        {
            var element1 = new LexicalElement(LexicalElementType.Term, "test", 12);
            var element2 = new LexicalElement(LexicalElementType.Term, "test", 12);

            var element3 = new LexicalElement(LexicalElementType.Or, "test", 12);
            var element4 = new LexicalElement(LexicalElementType.Term, "nono", 12);
            var element5 = new LexicalElement(LexicalElementType.Term, "test", 19);

            var element6 = new LexicalElement(LexicalElementType.Or, null, 12);
            var element7 = new LexicalElement(LexicalElementType.Term, null, 12);
            var element8 = new LexicalElement(LexicalElementType.Term, null, 19);

            Assert.Equal(element1.GetHashCode(), element2.GetHashCode());
            Assert.NotEqual(element1.GetHashCode(), element3.GetHashCode());
            Assert.NotEqual(element1.GetHashCode(), element4.GetHashCode());
            Assert.NotEqual(element1.GetHashCode(), element5.GetHashCode());
            Assert.NotEqual(element1.GetHashCode(), element6.GetHashCode());
            Assert.NotEqual(element1.GetHashCode(), element7.GetHashCode());
            Assert.NotEqual(element1.GetHashCode(), element8.GetHashCode());

            Assert.NotEqual(element3.GetHashCode(), element4.GetHashCode());
            Assert.NotEqual(element3.GetHashCode(), element5.GetHashCode());

            Assert.NotEqual(element6.GetHashCode(), element7.GetHashCode());
            Assert.NotEqual(element6.GetHashCode(), element8.GetHashCode());
        }

        [Fact]
        public void GetHashcodeNullElementTextDoesNotThrow()
        {
            var element = new LexicalElement(LexicalElementType.Term, null, 1);
            var hashcode = element.GetHashCode();
        }
    }
}