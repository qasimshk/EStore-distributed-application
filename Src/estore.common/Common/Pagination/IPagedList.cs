namespace estore.common.Common.Pagination;

public interface IPagedList<T>
{
    Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize);
}
