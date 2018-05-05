using System;

namespace WebProxy.Controllers.EventArguments
{
	/// <summary>
	///		Argumentos del evento de número de conexiones
	/// </summary>
	public class ConnectionCountEventArgs : EventArgs
	{
		public ConnectionCountEventArgs(int number)
		{
			Number = number;
		}

		/// <summary>
		///		Número de conexiones
		/// </summary>
		public int Number { get; }
	}
}
