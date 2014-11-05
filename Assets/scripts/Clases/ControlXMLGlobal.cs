using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Oscuridad.Clases
{
	public class ControlXMLGlobal
	{
		private string archivoConfiguracion = Path.Combine(Application.persistentDataPath, "Global.xml");
		private string xmlDescripcion = Path.Combine(Application.persistentDataPath,"Descripciones.xml");
		private string xmlHojaPersonaje = Path.Combine(Application.persistentDataPath,"HojaPersonaje.xml");

		public ControlXMLGlobal()
		{
		}

		public ControlXMLGlobal(string nombreEscena)
		{
		}

		public bool Checkear_Estado ()
		{
			if (File.Exists (archivoConfiguracion)) 
			{
				cXML archivoXMLGlobal = new cXML ();
				archivoXMLGlobal.Abrir (archivoConfiguracion);

				bool retorno = bool.Parse (archivoXMLGlobal.DevolverValorElemento (("General/linea[@var=\"EstadoPartida\"]")));
				archivoXMLGlobal.Cerrar ();

				return retorno;
			} 
			else 
			{
				return false;
			}
		}

		public void Cambiar_Estado()
		{
			cXML archivoXMLGlobal = new cXML ();
			archivoXMLGlobal.Abrir (archivoConfiguracion);
			
			XmlNode nodoAux = archivoXMLGlobal.DevolverElementos("General/linea[@var=\"EstadoPartida\"]")[0];
			bool aux = !bool.Parse (nodoAux.InnerText);
			nodoAux.InnerText = aux.ToString ();

			archivoXMLGlobal.Grabar ();
			archivoXMLGlobal.Cerrar ();
		}


		public void Borrar_Configuracion()
		{
			File.Delete(Path.Combine(Application.persistentDataPath, "Global.xml"));
			File.Delete(Path.Combine(Application.persistentDataPath, "Conversaciones.xml"));
			File.Delete(Path.Combine(Application.persistentDataPath, "Descripciones.xml"));
			File.Delete(Path.Combine(Application.persistentDataPath, "HojaPersonaje.xml"));
		}

		public string Devolver_Nombre_Escena(string nombreEscena)
		{
			cXML archivoXMLGlobal = new cXML ();
			archivoXMLGlobal.Abrir (archivoConfiguracion);
			
			XmlNode nodoAux = archivoXMLGlobal.DevolverElementos("Escenas/" + nombreEscena + "/Nombre")[0];

			return nodoAux.InnerText;
		}

		public void Crear_Elemento_Descripciones(string ramaSeleccionada, string nuevaDefinicion)
		{
			//Cargamos el xml de Descripciones
			cXML xmlDescripciones = new cXML ();
			xmlDescripciones.Abrir (xmlDescripcion);

			//Eliminamos la rama de Tiradas, ya que se ha inspeccionado este objeto
			xmlDescripciones.EliminarElementos(ramaSeleccionada + "/TextoTirada");
			
			//Añadimos el nuevo texto a la rama Seleccionada
			int idMayor = Devolver_Id_Mayor(xmlDescripciones.DevolverElementos(ramaSeleccionada)[0]);
			string atributoAux = "id:" + (idMayor++);

			XmlNode nodoAuxiliar = xmlDescripciones.CrearElemento (ramaSeleccionada, "Texto", nuevaDefinicion, new string[]{atributoAux});

			xmlDescripciones.Grabar();

			xmlDescripciones.Cerrar ();
		}

		private int Devolver_Id_Mayor(XmlNode nodoSeleccionado)
		{
			int mayor = 0;
			int actual = 0;
			
			foreach(XmlNode nodoAux in nodoSeleccionado.ChildNodes)
			{
				if(nodoAux.Name == "Texto")
				{
					actual = int.Parse(nodoAux.Attributes["id"].Value.ToString());
					if(actual > mayor)
						mayor = actual;
				}
			}
			
			return mayor;
		}
	}
}
