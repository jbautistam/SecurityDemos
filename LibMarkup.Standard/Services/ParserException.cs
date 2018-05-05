using System;

namespace Bau.Libraries.LibMarkupLanguage.Services
{
	/// <summary>
	///		Excepciones del intérprete
	/// </summary>
	public class ParserException : Exception
	{
		public ParserException(string message) : base(message) { }

		public ParserException(string message, Exception innerException) : base(message, innerException) { }
	}
}