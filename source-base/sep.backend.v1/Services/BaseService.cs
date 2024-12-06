using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IRepositories;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class BaseService<TSource, TDestination> : IBaseService<TSource, TDestination>
        where TDestination : class
        where TSource : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IAutoMapper _mapper;
        protected readonly IRepository<TDestination> _repository;

        protected BaseService(IUnitOfWork unitOfWork, IAutoMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<TDestination>();
        }

        public async Task<TDestination> CreateAsync(TSource source)
        {
            var entity = _mapper.Map<TSource, TDestination>(source);
            var success = await _repository.Add(entity);

            if (success)
            {
                await _unitOfWork.CompleteAsync();
                return entity;
            }

            return null;
        }

        public async Task<TDestination> DeleteAsync(int id)
        {
            var success = await _repository.Delete(id);

            if (success)
            {
                await _unitOfWork.CompleteAsync();
                return null;
            }

            return null;
        }

        public async Task<IEnumerable<TDestination>> GetAllAsync()
        {
            return await _repository.All();
        }

        public async Task<TDestination> GetByIdAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<TDestination> UpdateAsync(int id, TSource source)
        {
            var entity = _mapper.Map<TSource, TDestination>(source);
            var success = await _repository.Update(entity);

            if (success)
            {
                await _unitOfWork.CompleteAsync();
                return entity;
            }

            return null;
        }
    }
}
