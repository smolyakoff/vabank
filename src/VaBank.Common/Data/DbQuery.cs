namespace VaBank.Common.Data
{
    public static class DbQuery
    {
        public static DbQuery<T> For<T>()
            where T : class 
        {
            return new DbQuery<T>();
        } 
    }
}
