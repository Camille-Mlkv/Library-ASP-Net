using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using WEB_253502_Melikava.Domain.Entities;

namespace WEB_253502_Melikava.UI.TagHelpers
{
    public class PagerTagHelper:TagHelper
    {
        private LinkGenerator _linkGenerator;
        private IHttpContextAccessor _httpContextAccessor;

        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; }

        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; }

        [HtmlAttributeName("current-genre")]
        public Genre CurrentGenre { get; set; }

        [HtmlAttributeName("admin")]
        public bool IsAdmin { get; set; }
        public PagerTagHelper(LinkGenerator linkGenerator,IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.Attributes.SetAttribute("aria-label", "Page navigation");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            // Previous button
            var prevPage = CurrentPage > 1 ? CurrentPage - 1 : 1;
            var prevItem = CreatePageItem("Previous", prevPage, CurrentPage == 1);
            ul.InnerHtml.AppendHtml(prevItem);

            // Page numbers
            for (int i = 1; i <= TotalPages; i++)
            {
                var pageItem = CreatePageItem(i.ToString(), i, CurrentPage == i);
                ul.InnerHtml.AppendHtml(pageItem);
            }

            // Next button
            var nextPage = CurrentPage < TotalPages ? CurrentPage + 1 : TotalPages;
            var nextItem = CreatePageItem("Next", nextPage, CurrentPage == TotalPages || TotalPages == 0);
            ul.InnerHtml.AppendHtml(nextItem);

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreatePageItem(string text, int pageNo, bool isDisabled)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (isDisabled)
            {
                li.AddCssClass("disabled");
            }

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");
            a.InnerHtml.Append(text);

            if (!isDisabled)
            {
                if (!IsAdmin)
                {
                    var genreName = CurrentGenre == null ? "" : CurrentGenre.NormalizedName;
                    var url = _linkGenerator.GetPathByPage(
                        httpContext: _httpContextAccessor.HttpContext,
                        page: null,  // Если это Razor Page, можно указать имя страницы, если нужно
                        values: new { pageNo = pageNo, genre = genreName }
                    );
                    a.Attributes["href"] = url;
                }
                else
                {
                    // Генерация пути для Razor Page в области Admin
                    var url = _linkGenerator.GetPathByPage(
                        httpContext: _httpContextAccessor.HttpContext,
                        page: "/Index",
                        values: new { pageNumber = pageNo, area = "Admin" }
                    );
                    a.Attributes["href"] = url;
                }
            }

            li.InnerHtml.AppendHtml(a);
            return li;
        }

       

    }
}
