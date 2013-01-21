using System.Collections.Generic;
using System.Linq;

namespace CodicePlastico.Framework.Web.Localization
{
    public class TranslationHelper
    {
        private static Dictionary<string, Dictionary<string, string>> _dictionaries;
        private static ITranslationsRepository _repository;
        public static string DefaultLanguage { get; private set; }

        public static string Regex
        {
            get { return @"\|\#(?<word>\s?[^#]+)#\|"; }
        }

        public static void Initialize(string language)
        {
            Initialize(language, Container.Resolve<ITranslationsRepository>());
        }

        public static void Initialize(string language, ITranslationsRepository repository)
        {
            DefaultLanguage = language;
            _repository = repository;
            _dictionaries = new Dictionary<string, Dictionary<string, string>>();
            IEnumerable<Translation> translations = repository.GetAllStringsForLanguage(language);
            var defaultTranslations = MapTranlationDictionary(translations);
            _dictionaries.Add(language, defaultTranslations);
        }

        public static string Translate(string word)
        {
            if (!string.IsNullOrEmpty(UserIdProvider.Current.User.Language))
            {
                SetupDictionary(UserIdProvider.Current.User.Language);
                return Translate(word, UserIdProvider.Current.User.Language);
            }
            return Translate(word, DefaultLanguage);
        }

        private static string Translate(string word, string language)
        {
            if (_dictionaries[language].ContainsKey(word))
                return _dictionaries[language][word];
            return word;
        }

        private static void SetupDictionary(string language)
        {
            if (!_dictionaries.ContainsKey(language))
            {
                IEnumerable<Translation> dictionary = _repository.GetAllStringsForLanguage(language);
                var translations = MapTranlationDictionary(dictionary);
                _dictionaries.Add(language, translations);
            }
        }

        private static Dictionary<string, string> MapTranlationDictionary(IEnumerable<Translation> tr)
        {
            return tr.ToDictionary(translation => translation.Key, translation => translation.Value);
        }
    }
}