using System;

namespace WebProxy.Controllers.EventArguments
{
	/// <summary>
	///		Argumentos del evento de modificación de datos de una sesión
	/// </summary>
	public class UpdateSessionEventArgs : EventArgs
	{
		public UpdateSessionEventArgs(WebSession.Session session)
		{
			Session = session;
		}

		/// <summary>
		///		Datos de la sesión
		/// </summary>
		public WebSession.Session Session { get; }
	}
}
