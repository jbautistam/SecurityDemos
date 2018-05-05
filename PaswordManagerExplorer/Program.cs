using System;

namespace PaswordManagerExplorer
{
	class Program
	{
		static void Main(string[] args)
		{
			// Obtiene y muestra las contraseñas de Chrome
			Console.WriteLine("Parsing Chrome passwords");
			foreach (var user in new Services.ChromePasswordReader().GetPasswords())
				Console.WriteLine($"Url {user.Url} - User: {user.User} - Password: {user.Password}");
			// Detiene la consola
			Console.WriteLine();
			Console.WriteLine("Press any key...");
			Console.ReadKey();
		}
	}
}
