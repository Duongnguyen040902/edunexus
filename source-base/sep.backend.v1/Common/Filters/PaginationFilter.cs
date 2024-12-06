using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Common.Filters
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter()
        {
            this.PageNumber = Configs.PAGE_NUMBER_DEFAULT;
            this.PageSize = Configs.PAGE_SIZE_DEFAULT;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < Configs.PAGE_NUMBER_DEFAULT ? Configs.PAGE_NUMBER_DEFAULT : pageNumber;
            this.PageSize = pageSize > Configs.PAGE_SIZE_DEFAULT ? Configs.PAGE_SIZE_DEFAULT : pageSize;
        }
    }
}