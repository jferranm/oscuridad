using UnityEngine;
using System.Collections;
using Oscuridad.Enumeraciones;

public class GameCenter : MonoBehaviour 
{
	//Controladoras
	public ControladoraJugador controladoraJugador;
	public ControladoraEscenas controladoraEscenas;
	public ControladoraSonidos controladoraSonidos;
	public ControladoraGUI controladoraGUI;
	public ControladoraJuego controladoraJuego;

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
		//-------------- Inicializo los Handlers ----------------\\
		controladoraJugador = ControladoraJugador.InstanceRef ();
		controladoraEscenas = ControladoraEscenas.InstanceRef ();
		controladoraSonidos = ControladoraSonidos.InstanceRef ();
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
		controladoraEscenas.CambiarSceneSegunEnum (Escenas.MenuPrincipal);
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
		controladoraJuego.Inicializar_Objetos ();

		controladoraGUI.Vaciar_Cajas_Texto ();

		//Desactivamos todas las ventanas
		controladoraGUI.DesactivarGUI ();

		//Iniciamos el Jugador
		controladoraJugador.Cambiar_Estado(EstadosJugador.enEspera);

		//Comenzamos BSO
		Camera.main.audio.clip = controladoraSonidos.bsoEscenaCasa;
		Camera.main.audio.Play ();
	}


}
