using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

public class GameCenter : MonoBehaviour 
{
	//Controladoras
	public ControladoraJugador controladoraJugador;
	public ControladoraEscenas controladoraEscenas;
	public ControladoraSonidos controladoraSonidos;
	public ControladoraGUI controladoraGUI;
	public ControladoraJuego controladoraJuego;
	public GameObject CanvasUIJuego;
	public GameObject CanvasMenuPrincipal;

	public string USERPATH;

	//Instancia Singleton
	private static GameCenter instanceRef;
	public static GameCenter InstanceRef
	{
		get { return instanceRef; }
	}

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
		
	}
	
	private void InicializarControladoras()
	{
		USERPATH = Application.persistentDataPath;

		//-------------- Inicializo los Handlers ----------------\\
		controladoraJugador = ControladoraJugador.InstanceRef ();
		controladoraEscenas = ControladoraEscenas.InstanceRef ();
		controladoraSonidos = ControladoraSonidos.InstanceRef ();
		controladoraGUI = ControladoraGUI.InstanceRef ();
		controladoraJuego = ControladoraJuego.InstanceRef ();

		//-------------- Opciones varias -------------------------\\
		CanvasUIJuego = GameObject.Find ("CanvasUIJuego");
		CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().escena0.SetActive (true);
		CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().escena1.SetActive (false);
		CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().escena2.SetActive (false);
	}

	void Start()
	{
		controladoraGUI.Start ();
		controladoraSonidos.Start ();
		controladoraEscenas.CambiarSceneSegunEnum (Escenas.MenuPrincipal);
	}
	
	void Update()
	{
		controladoraJugador.Update ();
	}

	void OnGUI()
	{
		controladoraEscenas.OnGUI ();
	}

	void OnLevelWasLoaded(int level)
	{
		controladoraEscenas.OnLevelWasLoaded(level);
	}

	public void CoroutinaBase(IEnumerator CoroutinaAEjecutar)
	{
		StartCoroutine(CoroutinaAEjecutar);
	}

	public void Inicializar_Escenario()
	{
		controladoraJuego.Inicializar_Objetos ();

		//Desactivamos todas las ventanas
		controladoraGUI.DesactivarGUI ();

		//Activamos Camara
		controladoraJuego.camaraActiva = null;
		controladoraJuego.cameraActiva = null;
		controladoraJuego.Desactivar_Camaras ();
		controladoraJuego.Cambiar_Camara (controladoraJuego.escenaActual.CamaraInicio.Nombre);

		//Comenzamos BSO
		controladoraSonidos.Lanzar_Bso (Application.loadedLevelName);

		//Insertamos marcador de Escena visitada en el jugador
		controladoraJuego.jugadorActual.AddEscenaVisitada (controladoraJuego.camaraActiva.Escena);

		CanvasUIJuego.SetActive (true);
	}
}
