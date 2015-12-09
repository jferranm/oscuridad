using UnityEngine;
using System.Collections;
using System;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;

[System.Serializable]
public class ControladoraJugador 
{
	#region VARIABLES

	[HideInInspector]
	public Vector3 posicionInicial;
	[HideInInspector]
	public Quaternion rotacionInicial;
	[HideInInspector]
	public ZoomCamara zoomCamaraRef;
	[HideInInspector]
	public bool estadoCambiado;
	[HideInInspector]
	public GameObject objetoPulsado;

	private EstadosJugador estadoJugador;
	[HideInInspector]
	public EstadosJugador EstadoJugador
	{
		get { return estadoJugador; }
		set
		{
			estadoJugador = value;
			CambioEnEstado();
			GameCenter.InstanceRef.controladoraGUI.CambioEnEstado();
		}
	}

	#endregion

	#region CONSTRUCTORES

	public ControladoraJugador()
	{
	}

	#endregion

	#region METODOS

	public void Start()
	{
		EstadoJugador = EstadosJugador.enMenus;
	}

	private void CambioEnEstado()
	{
		if (EstadoJugador.Equals (EstadosJugador.enZoomIn) || EstadoJugador.Equals (EstadosJugador.enZoomOut)) 
		{
			if (EstadoJugador.Equals (EstadosJugador.enZoomIn))
				GameCenter.InstanceRef.StartCoroutine (GameCenter.InstanceRef.controladoraJuego.Mover3D (GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform, GameCenter.InstanceRef.controladoraJuego.interactuablePulsadoOpciones.posicion, Quaternion.Euler(GameCenter.InstanceRef.controladoraJuego.interactuablePulsadoOpciones.rotacion), GameCenter.InstanceRef.controladoraJuego.interactuablePulsadoOpciones.suavizado, EstadosJugador.enZoomEspera));
			else
				GameCenter.InstanceRef.StartCoroutine (GameCenter.InstanceRef.controladoraJuego.Mover3D (GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform, zoomCamaraRef.posicionInicial, zoomCamaraRef.rotacionInicial, GameCenter.InstanceRef.controladoraJuego.interactuablePulsadoOpciones.suavizado, EstadosJugador.enEspera));				
		}
	}

	#endregion
}
