using System;

namespace WebProxy.Controllers.WebSession
{
	/// <summary>
	///		Datos de una solicitud / respuesta
	/// </summary>
	public class RequestData
	{
		/// <summary>
		///		Codificación
		/// </summary>
		public System.Text.Encoding Encoding { get; set; }

		/// <summary>
		///		Información inicial
		/// </summary>
		public string HelloInfo { get; set; }

		/// <summary>
		///		Cabeceras
		/// </summary>
		public System.Collections.Generic.Dictionary<string, string> Headers { get; } = new System.Collections.Generic.Dictionary<string, string>();

		/// <summary>
		///		Texto de la cabecera
		/// </summary>
		public string HeaderText { get; set; }

		/// <summary>
		///		Cuerpo
		/// </summary>
		public byte[] Body { get; set; }
	}
}
