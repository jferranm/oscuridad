using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Personajes
{
	public enum DatosPersonales : int {
		Nombre = 1,
		Profesion = 2, 
		Edad = 3,
		Foto = 4
	}

	public enum Caracteristicas: int {
		Fuerza = 1,
		Constitucion = 2, 
		Tamanyo = 3,
		Destreza = 4, 
		Aparicencia = 5, 
		Cordura = 6,
		Inteligencia = 7,
		Poder = 8,
		Educacion = 9, 
		Idea = 10, 
		Suerte = 11,
		Conocimiento = 12,
		Vida = 13,
		BonificacionDanyo = 14
	}

	public enum Armas : int {
		ArmaCorta = 1,
		Fusil = 2,
		Escopeta = 3
	}

	public class Utils {

		private string UserPath = Application.persistentDataPath;

		private static string sDatos = "DatosPersonales";
		private static string sCaracteristicas = "Caracteristicas";
		private static string sHabilidades = "Habilidades";
		private static string sArmas = "Armas";

		public void Inicializar_XML()
		{
			string destino = null;
			TextAsset origen = null;

			//Creacion del xml de las descripciones
			origen = (TextAsset)Resources.Load("xml/Base/Descripciones", typeof(TextAsset));
			destino = Path.Combine(UserPath, "Descripciones.xml");
			if (!File.Exists (destino))
				Crear_Fichero (origen, destino);

			//Creacion del xml de las Conversaciones
			origen = (TextAsset)Resources.Load("xml/Base/Conversaciones", typeof(TextAsset));
			destino = Path.Combine(UserPath, "Conversaciones.xml");
			if (!File.Exists (destino)) 
				Crear_Fichero (origen, destino);

			//Creacion del xml Global
			origen = (TextAsset)Resources.Load("xml/Base/Global", typeof(TextAsset));
			destino = Path.Combine(UserPath, "Global.xml");
			if (!File.Exists (destino)) 
			{
				Crear_Fichero (origen, destino);
				ControlXMLGlobal nuevoControl = new ControlXMLGlobal();
				nuevoControl.Cambiar_Estado();
			}
		}

		public void Crear_Fichero(TextAsset nuevoOrigen, string nuevoDestino)
		{
			try {
				StreamWriter sw = new StreamWriter(nuevoDestino, false);
				sw.Write(nuevoOrigen.text);
				sw.Close();
			} catch (IOException ex) {
				Console.WriteLine(ex.Message);
			}
		}
		
		public bool crearHojaPersonaje(Personaje personaje) {
			//copiar la hoja de personaje vacia a la ruta del usuario
			string destino = Path.Combine(UserPath, "HojaPersonaje.xml");
			TextAsset origen = null;
			try {
				origen = (TextAsset)Resources.Load("xml/HojaPersonaje", typeof(TextAsset));
			} catch (Exception ex) {
				StreamWriter sw = new StreamWriter(destino, false);
				sw.Write(ex.Message);
				sw.Close();
			}

			try {
				StreamWriter sw = new StreamWriter(destino, false);
				sw.Write(origen.text);
				sw.Close();
			} catch (IOException ex) {
				Console.WriteLine(ex.Message);
				return false;
			}

			rellenarHojaPersonaje(personaje, destino);
			//TODO: Insertar los datos del personaje en el XML global

			return true;
		}

		private void rellenarHojaPersonaje(Personaje personaje, string hoja) {
			cXML xml = new cXML();

			xml.Abrir(hoja);

			switch (personaje) {
			case Personaje.RobertDuncan:
				xml.SetValor(sDatos, "Dato[@id='" + (int)DatosPersonales.Nombre + "']", "Robert Duncan");
				xml.SetValor(sDatos, "Dato[@id='" + (int)DatosPersonales.Profesion + "']", "Detective");
				xml.SetValor(sDatos, "Dato[@id='" + (int)DatosPersonales.Edad + "']", "36");
				xml.SetValor(sDatos, "Dato[@id='" + (int)DatosPersonales.Foto + "']", "");

				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Fuerza + "']", "15");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Constitucion + "']", "16");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Tamanyo + "']", "12");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Inteligencia + "']", "11");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Poder + "']", "12");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Destreza + "']", "14");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Aparicencia + "']", "12");				
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Cordura + "']", "60");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Educacion + "']", "14");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Idea + "']", "55%");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Suerte + "']", "60%");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Conocimiento + "']", "70%");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.Vida + "']", "14");
				xml.SetValor(sCaracteristicas, "Caracteristica[@id='" + (int)Caracteristicas.BonificacionDanyo + "']", "1D4");

				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.BuscarLibros + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Cerrajeria + "']", "40");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Conducir + "']", "40");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Charlataneria + "']", "55");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Credito + "']", "15");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Derecho + "']", "55");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Descubrir + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Discrecion + "']", "40");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Disfrazarse + "']", "20");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Escuchar + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Esquivar + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Fotografia + "']", "45");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Historia + "']", "20");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.LenguaPropia + "']", "55");
				xml.SetAtributo(sHabilidades, "Habilidad[@id='" + (int)Habilidades.LenguaPropia + "']", "Idioma", "Ingles");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Mecanica + "']", "20");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Nadar + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Ocultar + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Ocultarse + "']", "40");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Persuasion + "']", "35");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.PrimerosAuxilios + "']", "30");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Psicologia + "']", "45");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Regatear + "']", "75");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Saltar + "']", "25");
				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.ArmaCorta + "']", "65");				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.Escopeta + "']", "30");				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.Fusil + "']", "25");

				break;

			case Personaje.MarlaGibbs:
				xml.SetValor(sDatos,"Dato[@id='" + (int)DatosPersonales.Nombre + "']", "Marla Gibbs");
				xml.SetValor(sDatos,"Dato[@id='" + (int)DatosPersonales.Profesion + "']", "Periodista");
				xml.SetValor(sDatos,"Dato[@id='" + (int)DatosPersonales.Edad + "']", "32");
				xml.SetValor(sDatos,"Dato[@id='" + (int)DatosPersonales.Foto + "']", "");
				
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Fuerza + "']", "8");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Constitucion + "']", "11");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Tamanyo + "']", "9");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Inteligencia + "']", "16");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Poder + "']", "13");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Destreza + "']", "12");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Aparicencia + "']", "14");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Cordura + "']", "65");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Educacion + "']", "17");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Idea + "']", "85%");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Suerte + "']", "65%");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Conocimiento + "']", "80%");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Vida + "']", "10");
				
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Antropologia + "']", "40");				
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.BuscarLibros + "']", "75");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.CienciasOcultas + "']", "40");;
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Conducir + "']", "35");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Charlataneria + "']", "45");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Credito + "']", "40");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Derecho + "']", "45");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Descubrir + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Discrecion + "']", "35");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Escuchar + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Esquivar + "']", "24");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Historia + "']", "65");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.LenguaPropia + "']", "80");
				xml.SetAtributo(sHabilidades, "Habilidad[@id='" + (int)Habilidades.LenguaPropia + "']", "Idioma", "Ingles");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Mecanica + "']", "20");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Nadar + "']", "40");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Ocultar + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Ocultarse + "']", "35");
				xml.CrearElemento(sHabilidades + "//" + ("Dato[@id='" + (int)Habilidades.OtraLengua).ToString() + "']", "Lengua", "55", new string[] {"Idioma:Frances"}); 
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Persuasion + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.PrimerosAuxilios + "']", "30");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Psicologia + "']", "45");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Saltar + "']", "25");
				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.ArmaCorta + "']", "40");				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.Escopeta + "']", "30");				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.Fusil + "']", "25");

				break;

			case Personaje.WarrenBedford:
				xml.SetValor(sDatos,"Dato[@id='" + (int)DatosPersonales.Nombre + "']", "Warren Bedford");
				xml.SetValor(sDatos,"Dato[@id='" + (int)DatosPersonales.Profesion + "']", "Profesor de Historia");
				xml.SetValor(sDatos,"Dato[@id='" + (int)DatosPersonales.Edad + "']", "56");
				xml.SetValor(sDatos,"Dato[@id='" + (int)DatosPersonales.Foto + "']", "");
				
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Fuerza + "']", "10");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Constitucion + "']", "9");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Tamanyo + "']", "10");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Inteligencia + "']", "17");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Poder + "']", "16");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Destreza + "']", "7");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Aparicencia + "']", "9");				
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Cordura + "']", "80");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Educacion + "']", "23");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Idea + "']", "85%");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Suerte + "']", "80%");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Conocimiento + "']", "99%");
				xml.SetValor(sCaracteristicas,"Dato[@id='" + (int)Caracteristicas.Vida + "']", "10");

				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Antropologia + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Arqueologia + "']", "50");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Astronomia + "']", "20");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.BuscarLibros + "']", "75");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.CienciasOcultas + "']", "55");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Conducir + "']", "50");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Credito + "']", "75");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Derecho + "']", "30");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Descubrir + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Equitacion + "']", "30");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Discrecion + "']", "40");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Disfrazarse + "']", "20");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Escuchar + "']", "25");
				xml.CrearElemento(sHabilidades + "//" + ("Dato[@id='" + (int)Habilidades.HabilidadArtistica).ToString() + "']", "Habilidad", "55", new string[] {"Nombre:Pintar"});
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Historia + "']", "85");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.HistoriaNatural + "']", "35");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.LenguaPropia + "']", "85");
				xml.SetAtributo(sHabilidades, "Habilidad[@id='" + (int)Habilidades.LenguaPropia + "']", "Idioma" + "']", "Ingles");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Nadar + "']", "25");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Ocultar + "']", "25");
				xml.CrearElemento(sHabilidades + "//" + ("Dato[@id='" + (int)Habilidades.OtraLengua).ToString() + "']", "Lengua", "45", new string[] {"Idioma:Frances"}); 
				xml.CrearElemento(sHabilidades + "//" + ("Dato[@id='" + (int)Habilidades.OtraLengua).ToString() + "']", "Lengua", "30", new string[] {"Idioma:Aleman"}); 
				xml.CrearElemento(sHabilidades + "//" + ("Dato[@id='" + (int)Habilidades.OtraLengua).ToString() + "']", "Lengua", "25", new string[] {"Idioma:Italiano"}); 
				xml.CrearElemento(sHabilidades + "//" + ("Dato[@id='" + (int)Habilidades.OtraLengua).ToString() + "']", "Lengua", "55", new string[] {"Idioma:Latin"}); 
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Persuasion + "']", "15");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.PrimerosAuxilios + "']", "30");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Psicologia + "']", "55");
				xml.SetValor(sHabilidades, "Habilidad[@id='" + (int)Habilidades.Saltar + "']", "25");
				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.ArmaCorta + "']", "30");				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.Escopeta + "']", "30");				
				xml.SetValor(sArmas,"Arma[@id='" + (int)Armas.Fusil + "']", "40");

				break;
			}

			xml.Grabar();
		}
	}
}