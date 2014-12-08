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
	[HideInInspector]
	public GameObject panelLateral;
	[HideInInspector]
	public GameObject panelInferior;

	void Start()
	{
		panelObjetos = GameObject.Find ("PanelObjetos");
		panelDirecciones = GameObject.Find ("PanelDirecciones");
		botonDiario = GameObject.Find ("BotonDiario");
		imagenCargando = GameObject.Find ("ImagenCargando");
		panelLateral = GameObject.Find ("PanelLateral");
		panelInferior = GameObject.Find ("PanelInferior");

		GameCenter.InstanceRef.controladoraGUI.imagenCargando = imagenCargando;
		GameCenter.InstanceRef.controladoraGUI.panelLateral = panelLateral;
		GameCenter.InstanceRef.controladoraGUI.panelInferior = panelInferior;
		GameCenter.InstanceRef.controladoraGUI.panelDirecciones = panelDirecciones;
		GameCenter.InstanceRef.controladoraGUI.botonDiario = botonDiario;
		GameCenter.InstanceRef.controladoraGUI.panelObjetos = panelObjetos;

		Desactivar_Todo ();
	}

	void onEnable()
	{
	}

	private void Desactivar_Todo()
	{
		panelObjetos.SetActive (false);
		panelDirecciones.SetActive (false);
		botonDiario.SetActive (false);
		imagenCargando.SetActive (false);
		panelLateral.SetActive (false);
		panelInferior.SetActive (false);
	}
}
