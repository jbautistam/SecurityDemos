using System;
using System.Web.Mvc;

using InsecureWeb.Repository;

namespace InsecureWeb.Controllers
{
	/// <summary>
	///		Controlador para <see cref="LogModel"/>
	/// </summary>
	public class LogsController : BaseController
	{
		/// <summary>
		///		Lista los datos de log
		/// </summary>
		public ActionResult Index()
		{
			var repository = new LogRepository(GetConnectionString());

				// Inserta el log
				repository.Insert(Request.UserAgent, Request.Url.ToString());
				// Devuelve la vista
				return View(repository.Load());
		}
	}
}