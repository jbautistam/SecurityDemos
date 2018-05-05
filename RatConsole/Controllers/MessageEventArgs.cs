using System;

namespace RatConsole.Controllers
{
	/// <summary>
	///		Argumentos del evento de mensajes de la aplicación
	/// </summary>
	public class MessageEventArgs : EventArgs
	{
		public MessageEventArgs(string message, bool error = false)
		{
			Message = message;
			Error = error;
		}

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; }

		/// <summary>
		///		Indica si es un mensaje de error
		/// </summary>
		public bool Error { get; }
	}
}
