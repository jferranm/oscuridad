using UnityEngine;
using System.Collections;

namespace Oscuridad.Clases
{
	public class Etiqueta
	{
		private string texto;
		private Color color;
		private bool tirada;
		private string textoTirada;
		private Color colorTirada;
		
		public Etiqueta(string texto, Color color, bool tirada)
		{
			this.texto = texto;
			this.color = color;
			this.tirada = tirada;
		}

		public Etiqueta(string texto, Color color, bool tirada, string textoTirada, Color colorTirada)
		{
			this.texto = texto;
			this.color = color;
			this.tirada = tirada;
			this.textoTirada = textoTirada;
			this.colorTirada = colorTirada;
		}

		public string ObtenerTexto()
		{
			return this.texto;
		}

		public string ObtenerColor()
		{
			return ObtenerColor(this.color);
		}

		public Color ObtenerColor(bool seleccion)
		{
			return this.color;
		}

		public bool ObtenerTirada()
		{
			return this.tirada;
		}

		public string ObtenerTextoTirada()
		{
			return this.textoTirada;
		}

		public string ObtenerColorTirada()
		{
			return ObtenerColor(this.colorTirada);
		}

		public Color ObtenerColorTirada(bool seleccion)
		{
			return this.colorTirada;
		}

		private string ObtenerColor(Color color)
		{
			if (color.Equals(Color.red))
				return "red";
			if (color == Color.green)
				return "green";	
			if (color == Color.white)
				return "white";

			return null;
		}
	}
}
