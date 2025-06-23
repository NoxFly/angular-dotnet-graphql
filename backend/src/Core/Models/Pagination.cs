namespace Core.Models;

public class Pagination<T> where T : class
{
    public List<T> Data { get; set; } = new List<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
