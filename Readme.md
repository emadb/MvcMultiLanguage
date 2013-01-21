==A simple localization framework for ASP.NET MVC==

How to use it:
- tag your string to translate using |# and #| (for example: <input type="button" value="|#click me#|" />)
- call TranslationHelper.Initialize(defaultLanguage, repository) in your application_start
- register TranslateFilter as global filter