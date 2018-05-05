using System;
using System.Collections.Generic;

namespace WebProxy.Controllers.Redirections
{
	/// <summary>
	///		Clase con urls a cambiar por HTML
	/// </summary>
	public class SubstitutionList : AbstractSitesList
	{
		/// <summary>
		///		Añade un sitio a la colección
		/// </summary>
		public void Add(string site, string html)
		{
			if (!string.IsNullOrWhiteSpace(site) && !Sites.ContainsKey(Normalize(site)))
				Sites.Add(site, html);
		}

		/// <summary>
		///		Indica si se debe cambiar el HTML de la petición a un sitio
		/// </summary>
		internal bool MustChange(string site)
		{
			return Sites.ContainsKey(Normalize(site));
		}

		/// <summary>
		///		Obtiene la cadena HTML que sustituye un sitio
		/// </summary>
		public string GetHtml(string site)
		{
			if (Sites.TryGetValue(Normalize(site), out string html))
				return html;
			else
				return null;
		}

		/// <summary>
		///		Limpia la lista
		/// </summary>
		public void Clear()
		{
			Sites.Clear();
		}

		/// <summary>
		///		Redirección
		/// </summary>
		private Dictionary<string, string> Sites { get; } = new Dictionary<string, string>();
	}
}
