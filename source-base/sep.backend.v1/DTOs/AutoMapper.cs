using AutoMapper;

namespace sep.backend.v1.DTOs
{
    public class CustomAutoMapper : IAutoMapper
    {
        private readonly IMapper _mapper;

        public CustomAutoMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TSource ReverseMap<TSource, TDestination>(TDestination destination)
        {
            return _mapper.Map<TSource>(destination);
        }
        
        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            _mapper.Map(source, destination);
        }
    }
}
