using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;

public class OpcionesCanvasUIJuego : MonoBehaviour 
{
	public GameObject panelObjetos;
	public GameObject panelDirecciones;
	public GameObject botonDiario;
	public GameObject imagenCargando;
	public GameObject panelLateral;
	public GameObject panelInferior;
	public GameObject libroDiario;
	public GameObject Mapa;
	public GameObject MenuOpciones;
	public GameObject fondoOscuro;

	void Start()
	{
		Desactivar_Todo ();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			fondoOscuro.SetActive(!fondoOscuro.activeSelf);

			if (libroDiario.activeSelf)
			{
				libroDiario.SetActive(!libroDiario.activeSelf);
				return;
			}

			if(Mapa.activeSelf)
			{
				Mapa.SetActive(!Mapa.activeSelf);
				return;
			}

			MenuOpciones.SetActive(!MenuOpciones.activeSelf);
			GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enEspera;
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
		Mapa.SetActive (false);
		MenuOpciones.SetActive (false);
		fondoOscuro.SetActive (false);
	}
}
