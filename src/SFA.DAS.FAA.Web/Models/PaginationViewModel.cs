namespace SFA.DAS.FAA.Web.Models
{
    public class PaginationViewModel
    {
        public const string PreviousText = "Previous";
        public const string NextText = "Next";
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
        public string BaseUrl { get; init; }

        public List<LinkItem> LinkItems { get; set; } = new();


        public PaginationViewModel(int currentPage, int pageSize, int totalPages, string baseUrl)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            BaseUrl = baseUrl;

            if (totalPages == 0) return;

            var startPage =
                (totalPages < 7 || currentPage < 4)
                    ? 1
                    : CalculateStartPage(currentPage, totalPages);

            var range = Enumerable.Range(startPage, 6);

            if (currentPage > 1) LinkItems.Add(new LinkItem(GetUrl(baseUrl, currentPage - 1, pageSize), PreviousText));

            foreach (var r in range)
            {
                var url = r == currentPage ? null : GetUrl(baseUrl, r, pageSize);
                LinkItems.Add(new LinkItem(url, r.ToString()));
                if (r == totalPages) break;
            }

            if (currentPage < totalPages) LinkItems.Add(new LinkItem(GetUrl(baseUrl, currentPage + 1, pageSize), NextText));
        }

        private static int CalculateStartPage(int currentPage, int totalPages)
        {
            return currentPage > totalPages - 3
                ? totalPages - 5
                : currentPage - 2;
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
            public LinkItem(string? url, string text)
            {
                Url = url;
                Text = text;
            }
        }
    }
}
