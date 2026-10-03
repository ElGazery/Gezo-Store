namespace Gezo.Api.ResponseBase.Paginations
{
    public class PaginationResponse<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; } // total count of the return response items 

        public IReadOnlyList<T> PaginationData { get; set; }

        public PaginationResponse(int pageIndex, int pageSize, int totalCount, IReadOnlyList<T> paginationData)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            PaginationData = paginationData;
        }


    }
}
