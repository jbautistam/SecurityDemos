using System;
using System.Configuration;
using System.Web.Mvc;

namespace InsecureWeb.Controllers
{
	/// <summary>
	///		Base para los controladores
	/// </summary>
	public abstract class BaseController : Controller
	{
		/// <summary>
		///		Obtiene la cadena de conexión de la configuración
		/// </summary>
		protected string GetConnectionString()
		{
			return ConfigurationManager.AppSettings["ConnectionString"];
		}
	}
}