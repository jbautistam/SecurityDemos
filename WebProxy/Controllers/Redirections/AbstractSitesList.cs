using System;

namespace WebProxy.Controllers.Redirections
{
	/// <summary>
	///		Clase abstracta para listas de sitios
	/// </summary>
	public abstract class AbstractSitesList
	{
		/// <summary>
		///		Normaliza una cadena
		/// </summary>
		protected string Normalize(string site)
		{
			// Le añade www. al nombre del sitio
			if (!site.StartsWith("www.", StringComparison.CurrentCultureIgnoreCase))
				site = $"www.{site}";
			// Devuelve el nombre del sitio en minúsculas
			return site.ToLower().Trim();
		}
	}
}
