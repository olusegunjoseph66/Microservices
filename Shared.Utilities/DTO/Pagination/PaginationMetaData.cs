using Shared.Utilities.Constants;

namespace Shared.Utilities.DTO.Pagination
{
    public class PaginationMetaData
    {
        public PaginationMetaData(int pageNumber, int pageSize, int totalPages, int totalRecords)
        {
            PageNumber = pageNumber == 0 ? PaginationConstants.DEFAULT_PAGE_NUMBER : pageNumber;
            PageSize = pageSize == 0 ? PaginationConstants.DEFAULT_PAGE_SIZE : pageSize;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}
