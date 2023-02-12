namespace PollingStationsResolver.Domain.Specifications;

public class Pagination
{
    public Pagination(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public int Page { get; }
    public int PageSize { get; }
}