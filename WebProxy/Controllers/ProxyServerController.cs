using System;
using System.Net;
using System.Threading.Tasks;

using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Helpers;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;
using WebProxy.Controllers.EventArguments;

namespace WebProxy.Controllers
{
	/// <summary>
	///		Controlador del servidor de proxy
	/// </summary>
	public class ProxyServerController : IDisposable
	{
		// Eventos
		public event EventHandler<ConnectionCountEventArgs> ClientConnectionCountChanged;
		public event EventHandler<ConnectionCountEventArgs> ServerConnectionCountChanged;
		public event EventHandler<NewSessionEventArgs> SessionAdded;
		public event EventHandler<UpdateSessionEventArgs> SessionUpdated;
		// Variables privadas
        private ProxyServer _proxyServer;
		private readonly System.Collections.Generic.Dictionary<HttpWebClient, WebSession.Session> _sessions = new System.Collections.Generic.Dictionary<HttpWebClient, WebSession.Session>();

		public ProxyServerController(System.Windows.Threading.Dispatcher dispatcher)
		{
			// Guarda el dispatcher de Wpf
			Dispatcher = dispatcher;
			// Crea el objeto
            _proxyServer = new ProxyServer();
			// Añade los manejadores de eventos
            _proxyServer.BeforeRequest += ProxyServer_BeforeRequest;
            _proxyServer.BeforeResponse += ProxyServer_BeforeResponse;
            _proxyServer.TunnelConnectRequest += ProxyServer_TunnelConnectRequest;
            _proxyServer.TunnelConnectResponse += ProxyServer_TunnelConnectResponse;
			// Asigna los manejadores de eventos
			_proxyServer.ClientConnectionCountChanged += (sender, args) 
						=> ClientConnectionCountChanged?.Invoke(this, 
																new ConnectionCountEventArgs(_proxyServer.ClientConnectionCount));
            _proxyServer.ServerConnectionCountChanged += (sender, args) 
						=> ServerConnectionCountChanged?.Invoke(this, 
																new ConnectionCountEventArgs(_proxyServer.ServerConnectionCount));
		}

		/// <summary>
		///		Arranca el proxy
		/// </summary>
		public void Start()
		{
            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true);

				// Prepara el proxy
				_proxyServer.TrustRootCertificate = true;
				_proxyServer.CertificateManager.TrustRootCertificateAsAdministrator();
				_proxyServer.ForwardToUpstreamGateway = true;
				// Añade los manejadores de eventos
				_proxyServer.BeforeRequest += OnRequest;
				// Arranca el servidor de proxy
				_proxyServer.Start();
				// Asigna el servidor
				_proxyServer.AddEndPoint(explicitEndPoint);
				_proxyServer.SetAsSystemProxy(explicitEndPoint, ProxyProtocolType.AllHttp);
		}

		/// <summary>
		///		Detiene el proxy
		/// </summary>
		public void Stop()
		{
			// Limpia las sesiones
			_sessions.Clear();
			// Desactiva el proxy
			_proxyServer.DisableSystemProxy(ProxyProtocolType.AllHttp);
			_proxyServer.DisableAllSystemProxies();
            _proxyServer.BeforeRequest -= OnRequest;
            _proxyServer.Stop();
            // Elimina los certificados
            _proxyServer.CertificateManager.RemoveTrustedRootCertificates();
		}

		/// <summary>
		///		Añade una sesión
		/// </summary>
		private async Task AddSessionAsync(SessionEventArgs e)
		{
			var session = new WebSession.Session(e.Id);
			var webRequest = e.WebSession.Request;
			var webResponse = e.WebSession.Response;

				// Asigna los datos a la sesión
				session.IsTunnelConnect = e is TunnelConnectSessionEventArgs;
				// Asigna los datos recibidos y enviados
				if (session.IsTunnelConnect || e.WebSession.Request.UpgradeToWebSocket)
				{
					e.DataReceived += (sender, args) => session.ReceivedDataCount += args.Count;
					e.DataSent += (sender, args) => session.SentDataCount += args.Count;
				}
				// Actualiza los datos de sesión
				await UpdateSessionDataAsync(session, e, true);
		}

		/// <summary>
		///		Modifica una sesión
		/// </summary>
		private async Task UpdateSessionAsync(SessionEventArgs e)
		{
			if (_sessions.TryGetValue(e.WebSession, out WebSession.Session session))
				await UpdateSessionDataAsync(session, e, false);
		}

		/// <summary>
		///		Consolida los datos de la sesión
		/// </summary>
		private async Task UpdateSessionDataAsync(WebSession.Session session, SessionEventArgs e, bool isNew)
		{
			var webRequest = e.WebSession.Request;
			var webResponse = e.WebSession.Response;

				// Código de estado, protocolo, proceso...
				session.StatusCode = webResponse?.StatusCode ?? 0;
				session.Uri = webRequest.RequestUri;
				session.ProcessId = e.WebSession.ProcessId.Value;
				// Tamaño del cuerpo
				session.BodySize = -1;
				if (!session.IsTunnelConnect && webResponse != null)
				{
					if (webResponse.ContentLength != -1)
						session.BodySize = webResponse.ContentLength;
					else if (webResponse.IsBodyRead && webResponse.Body != null)
						session.BodySize = webResponse.Body.Length;
				}
				// Obtiene los detalles de la solicitud y la respuesta
				session.Request = ReadRequestData(webRequest);
				session.Response = ReadResponseData(webResponse);
				// Trata la sesión nueva o modificada
				await Dispatcher.InvokeAsync(() =>
					{
						if (isNew)
						{
							// Añade la sesión al diccionario
							_sessions.Add(e.WebSession, session);
							// Lanza el evento
							SessionAdded?.Invoke(this, new NewSessionEventArgs(session));
						}
						else
						{
							// Lanza el evento
							SessionUpdated?.Invoke(this, new UpdateSessionEventArgs(session));
							// Elimina la sesión del diccionario
							_sessions.Remove(e.WebSession);
						}
					});
		}

		/// <summary>
		///		Lee el contenido de una solicitud
		/// </summary>
		private WebSession.RequestData ReadRequestData(Request webRequest)
		{
			WebSession.RequestData request = new WebSession.RequestData();

				// Cabeceras
				foreach (var header in webRequest.Headers)
					request.Headers.Add(header.Name, header.Value);
				// Obtiene el contenido
				request.HeaderText = webRequest.HeaderText;
				request.Encoding = webRequest.Encoding;
				request.Body = (webRequest.IsBodyRead ? webRequest.Body : null) ?? new byte[0];
				if ((webRequest as ConnectRequest)?.ClientHelloInfo != null)
					request.HelloInfo = (webRequest as ConnectRequest)?.ClientHelloInfo.ToString();
				// Devuelve los datos
				return request;
		}

		/// <summary>
		///		Lee el contenido de una respuesta
		/// </summary>
		private WebSession.RequestData ReadResponseData(Response webResponse)
		{
			WebSession.RequestData response = new WebSession.RequestData();

				// Cabeceras
				foreach (var header in webResponse.Headers)
					response.Headers.Add(header.Name, header.Value);
				// Obtiene el contenido
				response.HeaderText = webResponse.HeaderText;
				response.Encoding = webResponse.Encoding;
				response.Body = (webResponse.IsBodyRead ? webResponse.Body : null) ?? new byte[0];
				if ((webResponse as ConnectResponse)?.ServerHelloInfo != null)
					response.HelloInfo = (webResponse as ConnectResponse)?.ServerHelloInfo.ToString();
				// Devuelve la respuesta
				return response;
		}

		/// <summary>
		///		Lee el cuerpo de la sesión
		/// </summary>
		private async Task ReadBodyAsync(SessionEventArgs e)
		{
			// Lee los datos del cuerpo
			if (e.WebSession.Response.HasBody)
			{
				e.WebSession.Response.KeepBody = true;
				await e.GetResponseBody();
			}
			// Modifica los datos de la sesión
			await UpdateSessionAsync(e);
		}

		/// <summary>
		///		Intercepta, cancela, redirige o modifica las solicitudes
		/// </summary>
        private async Task OnRequest(object sender, SessionEventArgs e)
        {
			if (CaptureRequest)
			{
				if (BlackList.IsForbidden(e.WebSession.Request.Host))
					await ShowHtmlAsync(e, "<!DOCTYPE html><html><body><h1>Website Blocked</h1></body></html>");
				else if (RedirectionsList.MustRedirect(e.WebSession.Request.Host))
					await e.Redirect(RedirectionsList.GetUri(e.WebSession.Request.Host).ToString());
				else if (SubstitutionList.MustChange(e.WebSession.Request.Host))
					await ShowHtmlAsync(e, SubstitutionList.GetHtml(e.WebSession.Request.Host));
			}
        }

		/// <summary>
		///		Muestra el valor de una cadena HTML en respuesta a una solicitud
		/// </summary>
		private async Task ShowHtmlAsync(SessionEventArgs e, string html)
		{
			await e.Ok(html, new System.Collections.Generic.Dictionary<string, HttpHeader>());
		}

		/// <summary>
		///		Elimina el objeto
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed)
			{
				// Libera los objetos
				if (disposing)
					Stop();
				// Indica que se ha liberado
				Disposed = true;
			}
		}

		/// <summary>
		///		Elimina el objeto
		/// </summary>
		public void Dispose()
		{
			// No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
			Dispose(true);
			// TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
			// GC.SuppressFinalize(this);
		}

		/// <summary>
		///		Dispatcher de Windows
		/// </summary>
		private System.Windows.Threading.Dispatcher Dispatcher { get; }
		
		/// <summary>
		///		Indica si se deben interceptar las solicitudes Web
		/// </summary>
		public bool CaptureRequest { get; set; }

		/// <summary>
		///		Lista de sitios prohibidos
		/// </summary>
		public Redirections.BlackList BlackList { get; } = new Redirections.BlackList();

		/// <summary>
		///		Lista de redirecciones
		/// </summary>
		public Redirections.RedirectionsList RedirectionsList { get; } = new Redirections.RedirectionsList();

		/// <summary>
		///		Lista de sustituciones
		/// </summary>
		public Redirections.SubstitutionList SubstitutionList { get; } = new Redirections.SubstitutionList();

		/// <summary>
		///		Indica si se ha liberado el objeto
		/// </summary>
		public bool Disposed { get; private set; }

        private async Task ProxyServer_TunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e)
        {
			await AddSessionAsync(e);
        }

        private async Task ProxyServer_TunnelConnectResponse(object sender, SessionEventArgs e)
        {
			await UpdateSessionAsync(e);
        }

		private async Task ProxyServer_BeforeRequest(object sender, SessionEventArgs e)
        {
			// Añade los datos de la sesión
			await AddSessionAsync(e);
			// Recoge el cuerpo de la sesión
			await ReadBodyAsync(e);
        }

        private async Task ProxyServer_BeforeResponse(object sender, SessionEventArgs e)
        {
			await ReadBodyAsync(e);
        }
	}
}
