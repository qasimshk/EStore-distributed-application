namespace estore.api.Common.Pagination;

public interface IPagedList<T>
{
    PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize);

    Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize);
}
