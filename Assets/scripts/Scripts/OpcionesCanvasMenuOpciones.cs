using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;

public class OpcionesCanvasMenuOpciones : MonoBehaviour 
{
    void Start () 
	{
		this.gameObject.SetActive (false);
	}

	void Update()
	{
		if (this.gameObject.activeSelf) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				this.gameObject.SetActive(!this.gameObject.activeSelf);
				GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enEspera;
			}
		}
	}
}
