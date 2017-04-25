using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspModular.Infrastructure.Helpers
{
    public static class HtmlSanitizerHelper
    {
        private static readonly HtmlSanitizer HtmlSanitizer = HtmlSanitizer.Instance;

        public static HtmlString SanitizeHtml(this HtmlHelper helper, string text)
        {
            return HtmlSanitizer.ToHtmlString(text);
        }
    }
}
