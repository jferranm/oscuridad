using UnityEngine;
using System.Collections;
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
	public ObjetoInteractuablev2 objetoInteractuableRef;
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
	private string nombreJugador;

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

	public void Update()
	{
		if (estadoCambiado) 
		{
			switch (EstadoJugador) 
			{
				case EstadosJugador.enZoomIn:
					JugadorEnZoomIn();
					break;
					
				case EstadosJugador.enZoomOut:
					JugadorEnZoomOut();
					break;
			}
		}
	}

	private void CambioEnEstado()
	{
		if (EstadoJugador.Equals (EstadosJugador.enZoomIn) || EstadoJugador.Equals (EstadosJugador.enZoomOut)) 
		{
			estadoCambiado = true;
		}
	}

	public void Nombre_Jugador(string nombre)
	{
		nombreJugador = nombre;
	}
	
	public string Devolver_Nombre_Jugador ()
	{
		return nombreJugador;
	}
	
	private void JugadorEnZoomIn()
	{
		Vector3 vectorAuxiliarPosicion = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.PosicionNueva;
		Vector3 vectorAuxiliarRotacion = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.RotacionNueva;
		float smoothAuxiliar = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Smooth;

		//Ruta de la camara hacia el objeto seleccionado
		GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position = Vector3.Lerp(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position, vectorAuxiliarPosicion, Time.deltaTime*smoothAuxiliar);
		GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.rotation = Quaternion.Lerp(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.rotation, Quaternion.Euler(vectorAuxiliarRotacion.x, vectorAuxiliarRotacion.y, vectorAuxiliarRotacion.z), Time.deltaTime*smoothAuxiliar);

		if (GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position.ToString().Equals(vectorAuxiliarPosicion.ToString())) 
		{
			EstadoJugador = EstadosJugador.enZoomEspera;
			estadoCambiado = false;
		} 
	}

	private void JugadorEnZoomOut()
	{
		float smoothAuxiliar = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Smooth;

		//Ruta del objeto a la posicion inicial de la camara
		GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position = Vector3.Lerp(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position, zoomCamaraRef.posicionInicial, Time.deltaTime*smoothAuxiliar);
		GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.rotation = Quaternion.Lerp(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.rotation, zoomCamaraRef.rotacionInicial, Time.deltaTime*smoothAuxiliar);
		
		if(GameCenter.InstanceRef.controladoraJuego.cameraActiva.transform.position.ToString() == zoomCamaraRef.posicionInicial.ToString())
		{
			EstadoJugador = EstadosJugador.enEspera;
			estadoCambiado = false;
			objetoInteractuableRef = null;
		}
	}

	#endregion
}
