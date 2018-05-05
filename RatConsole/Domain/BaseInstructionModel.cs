using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace RatConsole.Domain
{
	/// <summary>
	///		Clase base para las instrucciones
	/// </summary>
	public abstract class BaseInstructionModel
	{
		// Constantes privadas
		private const string TagId = "Id";
		private const string TagFrom = "From";
		private const string TagTo = "To";
		private const string TagDelay = "Delay";
		private const string TagNext = "Next";
		private const string TagTimes = "Times";
		private const string TagTimesExecuted = "TimesExecuted";

		public BaseInstructionModel(Controllers.ProgramManager manager)
		{
			Manager = manager;
		}

		/// <summary>
		///		Interpreta una instrucción
		/// </summary>
		public abstract BaseInstructionModel Parse(Bau.Libraries.LibMarkupLanguage.MLNode nodeML);

		/// <summary>
		///		Interpreta los datos básicos
		/// </summary>
		protected void ParseInternal(Bau.Libraries.LibMarkupLanguage.MLNode nodeML)
		{
			Id = nodeML.Attributes[TagId].Value;
			From = nodeML.Attributes[TagFrom].Value.GetDateTime();
			To = nodeML.Attributes[TagTo].Value.GetDateTime();
			Delay = TimeSpan.FromSeconds(nodeML.Attributes[TagDelay].Value.GetInt(0));
			Next = nodeML.Attributes[TagNext].Value.GetDateTime(From ?? DateTime.Now);
			Times = nodeML.Attributes[TagTimes].Value.GetInt(1);
			TimesExecuted = nodeML.Attributes[TagTimesExecuted].Value.GetInt(0);
		}

		/// <summary>
		///		Comprueba si se debe ejecutar la instrucción
		/// </summary>
		public bool MustExecute()
		{
			return TimesExecuted == 0 || (Next > DateTime.Now && (To == null || To > DateTime.Now));
		}

		/// <summary>
		///		Obtiene un nodo
		/// </summary>
		public abstract Bau.Libraries.LibMarkupLanguage.MLNode GetMLNode();

		/// <summary>
		///		Obtiene los datos básicos del nodo
		/// </summary>
		protected Bau.Libraries.LibMarkupLanguage.MLNode GetMLNodeInternal(string tag)
		{
			Bau.Libraries.LibMarkupLanguage.MLNode rootML = new Bau.Libraries.LibMarkupLanguage.MLNode(tag);

				// Añade los atributos
				rootML.Attributes.Add(TagId, Id);
				rootML.Attributes.Add(TagFrom, From);
				rootML.Attributes.Add(TagTo, To);
				rootML.Attributes.Add(TagDelay, Delay.TotalSeconds);
				rootML.Attributes.Add(TagNext, Next);
				rootML.Attributes.Add(TagTimes, Times);
				rootML.Attributes.Add(TagTimesExecuted, TimesExecuted);
				// Devuelve el nodo
				return rootML;
		}

		/// <summary>
		///		Ejecuta la instrucción
		/// </summary>
		public void Execute()
		{
			// Ejecuta la instrucción
			ExecuteInternal();
			// Indica que se ha ejecutado y calcula la fecha de siguiente
			TimesExecuted++;
			if (Delay.TotalSeconds > 0 || To != null)
				Next = Next.AddSeconds(Delay.TotalSeconds);
		}

		/// <summary>
		///		Ejecuta la instrucción
		/// </summary>
		protected abstract void ExecuteInternal();

		/// <summary>
		///		Comprueba si se puede ejecutar la instrucción
		/// </summary>
		public bool CanDelete()
		{
			return (To == null || Next > To) && TimesExecuted >= Times;
		}

		/// <summary>
		///		Manager de ejecución
		/// </summary>
		public Controllers.ProgramManager Manager { get; }

		/// <summary>
		///		Id de la instrucción
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///		Fecha de inicio
		/// </summary>
		public DateTime? From { get; set; }

		/// <summary>
		///		Fecha de fin
		/// </summary>
		public DateTime? To { get; set; }

		/// <summary>
		///		Intervalo entre ejecuciones
		/// </summary>
		public TimeSpan Delay { get; set; }

		/// <summary>
		///		Número de veces que se debe ejecutar
		/// </summary>
		public int Times { get; set; } = 1;

		/// <summary>
		///		Siguiente ejecución
		/// </summary>
		public DateTime Next { get; private set; } = DateTime.Now;

		/// <summary>
		///		Número de veces que se ha ejecutado
		/// </summary>
		public int TimesExecuted { get; private set; }
	}
}
