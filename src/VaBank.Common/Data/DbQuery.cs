namespace VaBank.Common.Data
{
    public static class DbQuery
    {
        public static DbQuery<T> For<T>()
            where T : class 
        {
            return new DbQuery<T>();
        }

        public static PagedDbQuery<T> PagedFor<T>(int page = 1, int pageSize = 10)
            where T : class
        {
            return new PagedDbQuery<T>(page, pageSize);
        } 
    }
}
