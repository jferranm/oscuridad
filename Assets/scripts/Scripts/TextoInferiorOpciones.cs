using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Oscuridad.Enumeraciones;

public class TextoInferiorOpciones : MonoBehaviour 
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
		if (GameCenter.InstanceRef.controladoraJuego.objetoPulsado == null) 
			textoVentana.text = "";
		else 
			textoVentana.text = "Inspeccionando \"" + GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre + "\"";
	}
}
