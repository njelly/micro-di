namespace MicroDI.Core
{
    public static class TagUtility
    {
        private static sbyte _nullTag = -128;

        public static object GetNullTag()
        {
            return _nullTag;
        }
    }
}
