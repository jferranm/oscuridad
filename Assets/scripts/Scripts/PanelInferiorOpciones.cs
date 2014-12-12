using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class PanelInferiorOpciones : MonoBehaviour 
{
	public Text textoVentana;

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
			GameCenter.InstanceRef.controladoraGUI.textoInferior = textoVentana;

			switch (GameCenter.InstanceRef.controladoraJugador.EstadoJugador) 
			{
				case EstadosJugador.enEspera:
						JugadorEnEspera ();
						break;

				case EstadosJugador.enZoomEspera:
						JugadorEnZoomEspera ();
						break;
			}
		}
	}

	private void JugadorEnEspera()
	{
		textoVentana.text = GameCenter.InstanceRef.controladoraJuego.escenaActual.Descripcion;
	}

	private void JugadorEnZoomEspera()
	{
		textoVentana.text = "";
	}
}
