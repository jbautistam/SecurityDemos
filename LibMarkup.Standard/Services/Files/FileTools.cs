using System;
using System.Collections.Generic;
using System.IO;

namespace Bau.Libraries.LibMarkupLanguage.Services.Files
{
	/// <summary>
	///		Clase de ayuda para manejo de archivos
	/// </summary>
	internal class FileTools
	{
		/// <summary>
		///		Elimina un archivo (no tiene en cuenta las excepciones)
		/// </summary>
		internal static bool KillFile(string fileName)
		{	
			// Quita el atributo de s�lo lectura
			//! Est� en un try separado porque puede dar problemas de "Acceso denegado"
			try
			{ 
				File.SetAttributes(fileName, FileAttributes.Normal);
			}
			catch {}
			// Borra el archivo
			try
			{ 
				// Elimina el archivo
				File.Delete(fileName);
				// Indica que se ha borrado correctamente
				return true;
			}
			catch (Exception exception)
			{ 
				// Muestra el mensaje
				System.Diagnostics.Debug.WriteLine(exception.Message);
				// Indica que no se ha podido borrar
				return false;
			}
		}	

		/// <summary>
		///		Crea un directorio sin tener en cuenta las excepciones
		/// </summary>
		internal static bool MakePath(string path)
		{ 
			return MakePath(path, out string error);
		}

		/// <summary>
		///		Crea un directorio sin tener en cuenta las excepciones
		/// </summary>
		internal static bool MakePath(string path, out string error)
		{ 
			bool made = false;

				// Inicializa los argumentos de salida
				error = "";
				// Crea el directorio
				try
				{ 
					if (path.StartsWith("\\") || ExistsDrive(path))
					{ 
						if (Directory.Exists(path)) // ... si ya exist�a, intentar crearlo devolver�a un error
							made = true;
						else // ... intenta crea el directorio
						{ 
							Directory.CreateDirectory(path);
							made = true;
						}
					}
					else
						error = "No existe la unidad para crear: " + path;
				}
				catch (Exception exception)
				{ 
						error = "Error en la creaci�n del directorio " + path + Environment.NewLine +
									exception.Message + Environment.NewLine + exception.StackTrace;
				}
				// Devuelve el valor que indica si se ha creado
				return made;
		}

		/// <summary>
		///		Comprueba si existe una unidad
		/// </summary>
		private static bool ExistsDrive(string path)
		{ 
			// Comprueba si existe la unidad
			if (!string.IsNullOrEmpty(path))
			{ 
				DriveInfo [] drives = DriveInfo.GetDrives();
						  
					// Quita los espacios
					path = path.Trim();
					// Comprueba si existe la unidad
					foreach (DriveInfo drvDrive in drives)
						if (drvDrive.IsReady && path.StartsWith(drvDrive.Name, StringComparison.CurrentCultureIgnoreCase))
							return true;
			}
			// Si ha llegado hasta aqu� es porque la unidad no existe
			return false;
		}
		
		/// <summary>
		/// 	Carga un archivo de texto en una cadena
		/// </summary>
		internal static string LoadTextFile(string fileName)
		{ 
			return LoadTextFile(fileName, GetFileEncoding(fileName, System.Text.Encoding.UTF8));
		}

		/// <summary>
		/// 	Carga un archivo de texto con un encoding determinado en una cadena
		/// </summary>
		internal static string LoadTextFile(string fileName, System.Text.Encoding encoding)
		{	
			System.Text.StringBuilder content = new System.Text.StringBuilder();

				// Carga el archivo
				using (StreamReader file = new StreamReader(fileName, encoding))
				{ 
					string data;

						// Lee los datos
						while ((data = file.ReadLine()) != null)
						{ 
							// Le a�ade un salto de l�nea si es necesario
							if (content.Length > 0)
								content.Append("\n");
							// A�ade la l�nea le�da
							content.Append(data);
						}
						// Cierra el stream
						file.Close();
				}
				// Devuelve el contenido
				return content.ToString();
		}

		/// <summary>
		///		Obtiene la codificaci�n de un archivo
		/// </summary>
		private static System.Text.Encoding GetFileEncoding(string fileName, System.Text.Encoding encodingDefault = null)
		{ 
			System.Text.Encoding encoding;
			byte[] buffer = new byte[5];

				// Lee los primeros cinco bytes del archivo
				using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				{ 
					file.Read(buffer, 0, 5);
					file.Close();
				}
				// Obtiene la codificaci�n a partir de los bytes iniciales del archivo
				if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
					encoding = System.Text.Encoding.UTF8;
				else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
					encoding = System.Text.Encoding.Unicode;
				else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xFE && buffer[3] == 0xFF)
					encoding = System.Text.Encoding.UTF32;
				else if (buffer[0] == 0x2B && buffer[1] == 0x2F && buffer[2] == 0x76)
					encoding = System.Text.Encoding.UTF7;
				else if (encodingDefault == null)
					encoding = System.Text.Encoding.UTF8;
				else
					encoding = encodingDefault;
				// Devuelve la codificaci�n
				return encoding;
		}

		/// <summary>
		/// 	Graba una cadena en un archivo de texto
		/// </summary>
		internal static void SaveTextFile(string fileName, string text)
		{	
			SaveTextFile(fileName, text, System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// 	Graba una cadena en un archivo de texto
		/// </summary>
		internal static void SaveTextFile(string fileName, string text, string encoding)
		{	
			SaveTextFile(fileName, text, System.Text.Encoding.GetEncoding(encoding));
		}

		/// <summary>
		/// 	Graba una cadena en un archivo de texto
		/// </summary>
		internal static void SaveTextFile(string fileName, string text, System.Text.Encoding encoding)
		{	
			using (StreamWriter file = new StreamWriter(fileName, false, encoding))
			{ 
				// Escribe la cadena
				file.Write(text);
				// Cierra el stream
				file.Close();
			}
		}		
	}
}