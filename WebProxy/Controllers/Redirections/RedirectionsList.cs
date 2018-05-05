using System;
using System.Collections.Generic;

namespace WebProxy.Controllers.Redirections
{
	/// <summary>
	///		Clase con urls a redirigir
	/// </summary>
	public class RedirectionsList : AbstractSitesList
	{
		/// <summary>
		///		Añade un sitio a la colección
		/// </summary>
		public void Add(string site, Uri uri)
		{
			if (!string.IsNullOrWhiteSpace(site) && !Sites.ContainsKey(Normalize(site)))
				Sites.Add(site, uri);
		}

		/// <summary>
		///		Indica si se debe redireccionar la petición a un sitio
		/// </summary>
		internal bool MustRedirect(string site)
		{
			return Sites.ContainsKey(Normalize(site));
		}

		/// <summary>
		///		Obtiene la Url de redirección a un sitio
		/// </summary>
		public Uri GetUri(string site)
		{
			if (Sites.TryGetValue(Normalize(site), out Uri uri))
				return uri;
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
		private Dictionary<string, Uri> Sites { get; } = new Dictionary<string, Uri>();
	}
}
