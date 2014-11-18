using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

public class PanelVentana : MonoBehaviour 
{
	public GameObject ventanaConversacionesRef;
	public GameObject ventanaDescripcionesRef;
	public GameObject ventanaObjetosRef;

	//----- inventario
	private Vector2 posicionBarraScrollInventario;
	private bool enInventario = false;
	//---

	private List<Etiqueta> listaObjetos = new List<Etiqueta>();
	private List<Etiqueta> listaTiradas = new List<Etiqueta>();

	void Awake()
	{
		DontDestroyOnLoad(this);
		this.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		Cargar_Ventanas_Colgantes();
		Desactivar_Ventanas();
	}

	void Update()
	{
		switch (GameCenter.InstanceRef.controladoraJugador.Devolver_Estado ()) 
		{
			case EstadosJugador.enEspera:	
				JugadorEnEspera ();
				break;
				
			case EstadosJugador.enZoomEspera:
				JugadorEnZoomEspera ();
				break;
		}
	}

	private void Cargar_Ventanas_Colgantes()
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			switch(objetoHijo.name)
			{
				case "VentanaDescripciones":
					ventanaDescripcionesRef = objetoHijo.gameObject;
					break;

				case "VentanaConversaciones":
					ventanaConversacionesRef = objetoHijo.gameObject;
					break;

				case "VentanaObjetos":
					ventanaObjetosRef = objetoHijo.gameObject;
					break;
			}
		}
	}

	private void Desactivar_Ventanas()
	{
		ventanaDescripcionesRef.SetActive(false);
		ventanaConversacionesRef.SetActive(false);
		ventanaObjetosRef.SetActive(false);
	}

	private void JugadorEnEspera()
	{
		ventanaDescripcionesRef.SetActive(true);
		ventanaConversacionesRef.SetActive(false);
		ventanaObjetosRef.SetActive(false);
	}

	private void JugadorEnZoomEspera()
	{
		ventanaDescripcionesRef.SetActive(true);
		ventanaConversacionesRef.SetActive(false);
		ventanaObjetosRef.SetActive(true);
	}

	public void JugadorEnZoomEsperaConversacion()
	{
		ventanaDescripcionesRef.SetActive(false);
		ventanaConversacionesRef.SetActive(true);
		ventanaObjetosRef.SetActive(true);
	}

	public void Mostrar_Inventario()
	{
		GUILayout.Window (0, new Rect (200, Screen.height - 190, Screen.width - 400, 190), ventanaInventario, "Inventario");
	}

	public void ventanaInventario(int id)
	{
		posicionBarraScrollInventario = GUILayout.BeginScrollView (posicionBarraScrollInventario, GUIStyle.none);
		GUILayout.BeginHorizontal ();
		//TODO: chekear xml y poner imagenes en boton
		GUILayout.Box ("prueba", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba2", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba2", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba2", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba2", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.EndHorizontal ();
		GUILayout.EndScrollView();
	}



}




