namespace APICatalago.Pagination
{
    public class Parameters
    {
        const int maxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = maxPageSize;
        public int pageSize 
        { get { return _pageSize; } 
          set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
    }
}
