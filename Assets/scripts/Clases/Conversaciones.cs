using System;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Oscuridad.Clases
{
	public class Conversaciones
	{
		cXML nuevoXML = new cXML();
		public Pregunta nuevaPregunta;
		public string xmlConversaciones = Path.Combine(Application.persistentDataPath, "Conversaciones.xml");
		public string xmlHojaPersonaje = Path.Combine(Application.persistentDataPath, "HojaPersonaje.xml");
		public Stream streamHojaPersonaje;
		public string idPregunta;
		public string tagPersonaje;
		
		public Conversaciones(string idConversacion, string tagPersonajeSeleccionado)
		{
			//Cargamos los valores en las variables globales
			idPregunta = idConversacion;
			tagPersonaje = tagPersonajeSeleccionado;
			
			//Abrimos el xml de las descripciones
			nuevoXML.Abrir(xmlConversaciones);
		}
		
		public Pregunta IniciarConversacion()
		{
			Pregunta datosDevolver = null;

			//Rellenamos los Objetos con los datos
			Crear_Objetos_Conversacion();

			//Si el numero de pregunta es = 0 las preguntas han acabado
			if(nuevaPregunta.numeroPregunta == 0)
			{
				datosDevolver = null;
			}
			else //Si no es = 0 es que el codigo es de una pregunta nueva
			{
				//Si el texto es = * significa que tiene una tirada
				if(nuevaPregunta.textoPregunta == "*")
				{
					idPregunta = CalcularTiradas().ToString();
					datosDevolver = IniciarConversacion();
				}
				else //Si no es asi se envian los valores
				{
					datosDevolver = nuevaPregunta;
				}
			}
			
			//Cerramos el Xml
			nuevoXML.Cerrar();

			return datosDevolver;
		}

		public void Crear_Objetos_Conversacion()
		{
			//Inicializamos los objeto
			XmlNode nodoAuxiliar = nuevoXML.DevolverElementos("Conversaciones/"+ tagPersonaje +"/Pregunta[@id=\""+ idPregunta +"\"]")[0];
			
			nuevaPregunta = new Pregunta(nodoAuxiliar);
		}
		
		public int CalcularTiradas()
		{
			int nuevoId = 0;

			//Hacemos la tirada segun los valores rescatados de xml
			int valorLanzamiento = HacerTirada();
			
			//Rescatamos el valor de la Hablidad del Personaje
			int valorHabilidad = CalcularHabilidad(nuevaPregunta.habilidad);
			
			//Calculamos si ha pasado la tirada
			if(valorLanzamiento < valorHabilidad)
			{
				foreach(RespuestaConTirada nuevaRespuesta in nuevaPregunta.listaRespuestas.Values)
				{
					if (nuevaRespuesta.tiradaPasada != 0)
					{
						nuevoId = nuevaRespuesta.tiradaPasada;
					}
				}
			}
			else
			{
				foreach(RespuestaConTirada nuevaRespuesta in nuevaPregunta.listaRespuestas.Values)
				{
					if (nuevaRespuesta.tiradaNoPasada != 0)
					{
						nuevoId = nuevaRespuesta.tiradaNoPasada;
					}
				}
			}

			Grabar_Datos_XML(nuevoId);
			return nuevoId;
		}
		
		public int HacerTirada()
		{
			string valorTirada = "1D100";
			LanzamientoDados nuevoLanzamiento = new LanzamientoDados();
			nuevoLanzamiento.Add(new tirada(valorTirada));
					
			return nuevoLanzamiento.Lanzar(valorTirada);
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
			
			return Convert.ToInt16(valor);
		}

		public void Grabar_Datos_XML(int idACambiar)
		{
			//Capturamos la referencia del xml que manda la pregunta antes de la tirada.
			XmlNode nodoAuxiliar = nuevoXML.DevolverElementos("Conversaciones/"+ tagPersonaje +"/Pregunta[@id=\""+ idPregunta +"\"]")[0];
			string referencia = nodoAuxiliar.Attributes ["referencia"].Value.ToString();

			//Capturamos la el nodo con el valor de la referencia y la respuesta con el idPregunta anterior
			nodoAuxiliar = nuevoXML.DevolverElementos("Conversaciones/"+ tagPersonaje + "/Pregunta[@id=\""+ referencia + "\"]/Respuesta[@num=\""+ idPregunta +"\"]")[0];
			XmlAttribute atributoSeleccionado = nodoAuxiliar.Attributes["num"];
			//Modificamos el valor del atributo hacia su nueva referenia
			atributoSeleccionado.Value = idACambiar.ToString(); 

			//Limpiamos la habilidad entre parentesis de la pregunta
			nodoAuxiliar.InnerText = nodoAuxiliar.InnerText.Remove(0, nodoAuxiliar.InnerText.IndexOf(")") + 2);

			//Grabamos las modificaciones
			nuevoXML.Grabar();
		}
	}

	public class Pregunta
	{
		public int numeroPregunta;
		public string textoPregunta;
		public int referenciaPregunta;
		public string dado;
		public string habilidad;
		public Hashtable listaRespuestas;

		public Pregunta()
		{
		}

		public Pregunta(XmlNode nodoSeleccionado)
		{
			//Inicializamos Valores
			listaRespuestas = new Hashtable();
			numeroPregunta = int.Parse(nodoSeleccionado.Attributes["id"].Value.ToString());
			textoPregunta = nodoSeleccionado.Attributes["texto"].Value.ToString();
			if(textoPregunta == "*")
			{
				dado = nodoSeleccionado.Attributes["tirada"].Value.ToString();
				habilidad = nodoSeleccionado.Attributes["habilidad"].Value.ToString();
				referenciaPregunta = int.Parse(nodoSeleccionado.Attributes["referencia"].Value.ToString());

				byte cont = 0;
				foreach(XmlNode nuevoNodo in nodoSeleccionado.ChildNodes)
				{
					RespuestaConTirada nuevaRespuesta = new RespuestaConTirada(nuevoNodo);
					listaRespuestas.Add(cont, nuevaRespuesta);
					cont++;
				}
			}
			else
			{
				foreach(XmlNode nuevoNodo in nodoSeleccionado.ChildNodes)
				{
					RespuestaSinTirada nuevaRespuesta = new RespuestaSinTirada(nuevoNodo);
					listaRespuestas.Add(nuevaRespuesta.idSiguientePregunta, nuevaRespuesta);
				}
			}
		}
	}

	public class RespuestaSinTirada
	{
		public string textoRespuesta;
		public int idSiguientePregunta;

		public RespuestaSinTirada()
		{
		}

		public RespuestaSinTirada(XmlNode nodoRespuesta)
		{
			idSiguientePregunta = int.Parse(nodoRespuesta.Attributes["num"].Value.ToString());
			textoRespuesta = nodoRespuesta.InnerText.ToString();
		}
	}

	public class RespuestaConTirada
	{
		public int tiradaPasada;
		public int tiradaNoPasada;

		public RespuestaConTirada()
		{
		}
		
		public RespuestaConTirada(XmlNode nodoRespuesta)
		{
			string tirada = nodoRespuesta.Attributes["resultado"].Value.ToString();
			if(tirada == "True")
			{
				tiradaPasada = int.Parse(nodoRespuesta.InnerText.ToString());
			}
			else
			{
				tiradaNoPasada = int.Parse(nodoRespuesta.InnerText.ToString());
			}
		}
	}
}