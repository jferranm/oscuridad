using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System;

namespace Oscuridad.Clases
{
	public class Descripciones
	{
		private GameObject objetoActivo;
		private string xmlDescripcion = Path.Combine(Application.persistentDataPath,"Descripciones.xml");
		private string xmlHojaPersonaje = Path.Combine(Application.persistentDataPath,"HojaPersonaje.xml");
		private string nombreBuscar;
		private cXML nuevoXML;

		public Descripciones(GameObject objetoBuscar)
		{
			objetoActivo = objetoBuscar;
			nombreBuscar = objetoActivo.tag.ToString();
			nuevoXML = new cXML();
			nuevoXML.Abrir(xmlDescripcion);
		}

		public Descripciones(string nombreEscena)
		{
			nombreBuscar = nombreEscena;
			nuevoXML = new cXML();
			nuevoXML.Abrir(xmlDescripcion);
		}

		public string Devolver_Descripcion()
		{
			//Devolvemos la descripcion basica del objeto activo
			XmlNode nodoAuxiliar = nuevoXML.DevolverElementos(nombreBuscar+"/Texto")[0];

			return nodoAuxiliar.InnerText.ToString();
		}

		public string Devolver_Cabecera()
		{
			//Devolvemos el nombre de la escena u objeto
			XmlNode nodoAuxiliar = nuevoXML.DevolverElementos(nombreBuscar+"/Nombre")[0];
			
			return nodoAuxiliar.InnerText.ToString();
		}

		public List<string> Devolver_Descripcion_Tiradas()
		{
			//Devolvemos la descripcion tras las tiradas
			List<string> listaTexto = new List<string> ();
			string nombreObjeto = objetoActivo.tag.ToString();
			XmlNode nodoAuxiliar = nuevoXML.DevolverElementos(nombreObjeto+"/TextoTirada")[0];

			foreach(XmlNode nodoSeleccionado in nodoAuxiliar.ChildNodes)
			{
				listaTexto.Add (BuscarTirada(nodoSeleccionado));
				if (BuscarTirada(nodoSeleccionado).Contains("TIRADA EXITOSA")) break;
			}

			return listaTexto;
		}

		public string BuscarTirada(XmlNode nodoAuxiliar)
		{
			string descripcionADevolver;

			try
			{
				//Si todo funciona bien es una descripcion con tirada
				//Cargamos todos los datos de control de los atributos a las variables
				string tirada = nodoAuxiliar.Attributes["tirada"].Value.ToString();
				string habilidad = nodoAuxiliar.Attributes["habilidad"].Value.ToString();

				//Generamos un nuevo lanzamientro segun el dado
				int valorLanzamiento = HacerTirada(tirada);

				//Cargamos el valor de la habilidad
				int valorHabilidad = CalcularHabilidad(habilidad);

				//Elegimos segun tirada
				if (valorLanzamiento < valorHabilidad) //tirada pasada
				{
					descripcionADevolver = "TIRADA EXITOSA: " + NombreHabilidad(habilidad);
					descripcionADevolver = descripcionADevolver + "\n" + nodoAuxiliar.InnerText.ToString();

				}
				else //tirada no pasada
				{
					descripcionADevolver = "TIRADA FALLIDA: " + NombreHabilidad(habilidad);
				}
			}
			catch
			{
				//Si rompe es que la descripcion tras fallar las tiradas
				descripcionADevolver = nodoAuxiliar.InnerText.ToString();
			}

			return descripcionADevolver;
		}

		public int CalcularHabilidad(string numeroHabilidad)
		{
			//Abrimos el XML con la Hoja de Personaje
			cXML nuevoXMLPersonaje = new cXML();
			nuevoXMLPersonaje.Abrir(xmlHojaPersonaje);
			
			//Devolvemos el valor de habilidad para el calculo de la tirada
			XmlNode nodoPrincipal = nuevoXMLPersonaje.DevolverElementos("HojaPersonaje/Habilidades/Habilidad[@id=\""+ numeroHabilidad +"\"]")[0];
			string valor = nodoPrincipal.InnerText.ToString();
			
			//Cerramos el XML de la hoja de personaje
			nuevoXMLPersonaje.Cerrar();
			
			return int.Parse(valor);
		}

		public string NombreHabilidad(string numeroHabilidad)
		{
			//Abrimos el XML con la Hoja de Personaje
			cXML nuevoXMLPersonaje = new cXML();
			nuevoXMLPersonaje.Abrir(xmlHojaPersonaje);
			
			//Devolvemos el valor de habilidad para el calculo de la tirada
			XmlNode nodoPrincipal = nuevoXMLPersonaje.DevolverElementos("HojaPersonaje/Habilidades/Habilidad[@id=\""+ numeroHabilidad +"\"]")[0];
			string valor = nodoPrincipal.Attributes["texto"].Value.ToString();
			
			//Cerramos el XML de la hoja de personaje
			nuevoXMLPersonaje.Cerrar();
			
			return valor;
		}

		public int HacerTirada(string valorTirada)
		{
			LanzamientoDados nuevoLanzamiento = new LanzamientoDados();
			//nuevoLanzamiento.Add(new tirada(valorTirada));
			
			return nuevoLanzamiento.Lanzar(valorTirada);
		}

		public void Cerrar()
		{
			nuevoXML.Cerrar();
		}
	}
}
