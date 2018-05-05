using System;
using System.Data;

using InsecureWeb.Models;

namespace InsecureWeb.Repository
{
	/// <summary>
	///		Repositorio para <see cref="UserModel"/>
	/// </summary>
	public class UserRepository : SqlConnectionRepository<UserModel>
	{
		public UserRepository(string connectionString) : base(connectionString)
		{
		}

		/// <summary>
		///		Carga los datos de un usuario
		/// </summary>
		public UserModel Load(string user, string password)
		{
			return LoadObject($@"SELECT UserId, Name, Password
								   FROM Users
								   WHERE Name = '{user}'
									 AND Password = '{password}'",
							  null, CommandType.Text);
		}

		/// <summary>
		///		Asigna los datos de un registro
		/// </summary>
		protected override UserModel AssignData(IDataReader reader)
		{
			UserModel user = new UserModel();

				// Carga los datos
				if (reader != null)
				{
					user.Id = reader.IisNull("UserId", 0);
					user.UserName = (string) reader.IisNull("Name");
					user.Password = (string) reader.IisNull("Password");
				}
				// Devuelve el usuario leído
				return user;
		}
	}
}