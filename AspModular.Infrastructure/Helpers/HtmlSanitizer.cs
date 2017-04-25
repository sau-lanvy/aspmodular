using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspModular.Infrastructure.Helpers
{
    public interface ISanitizer
    {
        HtmlString ToHtmlString(string plainText);
    }

    public class HtmlSanitizer : ISanitizer
    {
        private static readonly Lazy<HtmlSanitizer> LazyHtmlSanitizer = new Lazy<HtmlSanitizer>(() => new HtmlSanitizer());

        public static HtmlSanitizer Instance { get { return LazyHtmlSanitizer.Value; } }

        public HtmlString ToHtmlString(string plainText)
        {
            return new HtmlString(plainText);
        }
    }
}
