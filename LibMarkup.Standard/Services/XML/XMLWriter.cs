using System;
using System.Text;

namespace Bau.Libraries.LibMarkupLanguage.Services.XML
{
	/// <summary>
	///		Clase de ayuda para generaci�n de XML
	/// </summary>
	public class XMLWriter : IWriter
	{ 
		// Variables privadas
		private StringBuilder _sbXML = new StringBuilder();

		/// <summary>
		///		Graba los datos de un archivo XML
		/// </summary>
		public bool Save(string fileName, MLFile fileML)
		{
			return Save(fileName, fileML, out string error);
		}

		/// <summary>
		///		Graba los datos de un archivo XML
		/// </summary>
		public bool Save(string fileName, MLFile fileML, out string error)
		{
			// Inicializa los argumentos de salida
			error = string.Empty;
			// Graba el archivo
			Files.FileTools.MakePath(System.IO.Path.GetDirectoryName(fileName));
			try
			{	
				Files.FileTools.SaveTextFile(fileName, ConvertToString(fileML));
			}
			catch (Exception exception)
			{
				error = $"Error al grabar el archivo {fileName}. {exception.Message}";
			}
			// Devuelve el valor que indica si se ha grabado correctamente
			return string.IsNullOrEmpty(error);
		}

		/// <summary>
		///		Convierte los datos de un MLFile en una cadena
		/// </summary>
		public string ConvertToString(MLFile fileML, bool addHeader = true)
		{ 
			// Crea el stringBuilder del archivo
			Create(fileML, addHeader);
			// Devuelve la cadena
			return _sbXML.ToString();
		}

		/// <summary>
		///		Convierte los datos de un nodo a una cadena
		/// </summary>
		public string ConvertToString(MLNode nodeML)
		{ 
			// Limpia el contenido
			_sbXML.Clear();
			// A�ade la informaci�n del nodo y sus hijos
			Add(0, nodeML);
			// Devuelve la cadena
			return _sbXML.ToString();
		}

		/// <summary>
		///		Convierte los datos de una colecci�n de nodos en una cadena
		/// </summary>
		public string ConvertToString(MLNodesCollection nodesML)
		{ 
			// Limpia el contenido
			_sbXML.Clear();
			// A�ade la informaci�n del nodo y sus hijos
			Add(0, nodesML);
			// Devuelve la cadena
			return _sbXML.ToString();
		}

		/// <summary>
		///		Crea el texto de un archivo
		/// </summary>
		private void Create(MLFile file, bool addHeader)
		{ 
			// Limpia el contenido
			_sbXML.Clear();
			// A�ade la cabecera
			if (addHeader)
				_sbXML.Append("<?xml version='1.0' encoding='utf-8'?>" + Environment.NewLine);
			// A�ade los nodos
			Add(0, file.Nodes);
		}

		/// <summary>
		///		A�ade los nodos
		/// </summary>
		private void Add(int indent, MLNodesCollection nodesML)
		{
			foreach (MLNode nodeML in nodesML)
				Add(indent, nodeML);
		}

		/// <summary>
		///		A�ade los datos de un nodo
		/// </summary>
		private void Add(int indent, MLNode nodeML)
		{ 
			// Indentaci�n
			AddIndent(indent);
			// Cabecera
			_sbXML.Append("<");
			// Nombre
			AddName(nodeML);
			// Espacios de nombres
			Add(nodeML.NameSpaces);
			// Atributos
			Add(nodeML.Attributes);
			// Final y datos del nodo (en su caso)
			if (IsAutoClose(nodeML))
				_sbXML.Append("/>" + Environment.NewLine);
			else
			{ 
				// Cierre de la etiqueta de apertura
				_sbXML.Append(">");
				// Datos
				if (nodeML.Nodes.Count > 0)
				{
					_sbXML.Append(Environment.NewLine);
					Add(indent + 1, nodeML.Nodes);
				}
				else if (!string.IsNullOrEmpty(nodeML.Value))
				{
					if (nodeML.Value.IndexOf("<![CDATA[") > 0) // ... si la cadena ten�a ya un CDATA
						Add(indent + 1, nodeML.Value);
					else
						_sbXML.Append(EncodeHTML(nodeML.Value));
				}
				// Cierre
				_sbXML.Append("</");
				AddName(nodeML);
				_sbXML.Append(">" + Environment.NewLine);
			}
		}

		/// <summary>
		///		A�ade la indentaci�n
		/// </summary>
		private void AddIndent(int indent)
		{
			for (int index = 0; index < indent; index++)
				_sbXML.Append("\t");
		}

		/// <summary>
		///		A�ade un texto con indentaci�n
		/// </summary>
		private void Add(int indent, string text)
		{ 
			// A�ade la indentaci�n
			AddIndent(indent);
			// A�ade el texto
			_sbXML.Append(text);
		}

		/// <summary>
		///		A�ade el nombre de un elemento
		/// </summary>
		private void AddName(MLItemBase nodeML)
		{   
			// Espacio de nombres
			if (!string.IsNullOrEmpty(nodeML.Prefix))
				_sbXML.Append($"{nodeML.Prefix}:");
			// Nombre
			_sbXML.Append(nodeML.Name);
		}

		/// <summary>
		///		Espacios de nombres
		/// </summary>
		private void Add(MLNameSpacesCollection nameSpaces)
		{
			foreach (MLNameSpace nameSpace in nameSpaces)
			{ 
				// Nombre
				_sbXML.Append(" xmlns");
				if (!string.IsNullOrEmpty(nameSpace.Prefix))
					_sbXML.Append($":{nameSpace.Prefix}");
				// Atributos
				_sbXML.Append($" = \"{nameSpace.NameSpace}\" ");
			}
		}

		/// <summary>
		///		Atributos
		/// </summary>
		private void Add(MLAttributesCollection attributes)
		{
			foreach (MLAttribute attribute in attributes)
				_sbXML.Append($" {attribute.Name} = \"{EncodeHTML(attribute.Value)}\" ");
		}

		/// <summary>
		///		Codifica una cadena HTML quitando los caracteres raros
		/// </summary>
		private string EncodeHTML(string value)
		{ 
			// Quita los caracteres raros
			if (!string.IsNullOrEmpty(value))
			{
				value = value.Replace("&", "&amp;");
				value = value.Replace("<", "&lt;");
				value = value.Replace(">", "&gt;");
				value = value.Replace("\"", "&quot;");
				//value = value.Replace("�", "&aacute;");
				//value = value.Replace("�", "&eacute;");
				//value = value.Replace("�", "&iacute;");
				//value = value.Replace("�", "&oacute;");
				//value = value.Replace("�", "&uacute;");
			}
			// Devuelve la cadena
			return value;
		}

		/// <summary>
		///		Indica que es un nodo que se debe autocerrar
		/// </summary>
		private bool IsAutoClose(MLNode nodeML)
		{
			return string.IsNullOrEmpty(nodeML.Value) && (nodeML.Nodes == null || nodeML.Nodes.Count == 0);
		}
	}
}
