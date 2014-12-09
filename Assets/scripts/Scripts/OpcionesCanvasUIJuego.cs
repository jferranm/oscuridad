using UnityEngine;
using System.Collections;

public class OpcionesCanvasUIJuego : MonoBehaviour 
{
	public GameObject panelObjetos;
	public GameObject panelDirecciones;
	public GameObject botonDiario;
	public GameObject imagenCargando;
	public GameObject panelLateral;
	public GameObject panelInferior;

	void Start()
	{
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
