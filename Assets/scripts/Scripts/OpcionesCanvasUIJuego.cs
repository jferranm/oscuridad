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
	public GameObject libroDiario;

	void Start()
	{
		Desactivar_Todo ();
	}

	void Update()
	{
		if (libroDiario.activeSelf) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
				libroDiario.SetActive(!libroDiario.activeSelf);
		}
	}

	private void Desactivar_Todo()
	{
		panelObjetos.SetActive (false);
		panelDirecciones.SetActive (false);
		botonDiario.SetActive (false);
		imagenCargando.SetActive (false);
		panelLateral.SetActive (false);
		panelInferior.SetActive (false);
		libroDiario.SetActive (false);
	}
}
