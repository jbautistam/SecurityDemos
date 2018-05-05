using System;

namespace InsecureWeb.Models
{
	/// <summary>
	///		Modelo para los datos de log
	/// </summary>
	public class LogModel
	{
		/// <summary>
		///		Clave
		/// </summary>
		public int? LogId { get; set; }

		/// <summary>
		///		Agente del usuario
		/// </summary>
		public string UserAgent { get; set; }

		/// <summary>
		///		Url de la solicitud
		/// </summary>
		public string UrlRequest { get; set; }
	}
}