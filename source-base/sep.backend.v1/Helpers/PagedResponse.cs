using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sep.backend.v1.Helpers
{
    public static class PagedResponseHelper
    {
        public static PagedResponse<List<T>> CreatePagedResponse<T>(
            IQueryable<T> source,
            PaginationFilter validFilter,
            int totalRecords,
            IUriService uriService,
            string route)
        {
            var pagedData = GetPagedData(source, validFilter);
            var response = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            int totalPages = CalculateTotalPages(totalRecords, validFilter.PageSize);
            SetPagingUrls(response, validFilter, totalPages, uriService, route);
            response.TotalPages = totalPages;
            response.TotalRecords = totalRecords;

            return response;
        }

        private static List<T> GetPagedData<T>(IQueryable<T> source, PaginationFilter filter)
        {
            return source.Skip((filter.PageNumber - 1) * filter.PageSize)
                         .Take(filter.PageSize)
                         .ToList();
        }

        private static int CalculateTotalPages(int totalRecords, int pageSize)
        {
            var totalPages = (double)totalRecords / pageSize;
            return Convert.ToInt32(Math.Ceiling(totalPages));
        }

        private static void SetPagingUrls<T>(
            PagedResponse<List<T>> response,
            PaginationFilter filter,
            int totalPages,
            IUriService uriService,
            string route)
        {
            response.NextPage = filter.PageNumber < totalPages
                ? uriService.GetPageUri(new PaginationFilter(filter.PageNumber + 1, filter.PageSize), route)
                : null;

            response.PreviousPage = filter.PageNumber > 1
                ? uriService.GetPageUri(new PaginationFilter(filter.PageNumber - 1, filter.PageSize), route)
                : null;

            response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, filter.PageSize), route);
            response.LastPage = uriService.GetPageUri(new PaginationFilter(totalPages, filter.PageSize), route);
        }
    }
}
