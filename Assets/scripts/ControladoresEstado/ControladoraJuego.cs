using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

[System.Serializable]
public class ControladoraJuego
{
	public JugadorBase jugadorActual;
	public EscenaBase escenaActual;

	private static ControladoraJuego instanceRef;
	public static ControladoraJuego InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraJuego();
			instanceRef.Inicializar();
		}
		
		return instanceRef;
	}

	public void Inicializar()
	{
		jugadorActual = new JugadorBase ();
		jugadorActual.EstadoJugador = estadosJugador.enMenus;

		escenaActual = new EscenaBase ();
	}

	public void InicializarEscena()
	{
		escenaActual = new EscenaBase ();
	}
}
