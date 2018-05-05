using System;
using System.Collections.Generic;

namespace WebProxy.Controllers.Redirections
{
	/// <summary>
	///		Clase con urls prohibidas
	/// </summary>
	public class BlackList : AbstractSitesList
	{
		/// <summary>
		///		Añade un sitio a la colección
		/// </summary>
		public void Add(string site)
		{
			if (!string.IsNullOrWhiteSpace(site) && !Sites.Contains(Normalize(site)))
				Sites.Add(site);
		}

		/// <summary>
		///		Indica si un sitio está en la lista de elementos prohibidos
		/// </summary>
		public bool IsForbidden(string site)
		{
			return Sites.Contains(Normalize(site));
		}

		/// <summary>
		///		Limpia la lista
		/// </summary>
		public void Clear()
		{
			Sites.Clear();
		}

		/// <summary>
		///		Sitios prohibidos
		/// </summary>
		private HashSet<string> Sites { get; } = new HashSet<string>();
	}
}
