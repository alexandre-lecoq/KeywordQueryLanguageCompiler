namespace KeywordQueryLanguageCompiler.Tests
{
    public static class TestsHelper
    {
        public static bool IsEqual(LexicalElement[] a1, LexicalElement[] a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }

            for (var i = 0; i < a1.Length; i++)
            {
                if (!a1[i].Equals(a2[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
