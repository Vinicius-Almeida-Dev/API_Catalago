namespace APICatalago.Pagination
{
    public class PagedList<T> : List<T>
    {
        public PagedList()
        {
        }

        public int? currentPage { get; set; }
        public int? totalPages { get; set; }
        public int? pageSize { get; set; }
        public int? totalCount { get; set; }

        public bool hasPrevious => currentPage > 1;
        public bool hasNext => currentPage < totalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pSize)
        {
            totalCount = count;
            pageSize = pSize;
            currentPage = pageNumber;          
            totalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber -1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
