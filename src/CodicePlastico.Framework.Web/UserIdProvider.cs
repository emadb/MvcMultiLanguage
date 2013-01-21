using System.Web;

namespace CodicePlastico.Framework.Web
{
    public interface IUserIdProvider
    {
        int Id { get; }
        IUser User { get;  }
    }

    public class UserIdProvider : IUserIdProvider
    {
        private static IUserIdProvider _current;
        
        public int Id { get { return User.Id; } }

        public IUser User
        {
            get
            {
                if (HttpContext.Current != null)
                    return (IUser)HttpContext.Current.User.Identity;
                return new NullUser();
            }
        }

        public static IUserIdProvider Current
        {
            get { return _current ?? (_current = new UserIdProvider()); }
        }

        public static void SetCurrent(IUserIdProvider provider)
        {
            _current = provider;
        }
    }

    public class NullUser : IUser
    {
        public int Id
        {
            get { return -1; }
            set {}
        }

        public string Language
        {
            get { return "it"; }
        }
    }
}