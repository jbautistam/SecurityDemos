using System;

namespace WebProxy.Controllers.EventArguments
{
	/// <summary>
	///		Argumentos para el evento de nueva sesión
	/// </summary>
	public class NewSessionEventArgs : EventArgs
	{
		public NewSessionEventArgs(WebSession.Session session)
		{
			Session = session;
		}

		/// <summary>
		///		Datos de la sesión
		/// </summary>
		public WebSession.Session Session { get; }
	}
}
