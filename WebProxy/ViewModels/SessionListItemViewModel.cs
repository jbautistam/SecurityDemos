using System;
using System.Linq;

using WebProxy.MVVM;
using WebProxy.Controllers.WebSession;

namespace WebProxy
{
	/// <summary>
	///		ViewModel con un elemento de la lista de sesiones
	/// </summary>
    public class SessionListItemViewModel : BaseObservableObject
    {
		// Constantes privadas
		private const int TruncateLimit = 1024;
		// Variables privadas
		private bool _isTunnelConnect;
        private string _tunnelText, _statusCode, _protocol, _host, _url;
		private int _port;
        private long? _bodySize;
        private string _process;
        private long _receivedDataCount, _sentDataCount;
		private string _request, _response;

		/// <summary>
		///		Inicializa el viewModel
		/// </summary>
		internal void InitViewModel(int index, Controllers.EventArguments.NewSessionEventArgs eventArgs)
		{
			// Inicializa las propiedades
			Number = index;
			// Obtiene los datos de la sesión
			Update(eventArgs.Session);
        }

		/// <summary>
		///		Modifica los datos con los datos de la sesión
		/// </summary>
		internal void Update(Session session)
		{
			// Guarda la sesión
			Session = session;
			// Inicializa las propiedades
			IsTunnelConnect = session.IsTunnelConnect;
			ReceivedDataCount = session.ReceivedDataCount;
            SentDataCount = session.SentDataCount;
			// Código de estado, protocolo, proceso...
			StatusCode = session.StatusCode == 0 ? "-" : session.StatusCode.ToString();
			Protocol = session.Uri.Scheme;
			Process = GetProcessDescription(session.ProcessId);
			// Host y url
			Host = session.Uri.Host;
			Url = session.Uri.AbsolutePath;
			Port = session.Uri.Port;
			// Tamaño del cuerpo
			BodySize = session.BodySize;
			// Obtiene los detalles de la solicitud y la respuesta
			Request = ReadData(session.Request, TruncateLimit);
			Response = ReadData(session.Response, TruncateLimit);
		}

		/// <summary>
		///		Lee el contenido de una solicitud
		/// </summary>
		private string ReadData(RequestData webRequest, int truncateLimit)
		{
            bool truncated = webRequest.Body.Length > truncateLimit;
			string body;

				// Trunca el array de datos final
				if (truncated)
					body = webRequest.Encoding.GetString(webRequest.Body.Take(truncateLimit).ToArray());
				else
					body = webRequest.Encoding.GetString(webRequest.Body.ToArray());
				// Obtiene el contenido
				//string hexStr = string.Join(" ", data.Select(x => x.ToString("X2")));
				return webRequest.HeaderText + body +
					   (truncated ? Environment.NewLine + $"Data is truncated after {truncateLimit} bytes" : null) + 
					   Environment.NewLine + webRequest.HelloInfo;
		}

		/// <summary>
		///		Obtiene los datos de un proceso
		/// </summary>
		private string GetProcessDescription(int processId)
        {
            try
            {
				return $"{System.Diagnostics.Process.GetProcessById(processId).ProcessName}:{processId}";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

		/// <summary>
		///		Indice del elemento
		/// </summary>
        public int Number { get; set; }

		/// <summary>
		///		Datos de la sesión
		/// </summary>
		private Session Session { get; set; }

		/// <summary>
		///		Indica si está conectado al túnel
		/// </summary>
        public bool IsTunnelConnect 
		{ 
			get { return _isTunnelConnect; }
			set 
			{ 
				CheckProperty(ref _isTunnelConnect, value);
				TunnelText = value ? "Yes" : "No";
			}
		}

		/// <summary>
		///		Texto que indica si está conectado al túnel
		/// </summary>
		public string TunnelText
		{
			get { return _tunnelText; }
			set { CheckProperty(ref _tunnelText, value); }
		}

		/// <summary>
		///		Código de estado
		/// </summary>
        public string StatusCode
        {
            get { return _statusCode; }
            set { CheckProperty(ref _statusCode, value); }
        }

		/// <summary>
		///		Protocolo
		/// </summary>
        public string Protocol
        {
            get { return _protocol; }
            set { CheckProperty(ref _protocol, value); }
        }

		/// <summary>
		///		Host
		/// </summary>
        public string Host
        {
            get { return _host; }
            set { CheckProperty(ref _host, value); }
        }

		/// <summary>
		///		Url
		/// </summary>
        public string Url
        {
            get { return _url; }
            set { CheckProperty(ref _url, value); }
        }

		/// <summary>
		///		Puerto
		/// </summary>
		public int Port
		{
			get { return _port; }
			set { CheckProperty(ref _port, value); }
		}

		/// <summary>
		///		Tamaño del cuerpo
		/// </summary>
        public long? BodySize
        {
            get { return _bodySize; }
            set { CheckProperty(ref _bodySize, value); }
        }

		/// <summary>
		///		Código de proceso
		/// </summary>
        public string Process
        {
            get { return _process; }
            set { CheckProperty(ref _process, value); }
        }

		/// <summary>
		///		Número de bytes recibidos
		/// </summary>
        public long ReceivedDataCount
        {
            get { return _receivedDataCount; }
            set { CheckProperty(ref _receivedDataCount, value); }
        }

		/// <summary>
		///		Número de bytes enviados
		/// </summary>
        public long SentDataCount
        {
            get { return _sentDataCount; }
            set { CheckProperty(ref _sentDataCount, value); }
        }

		/// <summary>
		///		Solicitud
		/// </summary>
		public string Request
		{
			get { return _request; }
			set { CheckProperty(ref _request, value); }
		}

		/// <summary>
		///		Respuesta
		/// </summary>
		public string Response
		{
			get { return _response; }
			set { CheckProperty(ref _response, value); }
		}
    }
}
