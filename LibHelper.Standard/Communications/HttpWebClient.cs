using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bau.Libraries.LibCommonHelper.Communications
{
	/// <summary>
	///		Cliente de Http
	/// </summary>
    public class HttpWebClient
    {
		public HttpWebClient(string userAgent = null)
		{
			if (string.IsNullOrWhiteSpace(userAgent))
				UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
			else
				UserAgent = userAgent;
		}

		/// <summary>
		///		Obtiene el contenido de una URL
		/// </summary>
		public string HttpGet(string uri)
		{
			return HttpGetAsync(uri, CancellationToken.None).Result;
		}

		/// <summary>
		///		Obtiene el contenido de una URL
		/// </summary>
		public async Task<string> HttpGetAsync(string uri)
		{
			return await HttpGetAsync(uri, CancellationToken.None);
		}

		/// <summary>
		///		Obtiene el contenido de una URL
		/// </summary>
		public async Task<string> HttpGetAsync(string uri, CancellationToken token)
		{
			using (HttpClient client = GetHttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(new Uri(uri, UriKind.Absolute), token);

						return await response.Content.ReadAsStringAsync();
				}
				catch (Exception exception)
				{
					System.Diagnostics.Debug.WriteLine("Excepción: " + exception.Message);
					return "";
				}
			}
		}

		/// <summary>
		///		Obtiene el cliente Http
		/// </summary>
		private HttpClient GetHttpClient()
		{
			HttpClientHandler handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip };
			HttpClient client = new HttpClient(handler);

				// Añade las cabeceras predeterminadas
				client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
				// Devuelve el cliente
				return client;
		}

		/// <summary>
		///		Descripción del agente por el que se hace la petición http
		/// </summary>
		public string UserAgent { get; set; }
	}
}
