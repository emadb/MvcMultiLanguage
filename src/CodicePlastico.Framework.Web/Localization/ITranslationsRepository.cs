using System;
using System.Collections.Generic;

namespace CodicePlastico.Framework.Web.Localization
{
    public interface ITranslationsRepository
    {
        IEnumerable<Translation> GetAllStringsForLanguage(string val);
    }
}