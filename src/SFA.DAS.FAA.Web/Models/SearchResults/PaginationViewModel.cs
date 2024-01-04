namespace SFA.DAS.FAA.Web.Models.SearchResults
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
        public string BaseUrl { get; init; }
        public string PrevUrl { get; set; }
        public string NextUrl { get; set; }
        public List<LinkItem> LinkItems { get; set; } = new();

        public PaginationViewModel(int currentPage, int pageSize, int totalPages, string baseUrl)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            BaseUrl = baseUrl;
            PrevUrl = "";
            NextUrl = "";

            if (totalPages == 0) return;

            if (CurrentPage > 1)
            {
                PrevUrl = GetUrl(baseUrl, CurrentPage - 1, PageSize);
            }

            if (CurrentPage < totalPages) NextUrl = GetUrl(baseUrl, CurrentPage + 1, PageSize);


            var delta = 3;
            if (TotalPages > 10)
            {
                delta = CurrentPage > 4 && CurrentPage < TotalPages - 3 ? 2 : 4;
            }

            var startIndex = (int)Math.Round(CurrentPage - delta / (double)2);
            var endIndex = (int)Math.Round(CurrentPage + delta / (double)2);

            if (startIndex - 1 == 1 || endIndex + 1 == TotalPages)
            {
                startIndex += 1;
                endIndex += 1;
            }

            var to = Math.Min(TotalPages, delta + 1);
            for (var index = 1; index <= to; index++)
            {
                var url = index == CurrentPage
                    ? null
                    : GetUrl(baseUrl, index, PageSize);
                LinkItems.Add(new LinkItem(url, index.ToString()));
            }

            if (CurrentPage > delta)
            {
                LinkItems.Clear();
                var from = Math.Min(startIndex, TotalPages - delta);
                to = Math.Min(endIndex, TotalPages);
                for (var index = from; index <= to; index++)
                {
                    var url = index == CurrentPage
                        ? null
                        : GetUrl(baseUrl, index, PageSize);
                    LinkItems.Add(new LinkItem(url, index.ToString()));
                }
            }

            if (LinkItems[0].Text != "1")
            {
                if (LinkItems.Count + 1 != TotalPages)
                {
                    LinkItems.Insert(0, new LinkItem(null, "...", true));
                }
                LinkItems.Insert(0, new LinkItem(GetUrl(baseUrl, 1, PageSize), "1"));
            }

            if (CurrentPage + 3 < TotalPages)
            {
                if (CurrentPage + 1 != TotalPages)
                {
                    LinkItems.Add(new LinkItem(null, "...", true));
                }
                LinkItems.Add(new LinkItem(GetUrl(baseUrl, TotalPages, PageSize), totalPages.ToString()));
            }
        }

        public static string GetUrl(string baseUrl, int page, int pageSize)
        {
            var query = $"pageNumber={page}&pageSize={pageSize}";
            var hasQueryParameters = baseUrl.Contains('?');

            var queryToAppend = hasQueryParameters ? $"&{query}" : $"?{query}";

            return $"{baseUrl}{queryToAppend}";
        }

        public class LinkItem
        {
            public string? Url { get; set; }
            public string Text { get; set; }
            public bool HasLink => !string.IsNullOrEmpty(Url);
            public bool IsEllipsesLink { get; set; }
            public LinkItem(string? url, string text, bool isEllipsesLink = false)
            {
                Url = url;
                Text = text;
                IsEllipsesLink = isEllipsesLink;
            }
        }
    }
}
