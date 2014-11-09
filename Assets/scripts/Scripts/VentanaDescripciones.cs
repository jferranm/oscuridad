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
			if(GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() != EstadosJugador.enMenus)
			{
				if (GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() == EstadosJugador.enEspera)
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
		GUILayout.Window(0, new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width), DescripcionesWindow, GameCenter.InstanceRef.controladoraGUI.cabeceraInferior);
	}
	
	void DescripcionesWindow(int windowID) 
	{
		posicionBarraScrollDescripciones = GUILayout.BeginScrollView (posicionBarraScrollDescripciones);
			GUILayout.BeginVertical ();
				foreach (Etiqueta nuevaEtiqueta in GameCenter.InstanceRef.controladoraGUI.listaVentanaInferior) 
				{
					GUIStyle fuente = new GUIStyle (GUI.skin.label);
					fuente.normal.textColor = nuevaEtiqueta.ObtenerColor();
					GUILayout.Label(nuevaEtiqueta.ObtenerTexto(), fuente);
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}
}