using System;

namespace RatConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Controllers.ProgramManager manager = new Controllers.ProgramManager(GetBasePath(), GetUrlDownload());

				// Asigna el manejador de eventos
				manager.Message += (sender, arguments) => Log(arguments.Message, arguments.Error);
				// Arranca el manager de ejecución
				manager.Start();
				// Espera a que se pulse una tecla
				Console.ReadLine();
				// Detiene el manager de ejecución
				manager.Stop();
		}

		/// <summary>
		///		Escribe el mensaje de log
		/// </summary>
		private static void Log(string message, bool error)
		{
			if (error)
				Console.ForegroundColor = ConsoleColor.Red;
			else
				Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(message);
		}

		/// <summary>
		///		Obtiene el directorio base
		/// </summary>
		private static string GetBasePath()
		{
			return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}

		/// <summary>
		///		Obtiene la URL de descarga base
		/// </summary>
		private static string GetUrlDownload()
		{
			return Properties.Settings.Default.UrlDownload;
		}
	}
}
