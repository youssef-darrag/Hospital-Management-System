﻿namespace Hospital.Core.Helpers
{
    public class PagedResult<T> where T : class
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
