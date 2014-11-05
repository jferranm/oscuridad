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

	private string cabecera = "";
	private string texto = "";

	private List<Etiqueta> listaTiradas = new List<Etiqueta>();

	void OnEnable()
	{
		if(GameCenter.InstanceRef != null)
		{
			if(GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() != EstadosJugador.enMenus)
			{
				//GameCenter.InstanceRef.controladoraTextos.listaTiradas.Clear();

				if(GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() == EstadosJugador.enEspera)
				{
					//GameCenter.InstanceRef.controladoraTextos.Generar_Descripcion(Application.loadedLevelName);
					//listaTiradas = GameCenter.InstanceRef.controladoraTextos.listaTiradas;
					//cabecera = GameCenter.InstanceRef.controladoraTextos.textoCabecera;
				}
				else
				{
					//if(GameCenter.InstanceRef.controladoraTextos.objetoSeleccionado != null)
					//{
					//	GameCenter.InstanceRef.controladoraTextos.Generar_Descripcion(GameCenter.InstanceRef.controladoraTextos.objetoSeleccionado.tag.ToString());
					//	cabecera = "Interaccion con " + GameCenter.InstanceRef.controladoraTextos.textoCabecera;
					//	listaTiradas.Clear();
					//}
				}

				//listaTiradas = GameCenter.InstanceRef.controladoraTextos.listaTiradas;
				//cabecera = GameCenter.InstanceRef.controladoraTextos.textoCabecera;
			}
		}
	}

	void OnGUI() 
	{ 
		GUI.skin = skinVentana;
		GUILayout.Window(0, new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width),DescripcionesWindow,cabecera);
	}
	
	void DescripcionesWindow(int windowID) 
	{
		posicionBarraScrollDescripciones = GUILayout.BeginScrollView (posicionBarraScrollDescripciones);
			GUILayout.BeginVertical ();
				foreach (Etiqueta nuevaEtiqueta in listaTiradas) 
				{
					GUIStyle fuente = new GUIStyle (GUI.skin.label);
					fuente.normal.textColor = nuevaEtiqueta.ObtenerColor();
					GUILayout.Label(nuevaEtiqueta.ObtenerTexto(), fuente);
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}
}