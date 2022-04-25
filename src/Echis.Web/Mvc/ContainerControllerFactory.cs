using System;
using System.Web.Routing;
using System.Web;

namespace System.Web.Mvc
{
	/// <summary>
	/// Represents a Controller Factory which uses the Inversion Of Control Container.  If the controller is not found in the
	/// IOC Container, then the Default Controller Factory method is used.
	/// </summary>
	public class ContainerControllerFactory : DefaultControllerFactory
	{
		/// <summary>
		/// Retrieves the specified controller from the IOC Container,
		/// or if not found in the container, creates the specified controller using the specified request context.
		/// </summary>
		/// <param name="requestContext">The context of the HTTP Request, which includes the HTTP Context and Route Data.</param>
		/// <param name="controllerName">The ObjectId or name of the Controller</param>
		/// <returns>Returns the specified controller.</returns>
		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			return (IOC.Instance.ContainsObject(Settings.Values.ControllerContext, controllerName)) ?
				IOC.Instance.GetObjectAndInject<IController>(Settings.Values.ControllerContext, controllerName) : 
				base.CreateController(requestContext, controllerName);
		}

		/// <summary>
		/// Retrieves the controller instance for the specified request context and controller type.
		/// </summary>
		/// <param name="requestContext">The context of the HTTP Request, which includes the HTTP Context and Route Data.</param>
		/// <param name="controllerType">The type of the controller.</param>
		/// <returns>Returns the controller instance for the specified request context and controller type.</returns>
		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			IController controller = base.GetControllerInstance(requestContext, controllerType);
			IOC.Injector.InjectObjectDependencies(controller);
			return controller;
		}
	}
}
