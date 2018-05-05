using System;
using System.Diagnostics;
using Bau.Libraries.LibMarkupLanguage;

namespace RatConsole.Domain
{
	/// <summary>
	///		Instrucción para ejecutar un comando
	/// </summary>
	public class ProcessInstructionModel : BaseInstructionModel
	{
		// Constantes públicas
		public const string TagRoot = "Execute";
		private const string TagCommand = "Command";

		public ProcessInstructionModel(Controllers.ProgramManager manager) : base(manager) {}

		/// <summary>
		///		Interpreta el nodo
		/// </summary>
		public override BaseInstructionModel Parse(MLNode nodeML)
		{
			// Interpreta los datos básicos
			ParseInternal(nodeML);
			Command = nodeML.Nodes[TagCommand].Value;
			// Devuelve la instrucción interpretada
			return this;
		}

		/// <summary>
		///		Ejecuta la instrucción
		/// </summary>
		protected override void ExecuteInternal()
		{
			if (!string.IsNullOrEmpty(Command))
			{
				string fileName = System.IO.Path.GetTempFileName() + ".bat";

					// Log
					Manager.Log("Ejecutando un comando del shell");
					// Graba el comando en el archivo
					Bau.Libraries.LibCommonHelper.Files.HelperFiles.SaveTextFile(fileName, Command);
					// Ejecuta el archivo
					if (System.IO.File.Exists(fileName))
						ExecuteBat(fileName);
					// Elimina el archivo
					Bau.Libraries.LibCommonHelper.Files.HelperFiles.KillFile(fileName);
					// Log
					Manager.Log("Fin de ejecución del comando");
			}
		}

		/// <summary>
		///		Ejecuta un archivo bat
		/// </summary>
		private void ExecuteBat(string fileName)
		{
			Process.Start(new ProcessStartInfo
									{
										WindowStyle = ProcessWindowStyle.Maximized, // ProcessWindowStyle.Hidden,
										UseShellExecute = true,
										FileName = fileName
									}
						  );
		}

		/// <summary>
		///		Obtiene los datos del nodo
		/// </summary>
		public override MLNode GetMLNode()
		{
			MLNode nodeML = GetMLNodeInternal(TagRoot);

				// Añade los parámetros de la instrucción
				nodeML.Nodes.Add(TagCommand, Command);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Comando
		/// </summary>
		private string Command { get; set; }
	}
}
