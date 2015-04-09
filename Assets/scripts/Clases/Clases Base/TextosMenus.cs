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
		public string btComenzar;
		public string btContinuar;
		public string btOpciones;
		public string btSalir;
		public string btPartidaNueva;
		public string toggleSonido;
		public string toggleMusica;
		public string btIdioma;
		
		/// <summary>
		/// Constructor de la clase <see cref="TextosMenus"/>.
		/// </summary>
		public TextosMenus()
		{
		}
	}
}
