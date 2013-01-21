using System.Collections.Generic;
using CodicePlastico.Framework.Web.Localization;
using Moq;
using Xunit;

namespace CodicePlastico.Framework.Web.Fixture.Localization
{
    public class TranslationHelperFixture
    {
        private readonly Mock<ITranslationsRepository> _repository;

        public TranslationHelperFixture()
        {
            _repository = new Mock<ITranslationsRepository>();
        }


        [Fact]
        public void Initialize_ShouldSetCurrentLanguage()
        {
            TranslationHelper.Initialize("IT", _repository.Object);
            Assert.Equal("IT", TranslationHelper.DefaultLanguage);
        }

        [Fact]
        public void Translate_ShouldReturnTheCorrectTranslation()
        {
            UserIdProvider.SetCurrent(new FakeUserIdProvider(1, "IT"));
            
            IEnumerable<Translation> words = new List<Translation>{new Translation{Key = "Hello", Value = "Ciao"}, new Translation{Key = "Home", Value = "Casa"}};
            _repository.Setup(r => r.GetAllStringsForLanguage("IT")).Returns(words);
            TranslationHelper.Initialize("IT", _repository.Object);

            string word = TranslationHelper.Translate("Hello");
            Assert.Equal("Ciao", word);
        }

        [Fact]
        public void Translate_WordDoesNotExists_ShouldReturnTheKey()
        {
            UserIdProvider.SetCurrent(new FakeUserIdProvider(1, "IT"));
            IEnumerable<Translation> words = new List<Translation> { new Translation { Key = "Hello", Value = "Ciao" }, new Translation { Key = "Home", Value = "Casa" } };
            _repository.Setup(r => r.GetAllStringsForLanguage("IT")).Returns(words);
            TranslationHelper.Initialize("IT", _repository.Object);

            string word = TranslationHelper.Translate("Customer");
            Assert.Equal("Customer", word);
        }

        [Fact]
        public void Translate_CurrentUserHasALanguage_ShouldUseUserLanguage()
        {
            TranslationHelper.Initialize("IT", _repository.Object);

            UserIdProvider.SetCurrent(new FakeUserIdProvider(1, "FR"));
            
            IEnumerable<Translation> words = new List<Translation> { new Translation { Key = "Hello", Value = "Bonjour" }, new Translation { Key = "Home", Value = "Accueil" } };
            _repository.Setup(r => r.GetAllStringsForLanguage("FR")).Returns(words);

            string word = TranslationHelper.Translate("Home");

            Assert.Equal("Accueil", word);

        }


        [Fact]
        public void Translate_CurrentUserHasALanguageAndTheDictionaryIsAlreadyLoaded_ShouldUseTheLoadedDictionary()
        {
            TranslationHelper.Initialize("IT", _repository.Object);

            FakeUserIdProvider fakeUser = new FakeUserIdProvider(1, "FR");
            UserIdProvider.SetCurrent(fakeUser);
            IEnumerable<Translation> words = new List<Translation> { new Translation { Key = "Hello", Value = "Bonjour" }, new Translation { Key = "Home", Value = "Accueil" } };
            _repository.Setup(r => r.GetAllStringsForLanguage("FR")).Returns(words);

            TranslationHelper.Translate("Home");
            TranslationHelper.Translate("Hello");

            _repository.Verify(r => r.GetAllStringsForLanguage("FR"), Times.Once());
        }



    }
}