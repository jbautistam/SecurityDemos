using System;
using System.Collections.Generic;
using System.Data;

using InsecureWeb.Models;

namespace InsecureWeb.Repository
{
	/// <summary>
	///		Repository de <see cref="LogModel"/>
	/// </summary>
	public class LogRepository : SqlConnectionRepository<LogModel>
	{
		public LogRepository(string connectionString) : base(connectionString) {}

		/// <summary>
		///		Carga los datos de log
		/// </summary>
		public List<LogModel> Load()
		{
			return LoadCollection(@"SELECT LogId, UserAgent, UrlRequest
									   FROM Logs",
								  null, CommandType.Text);
		}

		/// <summary>
		///		Lee los datos de un registro
		/// </summary>
		protected override LogModel AssignData(IDataReader reader)
		{
			LogModel log = new LogModel();

				// Lee el registro
				if (reader != null)
				{
					log.LogId = (int?) reader.IisNull("LogId");
					log.UserAgent = (string) reader.IisNull("UserAgent");
					log.UrlRequest = (string) reader.IisNull("UrlRequest");
				}
				// Devuelve el registro
				return log;
		}

		/// <summary>
		///		Inserta un registro de log
		/// </summary>
		public void Insert(string userAgent, string urlRequest)
		{
			Execute($@"INSERT INTO Logs (UserAgent, UrlRequest)
						VALUES ('{userAgent}', '{urlRequest}')",
					null, CommandType.Text);
		}
	}
}