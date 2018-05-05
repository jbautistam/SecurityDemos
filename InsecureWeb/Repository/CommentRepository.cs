using System;
using System.Data;

using InsecureWeb.Models;

namespace InsecureWeb.Repository
{
	/// <summary>
	///		Repository de <see cref="CommentModel"/>
	/// </summary>
	public class CommentRepository : SqlConnectionRepository<CommentModel>
	{
		public CommentRepository(string connectionString) : base(connectionString) {}

		/// <summary>
		///		Consulta un registro
		/// </summary>
		public CommentModel Load(string id)
		{
			return LoadObject($@"SELECT CommentId, Subject, Body, Url
								   FROM Comments
								   WHERE CommentId = '{id}'",
							  null, CommandType.Text);
		}

		/// <summary>
		///		Carga una lista de comentarios
		/// </summary>
		public System.Collections.Generic.List<CommentModel> LoadIndex(string subject)
		{
			if (string.IsNullOrEmpty(subject))
				return LoadCollection($@"SELECT CommentId, Subject, Body, Url
										   FROM Comments
										   WHERE Subject LIKE '%{subject}%'",
									  null, CommandType.Text);
			else
				return LoadCollection($@"SELECT CommentId, Subject, Body, Url
										   FROM Comments
										   WHERE Subject LIKE '%{subject}%'",
									  null, CommandType.Text);
		}

		/// <summary>
		///		Graba un registro
		/// </summary>
		public void Save(CommentModel comment)
		{
			if (comment.CommentId < 1)
				Execute($@"INSERT INTO Comments (Subject, Body, Url)
								VALUES ('{comment.Subject}', '{comment.Body}', '{comment.Url}')",
						null, CommandType.Text);
			else
				Execute($@"UPDATE Comments 
							  SET Subject = '{comment.Subject}',
								  Body = '{comment.Body}',
								  Url = '{comment.Url}'
							WHERE CommentId = '{comment.CommentId}'",
						null, CommandType.Text);

		}

		/// <summary>
		///		Asigna los datos en un objeto
		/// </summary>
		protected override CommentModel AssignData(IDataReader reader)
		{
			CommentModel comment = new CommentModel();

				// Carga los datos
				if (reader != null)
				{
					comment.CommentId = reader.IisNull("CommentId", 0);
					comment.Subject = (string) reader.IisNull("Subject");
					comment.Body = (string) reader.IisNull("Body");
					comment.Url = (string) reader.IisNull("Url");
				}
				// Devuelve el objeto
				return comment;
		}

		/// <summary>
		///		Borra un comentario
		/// </summary>
		internal void Delete(int id)
		{
			Execute($@"DELETE FROM Comments
						WHERE CommentId = {id}",
					null, CommandType.Text);
		}
	}
}