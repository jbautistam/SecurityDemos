using System;
using System.Web.Mvc;

using InsecureWeb.Models;
using InsecureWeb.Repository;

namespace InsecureWeb.Controllers
{
	/// <summary>
	///		Controlador para el login
	/// </summary>
	public class LoginController : BaseController
	{
		public ActionResult Index()
		{
			ViewBag.Message = "";
			return View();
		}

		/// <summary>
		///		Inicio de sesión
		/// </summary>
        [HttpPost]  
        public ActionResult Login(UserModel user)   
        {  
			// Procesa el inicio de sesión
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password)) 
				ViewBag.Message = "Introduzca usuario y contraseña";
			else
            {  
				UserModel loaded = new UserRepository(GetConnectionString()).Load(user.UserName, user.Password);

					if (loaded == null || loaded.Id < 1)
						ViewBag.Message = "Error en los datos de usuario";
					else
                    {  
                        Session["UserID"] = loaded.Id.ToString();  
                        Session["UserName"] = loaded.UserName;
                        return RedirectToAction(nameof(HomeController.Index), "Home");  
                    }  
            }  
			// Devuelve la vista
            return View(nameof(Index));  
        } 

		/// <summary>
		///		Desconecta el usuario
		/// </summary>
        public ActionResult Logout()   
        {  
			// Limpia las variables de sesión
			Session["UserID"] = null;  
			Session["UserName"] = null;
			// Vuelve al menú principal
            return RedirectToAction(nameof(HomeController.Index), "Home");  
        }
	}
}