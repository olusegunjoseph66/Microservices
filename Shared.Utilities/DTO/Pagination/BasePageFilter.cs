using Shared.Utilities.Constants;

namespace Shared.Utilities.DTO.Pagination
{
    public class BasePageFilter
    {
        public BasePageFilter(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber == 0 ? PaginationConstants.DEFAULT_PAGE_NUMBER : pageNumber;
            PageSize = pageSize == 0 ? PaginationConstants.DEFAULT_PAGE_SIZE : pageSize;
        }

        public int PageSize { get; }
        public int PageNumber { get; }
    }
}
