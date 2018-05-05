using System;
using System.Web.Mvc;

namespace InsecureWeb.Models
{
	/// <summary>
	///		Clase con los datos de un comentario
	/// </summary>
	public class CommentModel
	{
		/// <summary>
		///		Clave principal
		/// </summary>
		public int CommentId { get; set; }

		/// <summary>
		///		Asunto
		/// </summary>
		[AllowHtml]
		public string Subject { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		[AllowHtml]
		public string Body { get; set; }

		/// <summary>
		///		Url
		/// </summary>
		public string Url { get; set; }
	}
}