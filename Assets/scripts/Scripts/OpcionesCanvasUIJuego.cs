using UnityEngine;
using System.Collections;

public class OpcionesCanvasUIJuego : MonoBehaviour 
{
	[HideInInspector]
	public GameObject panelObjetos;
	[HideInInspector]
	public GameObject panelDirecciones;
	[HideInInspector]
	public GameObject botonDiario;
	[HideInInspector]
	public GameObject imagenCargando;

	void Start()
	{
		panelObjetos = GameObject.Find ("PanelObjetos");
		panelDirecciones = GameObject.Find ("PanelDirecciones");
		botonDiario = GameObject.Find ("BotonDiario");
		imagenCargando = GameObject.Find ("ImagenCargando");

		GameCenter.InstanceRef.controladoraGUI.imagenCargando = imagenCargando;

		panelObjetos.SetActive (false);
		panelDirecciones.SetActive (false);
		botonDiario.SetActive (false);
		imagenCargando.SetActive (false);
	}

	void onEnable()
	{
		botonDiario.SetActive (true);
		panelDirecciones.SetActive (true);
	}
}
