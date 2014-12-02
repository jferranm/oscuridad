using UnityEngine;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

[ExecuteInEditMode]
public class VentanaDescripciones: MonoBehaviour 
{
	[Range(0, 1)]
	public float posicion_x;
	[Range(0, 1)]
	public float posicion_y;
	[Range(0, 1)]
	public float escala_x;
	[Range(0, 1)]
	public float escala_y;
	[Range(0,1)]
	public float posicion_z;

	public GUISkin skinVentana;

	private Vector2 posicionBarraScrollDescripciones;

	void OnEnable()
	{
		if(GameCenter.InstanceRef != null)
		{
			if(GameCenter.InstanceRef.controladoraJugador.EstadoJugador != EstadosJugador.enMenus)
			{
				if (GameCenter.InstanceRef.controladoraJugador.EstadoJugador == EstadosJugador.enEspera)
				{
					GameCenter.InstanceRef.controladoraGUI.listaVentanaInferior.Add(new Etiqueta(GameCenter.InstanceRef.controladoraJuego.escenaActual.Descripcion, Color.white));
					GameCenter.InstanceRef.controladoraGUI.cabeceraInferior = GameCenter.InstanceRef.controladoraJuego.escenaActual.NombreEscena;
					posicionBarraScrollDescripciones.y = Mathf.Infinity;
				}
			}
		}
	}

	void OnGUI() 
	{ 
		GUI.skin = skinVentana;
		try
		{
			GUILayout.Window(0, new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width), DescripcionesWindow, GameCenter.InstanceRef.controladoraGUI.cabeceraInferior);
		}
		catch {	}
	}
	
	void DescripcionesWindow(int windowID) 
	{
		posicionBarraScrollDescripciones = GUILayout.BeginScrollView (posicionBarraScrollDescripciones);
			GUILayout.BeginVertical ();
				if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado != null)
						Mostrar_Descripcion ();
				else
						Mostrar_Conversacion ();
				
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	public void Mostrar_Descripcion()
	{
		foreach (Etiqueta nuevaEtiqueta in GameCenter.InstanceRef.controladoraGUI.listaVentanaInferior) 
		{
			GUIStyle estilo = new GUIStyle (GUI.skin.label);
			estilo.richText = true;
			
			GUILayout.Label(nuevaEtiqueta.ObtenerTexto(), estilo);
		}
	}

	public void Mostrar_Conversacion()
	{
		foreach (PreguntaBase pregunta in GameCenter.InstanceRef.controladoraGUI.nuevaRespuesta.MostrarPreguntas()) 
		{
			if(GameCenter.InstanceRef.controladoraGUI.Comprobar_Pregunta(pregunta))
			{
				if (GUILayout.Button (pregunta.TextoPregunta)) 
					GameCenter.InstanceRef.controladoraGUI.Boton_Pulsado (pregunta.TextoPregunta, pregunta.IdRespuesta);
			}
		}
	}
}