using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace CodicePlastico.Framework.Web.Localization
{
    public class JsHttpHandler : DefaultHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            // TODO: move the script folder string out of here!
            if (context.Request.FilePath.Contains("/Scripts/Libs"))
            {
                base.ProcessRequest(context);
                return;
            }
            string js = File.ReadAllText(context.Server.MapPath(context.Request.FilePath));
            string result = Regex.Replace(js, TranslationHelper.Regex, TranslateAction, RegexOptions.Multiline);

            context.Response.Write(result);
        }

        private string TranslateAction(Match match)
        {
            string key = match.Groups["word"].Value;
            string translation = TranslationHelper.Translate(key);
            return translation;
        }
    }

}