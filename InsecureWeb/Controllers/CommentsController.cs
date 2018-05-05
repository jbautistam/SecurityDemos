using System;
using System.Web.Mvc;

using InsecureWeb.Models;
using InsecureWeb.Repository;

namespace InsecureWeb.Controllers
{
	/// <summary>
	///		Controlador para <see cref="CommentModel"/>
	/// </summary>
	public class CommentsController : BaseController
	{
		/// <summary>
		///		Lista los comentarios (permitiendo ataques tabnapping)
		/// </summary>
		public ActionResult Index(string subject)
		{
			return View(new CommentRepository(GetConnectionString()).LoadIndex(subject));
		}

		/// <summary>
		///		Lista los comentarios (sin permitir ataques tabnapping)
		/// </summary>
		public ActionResult IndexNoTabnapping(string subject)
		{
			return View(new CommentRepository(GetConnectionString()).LoadIndex(subject));
		}

		/// <summary>
		///		Inserta un comentario
		/// </summary>
		public ActionResult Edit(string id)
		{
			var comment = new CommentModel();

				// Carga el comentario
				if (!string.IsNullOrEmpty(id) && id != "0")
				{
					comment = new CommentRepository(GetConnectionString()).Load(id);
					ViewBag.ButtonTitle = "Modificar";
				}
				else
					ViewBag.ButtonTitle = "Añadir";
				// Devuelve la vista
				return View(comment);
		}

		/// <summary>
		///		Añade un comentario
		/// </summary>
        [HttpPost]
        public ActionResult Post(CommentModel comment)   
        {  
			// Graba el comentario
            if (string.IsNullOrEmpty(comment.Subject) || string.IsNullOrEmpty(comment.Body)) 
				ViewBag.Message = "Introduzca el asunto y el contenido del comentario";
			else
            {  
				// Inserta el registro
				new CommentRepository(GetConnectionString()).Save(comment);
				// Muestra la tabla
				return RedirectToAction(nameof(Index), "Comments");  
            }  
			// Devuelve la vista
            return View(nameof(Edit), comment.CommentId.ToString());
        } 

		/// <summary>
		///		Borra un comentario
		/// </summary>
		public ActionResult Delete(string id)
		{
			var comment = new CommentModel();

				// Carga el comentario
				if (!string.IsNullOrEmpty(id) && id != "0")
				{
					comment = new CommentRepository(GetConnectionString()).Load(id);
					return View(comment);
				}
				else
					return RedirectToAction(nameof(Index), "Comments");  
		}

		/// <summary>
		///		Borra un comentario
		/// </summary>
		[HttpPost]
		public ActionResult Delete(CommentModel comment)
		{
			// Borra el comentario
			if (comment.CommentId > 0)
				new CommentRepository(GetConnectionString()).Delete(comment.CommentId);
			// Devuelve la vista
			return RedirectToAction(nameof(Index), "Comments");  
		}
	}
}