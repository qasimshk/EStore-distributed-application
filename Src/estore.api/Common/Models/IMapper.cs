namespace estore.api.Common.Models;

public interface IMapper<in TSource, out TDestination>
{
    TDestination Map(TSource from);
}
