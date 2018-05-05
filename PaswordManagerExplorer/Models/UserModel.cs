using System;

namespace PaswordManagerExplorer.Models
{
	/// <summary>
	///		Clase con los datos de un usuario
	/// </summary>
	public class UserModel
	{
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
