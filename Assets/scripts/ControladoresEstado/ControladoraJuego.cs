using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

[System.Serializable]
public class ControladoraJuego
{
	public JugadorBase jugadorActual;
	public EscenaBase escenaActual;

	public ObjetoBase objetoPulsado;
	public PersonajeBase personajePulsado;

	private static ControladoraJuego instanceRef;
	public static ControladoraJuego InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraJuego();
			instanceRef.Inicializar();
		}
		
		return instanceRef;
	}

	public void Inicializar()
	{
		jugadorActual = new JugadorBase ();
		jugadorActual.EstadoJugador = estadosJugador.enMenus;

		escenaActual = new EscenaBase ();
	}

	public void InicializarEscena()
	{
		escenaActual = new EscenaBase ();
	}

	public void CopiarXML()
	{
		string destino = null;
		TextAsset origen = null;

		//Copiamos escenarios
		foreach (string escenario in Enum.GetNames(typeof(Escenas)))
		{
			if(escenario != "ninguna" && escenario != "MenuPrincipal" && escenario != "Escena1" && escenario != "Escena2" && escenario != "Escena3")
			{
				origen = (TextAsset)Resources.Load("xml/Escenarios/"+escenario, typeof(TextAsset));
				destino = Path.Combine(GameCenter.InstanceRef.USERPATH, escenario+".xml");
				if (!File.Exists (destino))
					Crear_Fichero (origen, destino);
			}
		}
	}

	public void Crear_Fichero(TextAsset nuevoOrigen, string nuevoDestino)
	{
		try 
		{
			StreamWriter sw = new StreamWriter(nuevoDestino, false);
			sw.Write(nuevoOrigen.text);
			sw.Close();
		}
		catch (IOException ex) 
		{ 
			//No se a podido crear el archivo	
		}
	}

	public void CargarJugador()
	{
		cXML nuevoXML = new cXML ();
		string pathJugador = Path.Combine(GameCenter.InstanceRef.USERPATH, "Jugador.xml");
		jugadorActual = nuevoXML.Cargar_Clase_Serializable<JugadorBase> (pathJugador, jugadorActual);
	}

	public void GrabarJugador()
	{
		cXML nuevoXML = new cXML ();
		string pathJugador = Path.Combine(GameCenter.InstanceRef.USERPATH, "Jugador.xml");
		nuevoXML.Guardar_Clase_Serializable<JugadorBase> (pathJugador, jugadorActual);
	}

	public EscenaBase Cargar_Escena(Escenas escena)
	{
		cXML nuevoxml = new cXML ();
		return nuevoxml.Cargar_Clase_Serializable<EscenaBase> (Path.Combine (Application.persistentDataPath, escena.ToString()+".xml"), GameCenter.InstanceRef.controladoraJuego.escenaActual);
	}

	public void Guardar_Escena(Escenas escena)
	{
		cXML nuevoxml = new cXML ();
		nuevoxml.Guardar_Clase_Serializable<EscenaBase> (Path.Combine (Application.persistentDataPath, escena.ToString () + ".xml"), GameCenter.InstanceRef.controladoraJuego.escenaActual);

	}

	public void Inicializar_Objetos()
	{
		foreach (ObjetoBase objeto in GameCenter.InstanceRef.controladoraJuego.escenaActual.MostrarObjeto()) 
		{
			GameObject.FindGameObjectWithTag(objeto.Nombre).SetActive(objeto.ObjetoActivo);
		}
	}
}
