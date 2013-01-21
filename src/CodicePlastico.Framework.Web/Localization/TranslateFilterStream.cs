using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CodicePlastico.Framework.Web.Localization;

namespace CodicePlastico.Framework.Web.Localization
{
    public class TranslateFilter : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            // TODO: Hack per gestire il download dei file
            if (filterContext.Result.GetType() != typeof(FileContentResult))
            {
               filterContext.HttpContext.Response.Filter = new TranslateFilterStream(filterContext.HttpContext.Response.Filter);
            }
        }
    }


    public class TranslateFilterStream : MemoryStream
    {
        private readonly Stream _outputStream;

        public TranslateFilterStream(Stream output)
        {
            _outputStream = output;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string html = UTF8Encoding.UTF8.GetString(buffer);
            string translate = Translate(html);
            _outputStream.Write(UTF8Encoding.UTF8.GetBytes(translate), offset, UTF8Encoding.UTF8.GetByteCount(translate));
        }

        public string Translate(string html)
        {
            string result = Regex.Replace(html, TranslationHelper.Regex, TranslateAction, RegexOptions.Multiline);
            return result;
        }

        private string TranslateAction(Match match)
        {
            string key = match.Groups["word"].Value;
            string translation = TranslationHelper.Translate(key);
            return translation;
        }
    }
}