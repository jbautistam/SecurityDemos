using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InsecureWeb.Repository
{
	/// <summary>
	///		Clase base para los repositorios
	/// </summary>
	public abstract class SqlConnectionRepository<TypeData> : IDisposable where TypeData : class
	{
		// Variables privadas
		private SqlConnection _connection = null;
		
		protected SqlConnectionRepository(string connectionString)
		{
			ConnectionString = connectionString;
		}

		/// <summary>
		///		Abre la conexión
		/// </summary>
		protected void Open()
		{
			if (Connection.State != ConnectionState.Open)
				Connection.Open();
		}

		/// <summary>
		///		Cierra la conexión
		/// </summary>
		protected void Close()
		{
			if (_connection != null && Connection.State == ConnectionState.Open)
			{
				Connection.Close();
				Connection.Dispose();
				_connection = null;
			}
		}

		/// <summary>
		///		Carga los datos de un elemento
		/// </summary>
		protected virtual TypeData LoadObject(string sql, Dictionary<string, object> parametersDB, CommandType commandType = CommandType.Text) 
		{
			TypeData data = null;

				// Abre la conexión
				Open();
				// Lee los datos
				using (IDataReader reader = ExecuteReader(sql, parametersDB, commandType))
				{ 
					// Lee los datos
					if (reader.Read())
						data = AssignData(reader);
					else
						data = AssignData(null);
					// Cierra el recordset
					reader.Close();
				}
				// Cierra la conexión
				Close();
				// Devuelve el objeto
				return data;
		}
		/// <summary>
		///		Carga los datos de un elemento
		/// </summary>
		protected virtual List<TypeData> LoadCollection(string sql, Dictionary<string, object> parametersDB, CommandType commandType = CommandType.Text) 
		{
			var data = new List<TypeData>();

				// Abre la conexión
				Open();
				// Lee los datos
				using (IDataReader reader = ExecuteReader(sql, parametersDB, commandType))
				{ 
					// Lee los datos
					while (reader.Read())
						data.Add(AssignData(reader));
					// Cierra el recordset
					reader.Close();
				}
				// Cierra la conexión
				Close();
				// Devuelve el objeto
				return data;
		}

		/// <summary>
		///		Obtiene un DataReader
		/// </summary>
		protected IDataReader ExecuteReader(string text, Dictionary<string, object> parametersDB, CommandType commandType)
		{
			IDataReader reader = null; // ... supone que no se puede abrir el dataReader			

				// Obtiene el reader
				using (IDbCommand command = new SqlCommand(text, Connection))
				{ 
					try
					{
						// Indica el tipo de comando
						command.CommandType = commandType;
						// Añade los parámetros
						AddParameters(command, parametersDB);
						// Obtiene el dataReader
						reader = command.ExecuteReader();
					}
					catch (Exception exception)
					{
						throw new Exception($"Error when execute SQL: {text}", exception);
					}
				}
				// Devuelve el dataReader
				return reader;
		}

		/// <summary>
		///		Ejecuta un comando
		/// </summary>
		protected void Execute(string text, Dictionary<string, object> parametersDB, CommandType commandType)
		{
			// Abre la conexión
			Open();
			// Ejecuta el comando
			using (IDbCommand command = new SqlCommand(text, Connection))
			{ 
				// Indica el tipo de comando
				command.CommandType = commandType;
				// Añade los parámetros
				AddParameters(command, parametersDB);
				// Ejecuta el comando
				command.ExecuteNonQuery();
			}
		}

		/// <summary>
		///		Añade a un comando los parámetros de una clase <see cref="ParametersDBCollection"/>
		/// </summary>
		protected void AddParameters(IDbCommand command, Dictionary<string, object> parameters)
		{ 
			// Limpia los parámetros antiguos
			command.Parameters.Clear();
			// Añade los parámetros nuevos
			if (parameters != null)
				foreach (KeyValuePair<string, object> parameter in parameters)
					command.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
		}

		/// <summary>
		///		Lee los datos de un <see cref="IDataReader"/>
		/// </summary>
		protected abstract TypeData AssignData(IDataReader reader);

		/// <summary>
		///		Libera la conexión
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed)
			{
				// Cierra la conexión
				if (disposing)
					Close();
				// Indica que se ha liberado
				Disposed = true;
			}
		}

		/// <summary>
		///		Libera la conexión
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		///		Cadena de conexión
		/// </summary>
		public string ConnectionString { get; }

		/// <summary>
		///		Conexión
		/// </summary>
		public SqlConnection Connection
		{
			get
			{
				// Crea la conexión si no existía
				if (_connection == null)
					_connection = new SqlConnection(ConnectionString);
				// Devuelve la conexión
				return _connection;
			}
		}

		/// <summary>
		///		Indica si se ha liberado la conexión
		/// </summary>
		public bool Disposed { get; private set; }
	}
}