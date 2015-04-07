using System;
using System.Collections.Generic;
using System.Collections;
using Oscuridad.Enumeraciones;
using UnityEngine;

namespace Oscuridad.Clases
{
	/// <summary>
	/// Clase base para Configuracion del Juego
	/// </summary>
	[System.Serializable]
	public class Config
	{
		private Idioma idiomaJuego;
		/// <summary>
		/// Idioma del juego
		/// </summary>
		/// <value>
		/// enumeracion con el idioma dle juego
		/// </value>
		public Idioma IdiomaJuego
		{
			get { return idiomaJuego; }
			set { idiomaJuego = value; }
		}

		/// <summary>
		/// Constructor de la clase <see cref="Config"/>.
		/// </summary>
		public Config()
		{
		}
		
		/// <summary>
		/// Constructor de la clase <see cref="Config"/>.
		/// </summary>
		/// <param name="idiomaCapturado">enum con el idioma capturado en la plataforma</param>
		public Config(SystemLanguage idiomaCapturado)
		{
			IdiomaJuego = CapturarIdioma (idiomaCapturado);
		}
		
		/// <summary>
		/// Captura el idioma del dispositivo
		/// </summary>
		/// <param name="idioma">enum con el nuevo idioma</param>
		public Idioma CapturarIdioma(SystemLanguage idioma)
		{
			switch (idioma) 
			{
				case SystemLanguage.English : return Idioma.english;
				case SystemLanguage.French : return Idioma.french;
				case SystemLanguage.Spanish : return Idioma.spanish;
				default: return Idioma.english;
			}
		}
	}
}
