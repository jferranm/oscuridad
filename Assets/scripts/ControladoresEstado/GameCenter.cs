using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;

public class GameCenter : MonoBehaviour 
{
	//Controladoras
	public ControladoraJugador controladoraJugador;
	public ControladoraEscenas controladoraEscenas;
	public ControladoraSonidos controladoraSonidos;
	public ControladoraTextos controladoraTextos;
	public ControladoraObjetos controladoraObjetos;
	public ControladoraGUI controladoraGUI;
	public ControladoraJuego controladoraJuego;

	public string USERPATH;

	//Instancia Singleton
	private static GameCenter instanceRef;
	public static GameCenter InstanceRef
	{
		get { return instanceRef; }
	}

	private EstadoJuego estadoActual;
	
	public EstadoJuego EstadoActual
	{
		get { return estadoActual; }
		set 
		{
			estadoActual = value;
		}
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
		//-------------- Inicializo los Handlers ----------------\\
		controladoraJugador = ControladoraJugador.InstanceRef ();
		controladoraEscenas = ControladoraEscenas.InstanceRef ();
		controladoraSonidos = ControladoraSonidos.InstanceRef ();
		controladoraTextos = ControladoraTextos.InstanceRef ();
		controladoraObjetos = ControladoraObjetos.InstanceRef ();
		controladoraGUI = ControladoraGUI.InstanceRef ();
		controladoraJuego = ControladoraJuego.InstanceRef ();

		controladoraGUI.LocalizarObjetos ();
		controladoraJugador.Cambiar_Estado(EstadosJugador.enMenus);

		USERPATH = Application.persistentDataPath;
	}

	void Start()
	{
		controladoraGUI.Start ();
		controladoraSonidos.Start ();
		controladoraEscenas.CambiarSceneSegunEnum (EstadoJuego.MenuPrincipal);
	}
	
	void Update()
	{
		controladoraGUI.Update ();
		controladoraJugador.Update ();
		controladoraGUI.Update ();
	}

	void OnGUI()
	{
		controladoraEscenas.OnGUI ();
		controladoraGUI.OnGUI ();
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
		//Desactivamos todas las ventanas
		controladoraGUI.DesactivarGUI ();

		//Le pasamos el nombre de la escena a la controladora de Textos
	//	controladoraTextos.Generar_Descripcion();

		//Cargamos el estado de los objetos en la escena...
		controladoraObjetos.Cargar_Estado_Objetos ();
		
		//Iniciamos el Jugador
		controladoraJugador.Cambiar_Estado(EstadosJugador.enEspera);

		//Comenzamos BSO
		Camera.main.audio.clip = controladoraSonidos.bsoEscenaCasa;
		Camera.main.audio.Play ();
	}


}
