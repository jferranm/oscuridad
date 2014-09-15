using UnityEngine;
using System.Collections;

namespace Oscuridad.Clases
{
	public class Etiqueta
	{
		private string texto;
		private Color color;
		
		public Etiqueta(string texto, Color color)
		{
			this.texto = texto;
			this.color = color;
		}

		public string ObtenerTexto()
		{
			return this.texto;
		}

		public Color ObtenerColor()
		{
			return this.color;
		}
	}
}
