using sep.backend.v1.Common.Filters;

namespace sep.backend.v1.Services.IServices
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}