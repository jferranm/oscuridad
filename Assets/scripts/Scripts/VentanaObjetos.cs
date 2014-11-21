using UnityEngine;
using System.Collections.Generic;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

[ExecuteInEditMode]
public class VentanaObjetos: MonoBehaviour 
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

	private Vector2 posicionBarraScrollObjeto;
	
	void OnEnable()
	{
		if(GameCenter.InstanceRef != null)
		{
			if(GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() != EstadosJugador.enMenus)
			{
				if(GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() == EstadosJugador.enZoomEspera)
				{
					if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado != null)
					{
						GameCenter.InstanceRef.controladoraGUI.listaVentanaLateral.Add(new Etiqueta(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.MostrarDescripcionBasica(), Color.white));
						GameCenter.InstanceRef.controladoraGUI.cabeceraInferior = "Interaccion con " + GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre;
						posicionBarraScrollObjeto.y = Mathf.Infinity;
					}
					else
					{
						GameCenter.InstanceRef.controladoraGUI.cabeceraInferior = "Interaccion con " + GameCenter.InstanceRef.controladoraJuego.personajePulsado.DescripcionNombre;
					}
				}
			}
		}
	}

	void OnGUI() 
	{ 
		GUI.skin = skinVentana;
		try
		{
			GUILayout.Window(2, new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width), Objetos, GameCenter.InstanceRef.controladoraGUI.cabeceraLateral);
		}
		catch {	}
	}
	
	void Objetos(int windowID) 
	{
		posicionBarraScrollObjeto = GUILayout.BeginScrollView (posicionBarraScrollObjeto, GUIStyle.none);
			GUILayout.BeginVertical ();
				foreach (Etiqueta nuevaEtiqueta in GameCenter.InstanceRef.controladoraGUI.listaVentanaLateral) 
				{
					GUIStyle estilo = new GUIStyle (GUI.skin.label);
					estilo.richText = true;
					
					GUILayout.Label(nuevaEtiqueta.ObtenerTexto(), estilo);
				}
			GUILayout.EndVertical ();
		GUILayout.EndScrollView ();
	}
}