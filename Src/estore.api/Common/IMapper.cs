namespace estore.api.Common;

public interface IMapper<in TSource, out TDestination>
{
    TDestination Map(TSource from);
}
