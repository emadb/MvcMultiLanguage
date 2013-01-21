using Castle.Windsor;

namespace CodicePlastico.Framework.Web
{
	public class Container
	{
		private static IWindsorContainer _kernel;

		public static void Initialize()
		{
			_kernel = new WindsorContainer();
		}

		public static T Resolve<T>()
		{
			return _kernel.Resolve<T>();
		}
		
	    public static void Dispose()
	    {
	        _kernel.Dispose();
	    }
	}
}