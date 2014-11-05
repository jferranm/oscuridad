using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;

[System.Serializable]
public class ControladoraGUI
{
	//Texturas de menus de inicio
	public Texture2D texturaMenuInicio;
	public Texture2D texturaMenuPersonaje;
	public Texture2D texturaMenuOpciones;
	public Texture2D texturaCargando;
	public Texture2D texturaMenuHojaPersonaje;
	public Texture2D texturaBotonComenzar;
	public Texture2D texturaBotonComenzarHover;
	public Texture2D texturaBotonContinuar;
	public Texture2D texturaBotonContinuarHover;
	public Texture2D texturaBotonOpciones;
	public Texture2D texturaBotonOpcionesHover;
	public Texture2D texturaBotonSalir;
	public Texture2D texturaBotonSalirHover;
	public Texture2D texturaBotonNuevaPartida;
	public Texture2D texturaBotonNuevaPartidaHover;
	public Texture2D texturaBotonAtras;
	public Texture2D texturaBotonAtrasHover;
	public Texture2D texturaBotonMarlaGibbs;
	public Texture2D texturaBotonMarlaGibbsHover;
	public Texture2D texturaBotonRobertDuncan;
	public Texture2D texturaBotonRobertDuncanHover;
	public Texture2D texturaBotonWarrenBedford;
	public Texture2D texturaBotonWarrenBedfordHover;
	public Texture2D texturaBotonSeleccionar;
	public Texture2D texturaBotonSeleccionarHover;
	public Texture2D texturaFlechas;
	public Texture2D texturaFlechasIzqOK;
	public Texture2D texturaFlechasIzqNo;
	public Texture2D texturaFlechasDerOK;
	public Texture2D texturaFlechasDerNo;
	public Texture2D texturaFlechasArrOK;
	public Texture2D texturaFlechasArrNo;
	public Texture2D texturaFlechasAbjOK;
	public Texture2D texturaFlechasAbjNo;
	public Texture2D texturaLibro;


	//Booleana de cambio de estado del jugador
	[HideInInspector]
	public bool estadoCambiado;
	//Controlador del juego
	
	//Paneles de GUI
	private GameObject menuObjetos;
	private GameObject menuOpciones;
	private GameObject menuVentana;
	private GameObject menuDirecciones;
	private GameObject pantallaCarga;

	//---- Cursores
	public Texture2D texturaCursorNormal;
	public Texture2D texturaCursorSeleccion;
	//----
	
	//---- Booleanas de gui
	[HideInInspector]
	public bool mostrarVentana = false;
	[HideInInspector]
	public bool mostrarInventario = false;
	//----
	
	//---- Fades
	//----

	public ControladoraGUI()
	{

	}
	
	private static ControladoraGUI instanceRef;
	
	public static ControladoraGUI InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraGUI();
		}
		
		return instanceRef;
	}

	public void Start()
	{
		texturaMenuInicio = Resources.Load("Imagenes/GUI/Menus/background_2", typeof(Texture2D)) as Texture2D;
		texturaMenuPersonaje = Resources.Load("Imagenes/GUI/Menus/background_2", typeof(Texture2D)) as Texture2D;
		texturaMenuOpciones = Resources.Load("Imagenes/GUI/Menus/background_2", typeof(Texture2D)) as Texture2D;
		texturaCargando = Resources.Load("Imagenes/GUI/PantallaCargando", typeof(Texture2D)) as Texture2D;
		texturaMenuHojaPersonaje = Resources.Load("Imagenes/GUI/Menus/HojaPersonaje", typeof(Texture2D)) as Texture2D;
		texturaBotonComenzar = Resources.Load("Imagenes/GUI/Menus/BotonComenzar", typeof(Texture2D)) as Texture2D;
		texturaBotonComenzarHover = Resources.Load("Imagenes/GUI/Menus/BotonComenzar", typeof(Texture2D)) as Texture2D;
		texturaBotonContinuar = Resources.Load("Imagenes/GUI/Menus/Continuar1", typeof(Texture2D)) as Texture2D;
		texturaBotonContinuarHover = Resources.Load("Imagenes/GUI/Menus/Continuar2", typeof(Texture2D)) as Texture2D;
		texturaBotonOpciones = Resources.Load("Imagenes/GUI/Menus/Opciones1", typeof(Texture2D)) as Texture2D;
		texturaBotonOpcionesHover = Resources.Load("Imagenes/GUI/Menus/Opciones2", typeof(Texture2D)) as Texture2D;
		texturaBotonSalir = Resources.Load("Imagenes/GUI/Menus/Salir1", typeof(Texture2D)) as Texture2D;
		texturaBotonSalirHover = Resources.Load("Imagenes/GUI/Menus/Salir2", typeof(Texture2D)) as Texture2D;
		texturaBotonNuevaPartida = Resources.Load("Imagenes/GUI/Menus/BotonNuevaPartida", typeof(Texture2D)) as Texture2D;
		texturaBotonNuevaPartidaHover = Resources.Load("Imagenes/GUI/Menus/BotonNuevaPartida", typeof(Texture2D)) as Texture2D;
		texturaBotonAtras = Resources.Load("Imagenes/GUI/Menus/BotonAtras", typeof(Texture2D)) as Texture2D;
		texturaBotonAtrasHover = Resources.Load("Imagenes/GUI/Menus/BotonAtras", typeof(Texture2D)) as Texture2D;
		texturaBotonMarlaGibbs = Resources.Load("Imagenes/GUI/Menus/BotonMarlaGibbs", typeof(Texture2D)) as Texture2D;
		texturaBotonMarlaGibbsHover = Resources.Load("Imagenes/GUI/Menus/BotonMarlaGibbs", typeof(Texture2D)) as Texture2D;
		texturaBotonRobertDuncan = Resources.Load("Imagenes/GUI/Menus/BotonRobertDuncan", typeof(Texture2D)) as Texture2D;
		texturaBotonRobertDuncanHover = Resources.Load("Imagenes/GUI/Menus/BotonRobertDuncan", typeof(Texture2D)) as Texture2D;
		texturaBotonWarrenBedford = Resources.Load("Imagenes/GUI/Menus/BotonWarrenBedford", typeof(Texture2D)) as Texture2D;
		texturaBotonWarrenBedfordHover = Resources.Load("Imagenes/GUI/Menus/BotonWarrenBedford", typeof(Texture2D)) as Texture2D;
		texturaBotonSeleccionar = Resources.Load("Imagenes/GUI/Menus/BotonSeleccionar", typeof(Texture2D)) as Texture2D;
		texturaBotonSeleccionarHover = Resources.Load("Imagenes/GUI/Menus/BotonSeleccionar", typeof(Texture2D)) as Texture2D;
		texturaFlechas = Resources.Load("Imagenes/GUI/flechas", typeof(Texture2D)) as Texture2D;
		texturaFlechasAbjOK = Resources.Load("Imagenes/GUI/abjOk", typeof(Texture2D)) as Texture2D;
		texturaFlechasAbjNo = Resources.Load("Imagenes/GUI/abjOut", typeof(Texture2D)) as Texture2D;
		texturaFlechasArrOK = Resources.Load("Imagenes/GUI/arrOk", typeof(Texture2D)) as Texture2D;
		texturaFlechasArrNo = Resources.Load("Imagenes/GUI/arrOut", typeof(Texture2D)) as Texture2D;
		texturaFlechasDerOK = Resources.Load("Imagenes/GUI/derOk", typeof(Texture2D)) as Texture2D;
		texturaFlechasDerNo = Resources.Load("Imagenes/GUI/derOut", typeof(Texture2D)) as Texture2D;
		texturaFlechasIzqOK = Resources.Load("Imagenes/GUI/izqOk", typeof(Texture2D)) as Texture2D;
		texturaFlechasIzqNo = Resources.Load("Imagenes/GUI/izqOut", typeof(Texture2D)) as Texture2D;
		texturaLibro = Resources.Load("Imagenes/GUI/Diario", typeof(Texture2D)) as Texture2D;
	}

	public void Update()
	{
		if (estadoCambiado) 
		{
			switch (GameCenter.InstanceRef.controladoraJugador.Devolver_Estado()) 
			{
			case EstadosJugador.enEspera:	
				JugadorEnEspera ();
				break;
				
			case EstadosJugador.enZoomIn:
				JugadorEnZoomIn ();
				break;
				
			case EstadosJugador.enZoomOut:
				JugadorEnZoomOut ();
				break;
				
			case EstadosJugador.enZoomEspera:
				JugadorEnZoomEspera ();
				break;
				
			case EstadosJugador.enMenus:
				JugadorEnMenu ();
				break;
			}
		}
	}
	
	public void OnGUI()
	{

	}
	
	private void JugadorEnEspera()
	{
		//Activar Opciones de Juego
		if(!Devolver_Pantalla_Carga().comenzarFade)
		{
			Activar_Opciones_Basicas();
		}
		estadoCambiado = false;
	}
	
	private void JugadorEnZoomIn()
	{
		//Desactivamos Ventanas
		DesactivarGUI ();
		estadoCambiado = false;
	}
	
	private void JugadorEnZoomOut()
	{
		//Desactivamos Ventanas
		DesactivarGUI ();
		estadoCambiado = false;
	}
	
	private void JugadorEnMenu()
	{
		
	}
	
	private void JugadorEnZoomEspera()
	{
		//Activamos el Menu
		Activar_Opciones_Basicas();
		menuObjetos.SetActive(true);

		estadoCambiado = false;
	}
	
	public void Mostrar_Ventana_Descriptiva ()
	{
		
	}
	
	public void LocalizarObjetos()
	{
		menuObjetos = GameObject.Find ("PanelGuiObjetos");
		menuOpciones = GameObject.Find ("PanelGuiOpciones");
		menuDirecciones = GameObject.Find ("PanelGuiDirecciones");
		menuVentana = GameObject.Find ("PanelGuiVentana");
		pantallaCarga = GameObject.Find ("PantallaCarga");
	}
	
	public void DesactivarGUI()
	{
		menuObjetos.SetActive (false);
		menuOpciones.SetActive (false);
		menuDirecciones.SetActive (false);
		menuVentana.SetActive (false);
	}

	public void Activar_Cargando()
	{
		pantallaCarga.SetActive(true);
	}

	public PantallaCarga Devolver_Pantalla_Carga()
	{
		return pantallaCarga.GetComponent<PantallaCarga>();
	}

	public void Activar_Opciones_Basicas ()
	{
		menuOpciones.SetActive (true);
		menuVentana.SetActive (true);
		menuDirecciones.SetActive (true);
	}

	public void Lanzar_Inspeccionar()
	{
	}
}