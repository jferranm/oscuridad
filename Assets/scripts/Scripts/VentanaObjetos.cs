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
	
	private string cabecera = "";

	private List<Etiqueta> listaObjetos = new List<Etiqueta>();

	void OnEnable()
	{
		if(GameCenter.InstanceRef != null)
		{
			if(GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() != EstadosJugador.enMenus)
			{
				//GameCenter.InstanceRef.controladoraTextos.listaObjetos.Clear();
				//cabecera = "Interaccion con " + GameCenter.InstanceRef.controladoraTextos.textoCabecera;
				//cabecera = GameCenter.InstanceRef.controladoraTextos.textoCabecera;
				//GameCenter.InstanceRef.controladoraTextos.Generar_Descripcion_Objeto(GameCenter.InstanceRef.controladoraTextos.objetoSeleccionado.tag.ToString());                             
				//listaObjetos = GameCenter.InstanceRef.controladoraTextos.listaObjetos;
			}
		}
	}

	void OnGUI() 
	{ 
		GUI.skin = skinVentana;
		GUILayout.Window(2, new Rect(posicion_x * Screen.width, posicion_y * Screen.height, escala_x * Screen.width, escala_y * Screen.width),Objetos,cabecera);
	}
	
	void Objetos(int windowID) 
	{
		posicionBarraScrollObjeto = GUILayout.BeginScrollView (posicionBarraScrollObjeto, GUIStyle.none);
			GUILayout.BeginVertical ();
				foreach (Etiqueta nuevaEtiqueta in listaObjetos) 
				{
					GUIStyle fuente = new GUIStyle (GUI.skin.label);
					fuente.normal.textColor = nuevaEtiqueta.ObtenerColor();
					GUILayout.Label(nuevaEtiqueta.ObtenerTexto(), fuente);
				}
			GUILayout.EndVertical ();
		GUILayout.EndScrollView ();
	}
}