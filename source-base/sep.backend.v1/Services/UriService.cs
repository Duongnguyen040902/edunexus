using Microsoft.AspNetCore.WebUtilities;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var endpointUri = new Uri(string.Concat(_baseUri.TrimEnd('/'), "/", route.TrimStart('/')));
            var modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());

            return new Uri(modifiedUri);
        }
    }
}
