using Firebase.Database;
using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Application.Common.Models
{
    public class FirebaseCursorPaginatedList<TItem> : CursorPaginatedList<FirebaseObject<TItem>, string>
    {
        public FirebaseCursorPaginatedList(string cursor, List<FirebaseObject<TItem>> items, int pageSize, bool hasNextPage) : base(cursor, items, pageSize, hasNextPage)
        {
        }
    }

    public class CursorPaginatedList<TItem> : CursorPaginatedList<TItem, string>
    {
        public CursorPaginatedList(string cursor, List<TItem> items, int pageSize, bool hasNextPage) : base(cursor, items, pageSize, hasNextPage)
        {
        }
    }

    public class FirebaseCursorPaginatedList<TItem, TKey> : CursorPaginatedList<FirebaseObject<TItem>, TKey>
    {
        public FirebaseCursorPaginatedList(TKey cursor, List<FirebaseObject<TItem>> items, int pageSize, bool hasNextPage) : base(cursor, items, pageSize, hasNextPage)
        {
        }
    }

    public class CursorPaginatedList<TItem, TKey>
    {
        public TKey Cursor { get; }

        public List<TItem> Items { get; }

        public int PageSize { get; }

        public bool HasNextPage { get; }

        public CursorPaginatedList(TKey cursor, List<TItem> items, int pageSize, bool hasNextPage)
        {
            Cursor = cursor;
            Items = items;
            PageSize = pageSize;
            HasNextPage = hasNextPage;
        }
    }

    public class CursorPaginatedParamter
    {
        [FromQuery(Name = "cursor")]
        public string Cursor { get; set; }
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
    }

    public class CursorPaginatedParamter<TKey>
    {
        [FromQuery(Name = "cursor")]
        public TKey Cursor { get; set; }
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
    }
}
