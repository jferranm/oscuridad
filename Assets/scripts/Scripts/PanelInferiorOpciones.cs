using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class PanelInferiorOpciones : MonoBehaviour 
{
	Text textoVentana;

	void Start()
	{
		textoVentana = this.transform.GetChild (0).GetComponent<Text>();
	}

	void OnEnable()
	{
		if (GameCenter.InstanceRef != null) 
		{
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
//		GameCenter.InstanceRef.controladoraGUI.listaVentanaInferior.Add(new Etiqueta(GameCenter.InstanceRef.controladoraJuego.escenaActual.Descripcion, Color.white));
	}

	private void JugadorEnZoomEspera()
	{
	}
}
