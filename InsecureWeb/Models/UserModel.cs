using System;

namespace InsecureWeb.Models
{
	/// <summary>
	///		Modelo con los datos de un usuario
	/// </summary>
	public class UserModel
	{
		/// <summary>
		///		Id del usuario
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///		Nombre de usuario
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		///		Contraseña de usuario
		/// </summary>
		public string Password { get; set; }
	}
}