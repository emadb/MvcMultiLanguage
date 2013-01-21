using System.Collections.Generic;

namespace CodicePlastico.Framework.Web.Fixture
{
    public class FakeUserIdProvider : IUserIdProvider
    {
        private readonly int _id = 7777;
        private readonly string _language = "";

        public FakeUserIdProvider(int id, string language)
        {
            _id = id;
            _language = language;
        }


        public int Id
        {
            get { return _id; }
        }

        public IUser User
        {
            get { return new FakeUser(_id, _language); }

        }

    }

    public class FakeUser : IUser
    {
        public FakeUser(int id, string language)
        {
            Id = id;
            Language = language;
        }

        public int Id { get; set; }
        public string Language { get; private set; }
    }
}