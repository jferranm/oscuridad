using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

namespace Oscuridad.Clases
{
	public class Etiqueta
	{
		private string texto;

		public Etiqueta(string textoMostrar, Color colorMostrar)
		{
			texto = ObtenerColor (colorMostrar) + textoMostrar + FinDeLineaColor ();
		}

		public Etiqueta(string textoMostrar, Color colorMostrar, string opcion)
		{
			switch (opcion) 
			{
				case "Examinar":
					texto = ObtenerColor(colorMostrar) + "Examinar " + Comillas () + textoMostrar + Comillas() + FinDeLineaColor();
					break;
			}
		}

		public Etiqueta(bool tirada, Habilidades habilidad, int resultado)
		{
			string aux = "";
			Color colorTirada;
			
			if (tirada) 
			{
				aux = "Exito";
				colorTirada = Color.green;
			} 
			else 
			{
				aux = "Fracaso";
				colorTirada = Color.red;
			}

				texto = ObtenerColor(Color.white) + "- Tirada " + GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Descripcion_Segun_Enum(habilidad) + "(" + GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Valor_Segun_Enum(habilidad) + "%): " + resultado.ToString () + "." + FinDeLineaColor() + ObtenerColor(colorTirada) + aux + FinDeLineaColor();
		}

		public Etiqueta(Objetos objetoSeleccionado, Color color)
		{
			texto = ObtenerColor(color) + Comillas() + GameCenter.InstanceRef.controladoraJuego.Devolver_Descripcion_Objeto_Segun_Enum(objetoSeleccionado) + Comillas() + " ahora esta en el Inventario" + FinDeLineaColor ();
		}

		public Etiqueta(Localizaciones localizacion, Color color)
		{
			texto = ObtenerColor(color) + "Nueva localizacion descubierta: " + Comillas() + GameCenter.InstanceRef.controladoraJuego.Devolver_Descripcion_Localizacion_Segun_Enum(localizacion) + Comillas() + FinDeLineaColor ();
		}

		public string ObtenerTexto()
		{
			return this.texto;
		}

		private string ObtenerColor(Color color)
		{
			if (color.Equals(Color.red))
				return "<color=red>";
			if (color.Equals(Color.green))
				return "<color=green>";	
			if (color.Equals(Color.white))
				return "<color=white>";

			return null;
		}

		private string FinDeLineaColor()
		{
			return "</color>";
		}

		private string Comillas()
		{
			return "\"";
		}
	}
}
