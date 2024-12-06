namespace sep.backend.v1.DTOs
{
    public interface IAutoMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
        TSource ReverseMap<TSource, TDestination>(TDestination destination);
        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
