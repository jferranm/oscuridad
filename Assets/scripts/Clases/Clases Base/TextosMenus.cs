using System;
using System.Collections.Generic;
using System.Collections;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
	/// <summary>
	/// Clase base para la Traduccion de los textos de los menus
	/// </summary>
	[System.Serializable]
	public class TextosMenus
	{
		private string cargando;
		public string Cargando
		{
			get { return cargando; }
			set { cargando = value; }
		}

		private string btComenzar;
		public string BtComenzar 
		{
			get { return btComenzar; }
			set { btComenzar = value; }
		}

		private string btContinuar;
		public string BtContinuar 
		{
			get { return btContinuar; }
			set { btContinuar = value; }
		}

		private string btOpciones;
		public string BtOpciones 
		{
			get { return btOpciones; }
			set { btOpciones = value; }
		}

		private string btSalir;
		public string BtSalir 
		{
			get { return btSalir; }
			set { btSalir = value; }
		}

		private string btPartidaNueva;
		public string BtPartidaNueva 
		{
			get { return btPartidaNueva; }
			set { btPartidaNueva = value; }
		}

		private string toggleSonido;
		public string ToggleSonido 
		{
			get { return toggleSonido; }
			set { toggleSonido = value; }
		}

		private string toggleMusica;
		public string ToggleMusica 
		{
			get { return toggleMusica; }
			set { toggleMusica = value; }
		}

		private string btIdioma;
		public string BtIdioma 
		{
			get { return btIdioma; }
			set { btIdioma = value;	}
		}

		private string exito;
		public string Exito 
		{
			get { return exito;	}
			set { exito = value; }
		}

		private string fracaso;
		public string Fracaso 
		{
			get { return fracaso;	}
			set { fracaso = value; }
		}

		private string tirada;
		public string Tirada
		{
			get { return tirada;	}
			set { tirada = value; }
		}

		private string localizacionDescubierta;
		public string LocalizacionDescubierta
		{
			get { return localizacionDescubierta;	}
			set { localizacionDescubierta = value; }
		}

		private string inspeccionando;
		public string Inspeccionando
		{
			get { return inspeccionando;	}
			set { inspeccionando = value; }
		}

		private string objetoInventario;
		public string ObjetoInventario
		{
			get { return objetoInventario;	}
			set { objetoInventario = value; }
		}

		private string btMarlaWibbs;
		public string BtMarlaWibbs
		{
			get { return btMarlaWibbs;	}
			set { btMarlaWibbs = value; }
		}

		private string btRobertDuncan;
		public string BtRobertDuncan
		{
			get { return btRobertDuncan;	}
			set { btRobertDuncan = value; }
		}

		private string btWarrenBedford;
		public string BtWarrenBedford
		{
			get { return btWarrenBedford;	}
			set { btWarrenBedford = value; }
		}

		/// <summary>
		/// Constructor de la clase <see cref="TextosMenus"/>.
		/// </summary>
		public TextosMenus()
		{
			Cargando = string.Empty;
			BtComenzar = string.Empty;
			BtContinuar = string.Empty;
			BtOpciones = string.Empty;
			BtSalir = string.Empty;
			BtPartidaNueva = string.Empty;
			ToggleSonido = string.Empty;
			ToggleMusica = string.Empty;
			BtIdioma = string.Empty;
			BtMarlaWibbs = string.Empty;
			BtRobertDuncan = string.Empty;
			BtWarrenBedford = string.Empty;
		}

		public string DevolverTexto(string nombreControl)
		{
			string textoReturn = string.Empty;

			switch (nombreControl) 
			{
				case "Cargando":		textoReturn = Cargando;
										break;
				case "btComenzar": 		textoReturn = BtComenzar.ToUpper();
										break;
				case "btContinuar": 	textoReturn = BtContinuar.ToUpper();
										break;
				case "btOpciones": 		textoReturn = BtOpciones.ToUpper();
										break;
				case "btSalir": 		textoReturn = BtSalir.ToUpper();
										break;
				case "btPartidaNueva": 	textoReturn = BtPartidaNueva.ToUpper();
										break;
				case "toggleSonido": 	textoReturn = ToggleSonido.ToUpper();
										break;
				case "toggleMusica": 	textoReturn = ToggleMusica.ToUpper();
										break;
				case "btIdioma": 		textoReturn = BtIdioma.ToUpper();
										break;
				case "btMarlaWibbs":    textoReturn = BtMarlaWibbs;
										break;
				case "btRobertDuncan":  textoReturn = BtRobertDuncan;
										break;
				case "btWarrenBedford": textoReturn = BtWarrenBedford;
										break;
			}
			
			return textoReturn;
		}
	}
}
