using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

public class GameCenter : MonoBehaviour 
{
	#region VARIABLES

	//Controladoras
	public ControladoraJugador controladoraJugador;
	public ControladoraEscenas controladoraEscenas;
	public ControladoraSonidos controladoraSonidos;
	public ControladoraGUI controladoraGUI;
	public ControladoraJuego controladoraJuego;
	public GameObject CanvasUIJuego;
	public GameObject CanvasMenuPrincipal;
	public GameObject CanvasMenuOpciones;

	public string USERPATH;

	//Instancia Singleton
	private static GameCenter instanceRef;
	public static GameCenter InstanceRef
	{
		get { return instanceRef; }
	}

	#endregion

	#region METODOS UNITY

	void Awake()
	{
		if (instanceRef == null)
		{
			instanceRef = this;
			DontDestroyOnLoad(this);
			InicializarControladoras();
		}
		else
		{
			Destroy(this.gameObject);
		}

		controladoraGUI.Awake ();
		controladoraSonidos.Awake ();
	}

	void Start()
	{

		controladoraJuego.Start ();
		controladoraJugador.Start ();
		
		controladoraEscenas.CambiarSceneSegunEnum (Escenas.MenuPrincipal);
		
		//-------------- Opciones varias -------------------------\\
		CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().TraduccionMenu ();
		CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().escena0.SetActive (true);
		CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().escena1.SetActive (false);
		CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().escena2.SetActive (false);
	}
	
	void Update()
	{
		controladoraJugador.Update ();
	}
	
	void OnLevelWasLoaded(int level)
	{
		controladoraEscenas.OnLevelWasLoaded(level);
	}

	#endregion

	#region METODOS PROPIOS
	
	private void InicializarControladoras()
	{
		USERPATH = Application.persistentDataPath;

		//-------------- Inicializo los Handlers ----------------\\
		controladoraJugador = new ControladoraJugador();
		controladoraEscenas = new ControladoraEscenas();
		controladoraSonidos = new ControladoraSonidos();
		controladoraGUI = new ControladoraGUI();
		controladoraJuego = new ControladoraJuego();
	}
	
	public void Inicializar_Escenario()
	{
		controladoraJuego.Inicializar_Interactuables ();
		controladoraJuego.Inicializar_Interactuables_SinZoom ();

		//Desactivamos todas las ventanas
		controladoraGUI.DesactivarGUI ();

		//Activamos Camara
		controladoraJuego.Desactivar_Camaras ();
		if (controladoraJuego.camaraActivar != null) 
		{
			controladoraJuego.Cambiar_Camara (controladoraJuego.camaraActivar);
			controladoraJuego.camaraActivar = null;
		}
		else
			controladoraJuego.Cambiar_Camara (controladoraJuego.escenaActual.CamaraInicio.Nombre);

		//Comenzamos BSO
		controladoraSonidos.Lanzar_Bso (Application.loadedLevelName);

		CanvasUIJuego.SetActive (true);

		controladoraJuego.configuracionJuego.UltimaEscenaVisitada = controladoraJuego.escenaActual.Escena;
		controladoraJuego.configuracionJuego.UltimaCamaraVisitada = controladoraJuego.escenaActual.CamaraInicio.Nombre;
		GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddEscenaVisitada(controladoraJuego.escenaActual.Escena);
	}

	public void Salir()
	{
		//controladoraJuego.configuracionJuego.SonidoActivado = CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().toggleSonido.isOn;
		//controladoraJuego.configuracionJuego.MusicaActivada = CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().toggleMusica.isOn;
		//controladoraJuego.configuracionJuego.VolumenSonido = CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderSonido.value;
		//controladoraJuego.configuracionJuego.VolumenMusica = CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderMusica.value;

		controladoraJuego.GrabarConfiguracion ();
		controladoraJuego.GrabarJugador ();
		controladoraJuego.Guardar_Escena (GameCenter.instanceRef.controladoraJuego.escenaActual.Escena);
		Application.Quit ();
	}

	#endregion
}
