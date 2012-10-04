namespace Gutek.Common.Tests.Extensions.String
{
    public class string_tests_base
    {
        protected readonly string STRING_EMPTY = string.Empty;
        protected readonly string STRING_NULL = null;
        protected readonly string STRING_VALUE = "VALUE";
        protected readonly string STRING_TEST_TEXT_VALUE = "VALUE";
        protected readonly string STRING_WITHOUT_PLACEHOLDERS = "VALUE";
        protected readonly string STRING_WITH_PLACEHOLDERS = "VALUE {0} {1}";
        protected readonly string STRING_WITH_PLACEHOLDERS_FILLED = "VALUE VALUE VALUE";
        protected readonly string STRING_SPACES = "              ";
        protected readonly string STRING_WHITESPACES = "\n\r \n\t";
    }
}