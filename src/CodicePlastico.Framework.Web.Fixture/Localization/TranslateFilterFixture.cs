using System.Collections.Generic;
using System.IO;
using CodicePlastico.Framework.Web.Localization;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace CodicePlastico.Framework.Web.Fixture.Localization
{
    public class TranslateFilterFixture
    {
        private readonly Mock<ITranslationsRepository> _repository;
        private readonly IEnumerable<Translation> _dictionary;

        public TranslateFilterFixture()
        {
            UserIdProvider.SetCurrent(new FakeUserIdProvider(1, "IT"));

            _repository = new Mock<ITranslationsRepository>();

            _dictionary = new List<Translation>
            {
                new Translation{Key = "Hello", Value = "Ciao"},
                new Translation{Key = "Home", Value = "Casa"},
                new Translation{Key = "Mouse", Value = "Topo"},
                new Translation{Key = "Apple", Value = "Mela"},

            };
            _repository.Setup(r => r.GetAllStringsForLanguage("IT")).Returns(_dictionary);
            TranslationHelper.Initialize("IT", _repository.Object);
        }

        [Theory]
        [InlineData("|#Mouse#|", "Topo")]
        [InlineData("|#Mouse#||#Apple#|", "TopoMela")]
        [InlineData("|#NoMatch#|", "NoMatch")]
        [InlineData("Ciao si dice |#Hello#|.", "Ciao si dice Ciao.")]
        [InlineData("Ciao si dice |#Hello#| e casa si dice |#Home#|", "Ciao si dice Ciao e casa si dice Casa")]
        public void Translate_SingleWord_ShouldReturnTheTranslatedString(string input, string output)
        {
            TranslateFilterStream filter = new TranslateFilterStream(new MemoryStream());
            string result = filter.Translate(input);
            Assert.Equal(result, output);
        }
    }
}