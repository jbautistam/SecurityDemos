using System;

namespace RatConsole.Domain.Models
{
	/// <summary>
	///		Clase con los datos de un usuario
	/// </summary>
	public class UserModel
	{
		public UserModel(string application)
		{
			Application = application;
		}

		/// <summary>
		///		Aplicación de la que se han obtenido los datos
		/// </summary>
		public string Application { get; }

		/// <summary>
		///		Código de usuario
		/// </summary>
		public string User { get; set; }

		/// <summary>
		///		Contraseña
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		///		Url
		/// </summary>
		public string Url { get; set; }
	}
}
