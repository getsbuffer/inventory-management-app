namespace IM.Library.DTO
{
    public class Query
    {
        public string? QueryString { get; set; }

        public Query(string queryString)
        {
            QueryString = queryString;
        }
    }
}