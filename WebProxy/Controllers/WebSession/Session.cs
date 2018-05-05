using System;

namespace WebProxy.Controllers.WebSession
{
	/// <summary>
	///		Clase con los datos de una sesión
	/// </summary>
	public class Session
	{
		public Session(Guid id)
		{
			Id = id;
		}

		/// <summary>
		///		Código de sesión
		/// </summary>
		public Guid Id { get; }

		/// <summary>
		///		Indica si es una conexión mediante túnel
		/// </summary>
		public bool IsTunnelConnect { get; set; }

		/// <summary>
		///		Código de estado
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		///		Url
		/// </summary>
		public Uri Uri { get; set; }

		/// <summary>
		///		Tamaño del cuerpo
		/// </summary>
        public long? BodySize { get; set; }

		/// <summary>
		///		Código de proces
		/// </summary>
        public int ProcessId { get; set; }

		/// <summary>
		///		Número de datos recibidos
		/// </summary>
        public long ReceivedDataCount { get; set; }

		/// <summary>
		///		Número de datos enviados
		/// </summary>
		public long SentDataCount { get; set; }

		/// <summary>
		///		Datos de la solicitud
		/// </summary>
		public RequestData Request { get; set; } = new RequestData();

		/// <summary>
		///		Datos de la respuesta
		/// </summary>
		public RequestData Response { get; set; } = new RequestData();
	}
}
