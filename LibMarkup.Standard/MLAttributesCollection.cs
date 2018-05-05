﻿using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Colección de <see cref="MLAttribute"/>
	/// </summary>
	public class MLAttributesCollection : MLItemsBaseCollection<MLAttribute>
	{
		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string GetDebug()
		{
			string debug = "";

				// Añade la cadena de atributos
				foreach (MLAttribute objAttribute in this)
				{ 
					// Añade el separador
					if (!string.IsNullOrEmpty(debug))
						debug += ",";
					// Añade el nombre y valor del atributo
					debug += $"{objAttribute.Name}: {objAttribute.Value}";
				}
				// Devuelve la cadena de atributos
				return debug;
		}
	}
}
