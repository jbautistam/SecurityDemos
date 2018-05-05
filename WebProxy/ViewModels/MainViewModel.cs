using System;

using WebProxy.MVVM;

namespace WebProxy.ViewModels
{
	/// <summary>
	///		ViewModel de la ventana principal
	/// </summary>
	public class MainViewModel : BaseObservableObject, IDisposable
	{
		// Variables privadas
		private SessionListViewModel _listSessions;
		private string _request, _response;
		private int _clientConnectionCount, _serverConnectionCount;
		private bool _captureRequest;

		public MainViewModel(System.Windows.Threading.Dispatcher dispatcher)
		{
			// Inicializa los elementos del viewModel
			Sessions = new SessionListViewModel();
			Sessions.PropertyChanged += (sender, evntArgs) => 
											{
												if (evntArgs.PropertyName == nameof(Sessions.SelectedSession))
													GetSelectedSessionData();
											};
			// Inicializa el manager del proxy
			ProxyManager = new Controllers.ProxyServerController(dispatcher);
			ProxyManager.ClientConnectionCountChanged += (sender, args) => ClientConnectionCount = args.Number;
			ProxyManager.ServerConnectionCountChanged += (sender, args) => ServerConnectionCount = args.Number;
			ProxyManager.SessionAdded += (sender, args) => Sessions.Add(args);
			ProxyManager.SessionUpdated += (sender, args) => Sessions.UpdateSession(args);
			// Inicializa las listas de elementos bloqueados, redirecciones, etc...
			UpdateRedirections();
		}

		/// <summary>
		///		Modifica las redirecciones
		/// </summary>
		private void UpdateRedirections()
		{
			// Añade elementos a la lista negra
			ProxyManager.BlackList.Add("www.wikipedia.org");
			// Añade elementos a la lista de redirecciones
			ProxyManager.RedirectionsList.Add("www.paypal.es", new Uri("http://www.github.com"));
			// Añade elementos a la lista de sustitución
			ProxyManager.SubstitutionList.Add("www.jbautistam.com",
											  @"<!DOCTYPE html><html><body><h1>Updating the content</h1></body></html>");
		}

		/// <summary>
		///		Arranca el proceso
		/// </summary>
		public void Start()
		{
			ProxyManager.Start();	
		}

		/// <summary>
		///		Detiene el proceso
		/// </summary>
		public void Stop()
		{
			ProxyManager.Stop();
		}

		/// <summary>
		///		Modifica los datos de la sesión
		/// </summary>
		private void GetSelectedSessionData()
        {
            if (Sessions.SelectedSession == null)
            {
                Request = string.Empty;
				Response = string.Empty;
            }
			else
			{
				Request = Sessions.SelectedSession.Request;
				Response = Sessions.SelectedSession.Response;
			}
		}

		/// <summary>
		///		Indica al proxy que suplante o no las solicitudes
		/// </summary>
		private void CaptureRequestProxy(bool capture)
		{
			ProxyManager.CaptureRequest = capture;
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
				{
					Stop();
				}
				// Indica que se ha liberado
				Disposed = true;
			}
		}

		// TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
		// ~ProxyServerController() {
		//   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
		//   Dispose(false);
		// }

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
		///		Indica si se ha liberado el objeto
		/// </summary>
		public bool Disposed { get; private set; }

		/// <summary>
		///		Manager del proxy
		/// </summary>
		private Controllers.ProxyServerController ProxyManager { get; }

		/// <summary>
		///		Sesiones
		/// </summary>
		public SessionListViewModel Sessions
		{
			get { return _listSessions; }
			set { CheckObject(ref _listSessions, value); }
		}

		/// <summary>
		///		Datos de la solicitud
		/// </summary>
		public string Request
		{
			get { return _request; }
			set { CheckProperty(ref _request, value); }
		}

		/// <summary>
		///		Datos de la respuesta
		/// </summary>
		public string Response
		{
			get { return _response; }
			set { CheckProperty(ref _response, value); }
		}

		/// <summary>
		///		Número de conexiones a cliente
		/// </summary>
		public int ClientConnectionCount
		{
			get { return _clientConnectionCount; }
			set { CheckProperty(ref _clientConnectionCount, value); }
		}

		/// <summary>
		///		Número de conexiones a servidor
		/// </summary>
		public int ServerConnectionCount
		{
			get { return _serverConnectionCount; }
			set { CheckProperty(ref _serverConnectionCount, value); }
		}

		/// <summary>
		///		Indica si se debe capturar o no las solicitudes del usuario
		/// </summary>
		public bool CaptureRequest
		{
			get { return _captureRequest; }
			set 
			{ 
				if (CheckProperty(ref _captureRequest, value))
					CaptureRequestProxy(value);
			}
		}
	}
}
