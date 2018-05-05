using System;

namespace KeyLogger
{
	/// <summary>
	///		Argumentos del evento de pulsación de tecla
	/// </summary>
	internal class KeyLoggerEventArgs : EventArgs
	{
		internal KeyLoggerEventArgs(string windowTitle, int keyCode)
		{
			WindowTitle = windowTitle;
			KeyCode = keyCode;
		}

		/// <summary>
		///		Título de la ventana en la que se ha pulsado la tecla
		/// </summary>
		internal string WindowTitle { get; }

		/// <summary>
		///		Código de la tecla pulsada
		/// </summary>
		internal int KeyCode { get; }
	}
}
