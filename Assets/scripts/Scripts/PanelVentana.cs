using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

public class PanelVentana : MonoBehaviour 
{
	public GameObject ventanaConversacionesRef;
	public GameObject ventanaDescripcionesRef;
	public GameObject ventanaObjetosRef;

	//---- Ventana Descripciones Objetos
	private int ventanaIDObjeto;
	private string textoCabeceraObjeto;
	private Vector2 posicionBarraScrollObjeto;
	//----

	//---- Ventana Descripcion Tiradas
	private int ventanaIDTiradas;
	private string textoCabeceraTiradas;
	private Vector2 posicionBarraScrollTiradas;
	//-------
	
	private bool cajaDescriptivaObjeto;

	//----- inventario
	private Vector2 posicionBarraScrollInventario;
	private bool enInventario = false;
	//---

	//---- Opciones de Jugador
	[HideInInspector]
	public string textoBoton1 = "";
	[HideInInspector]
	public string textoBoton2 = "";
	[HideInInspector]
	public string textoBoton3 = "";
	[HideInInspector]
	public int numeroPregunta1;
	[HideInInspector]
	public int numeroPregunta2;
	[HideInInspector]
	public int numeroPregunta3;
	[HideInInspector]
	public string textoPregunta;
	//------

	private GUIStyle fuenteNegra;
	private GUIStyle fuenteRoja;
	private GUIStyle fuenteVerde;

	private List<Etiqueta> listaObjetos = new List<Etiqueta>();
	private List<Etiqueta> listaTiradas = new List<Etiqueta>();

	void Awake()
	{
		DontDestroyOnLoad(this);
		this.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		Cargar_Ventanas_Colgantes();
		Desactivar_Ventanas();
	}

	void Update()
	{
		switch (GameCenter.InstanceRef.controladoraJugador.Devolver_Estado ()) 
		{
			case EstadosJugador.enEspera:	
				JugadorEnEspera ();
				break;
				
			case EstadosJugador.enZoomEspera:
				JugadorEnZoomEspera ();
				break;
		}
	}

	private void Cargar_Ventanas_Colgantes()
	{
		foreach (Transform objetoHijo in this.transform) 
		{
			switch(objetoHijo.name)
			{
				case "VentanaDescripciones":
					ventanaDescripcionesRef = objetoHijo.gameObject;
					break;

				case "VentanaConversaciones":
					ventanaConversacionesRef = objetoHijo.gameObject;
					break;

				case "VentanaObjetos":
					ventanaObjetosRef = objetoHijo.gameObject;
					break;
			}
		}
	}

	private void Desactivar_Ventanas()
	{
		ventanaDescripcionesRef.SetActive(false);
		ventanaConversacionesRef.SetActive(false);
		ventanaObjetosRef.SetActive(false);
	}

	private void JugadorEnEspera()
	{
		ventanaDescripcionesRef.SetActive(true);
		ventanaConversacionesRef.SetActive(false);
		ventanaObjetosRef.SetActive(false);
	}

	private void JugadorEnZoomEspera()
	{
		ventanaDescripcionesRef.SetActive(true);
		ventanaConversacionesRef.SetActive(false);
		ventanaObjetosRef.SetActive(true);
	}

	private void Mostrar_Caja_Objeto()
	{
		cajaDescriptivaObjeto = true;

		textoCabeceraObjeto = GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef.tag.ToString();
		listaObjetos.Add(new Etiqueta(Devolver_Texto_Descriptivo (GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef.tag.ToString()), Color.white));

		textoCabeceraTiradas = "";
	}

	private void Mostrar_Caja_Tiradas()
	{
		Activar_Ventana_Descripciones ();

		if (cajaDescriptivaObjeto) 
		{
			textoCabeceraTiradas = "Interaccion con " + GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef.tag.ToString();
		} 
		else 
		{
			//Mostramos la cabecera de la ventana con el nombre de la escena
			textoCabeceraTiradas = Application.loadedLevelName;

			//Mostramos la descripcion basica de la escena
			listaTiradas.Add (new Etiqueta (Devolver_Texto_Descriptivo (Application.loadedLevelName.Trim ()), Color.white));
		}
	}

	private void Creacion_Ventana_Objeto(int Id)
	{
		posicionBarraScrollObjeto = GUILayout.BeginScrollView (posicionBarraScrollObjeto, GUIStyle.none);
			GUILayout.BeginVertical ();
				foreach (Etiqueta nuevaEtiqueta in listaObjetos) 
				{
					GUIStyle fuente = new GUIStyle (GUI.skin.label);
					fuente.normal.textColor = nuevaEtiqueta.ObtenerColor();
					GUILayout.Label(nuevaEtiqueta.ObtenerTexto(), fuente);
				}
			GUILayout.EndVertical ();
		GUILayout.EndScrollView ();
	}

	public void Creacion_Ventana_Tiradas(int Id) 
	{
		switch(Id)
		{
			case 1:	
				VentanaDescripciones();
				break;
			case 2:	
		 		VentanaConversaciones();
				break;
		}
	}

	public void VentanaDescripciones()
	{

	}

	public void VentanaConversaciones()
	{
		posicionBarraScrollTiradas = GUILayout.BeginScrollView (posicionBarraScrollTiradas);
			GUILayout.BeginVertical ();
				if (GUILayout.Button(textoBoton1))
				{
					//Insertar_Label_Tabla(true, textoBoton1, Color.green);
					posicionBarraScrollObjeto.y = Mathf.Infinity;
					Limpiar_Datos();
					Iniciar_Conversacion(numeroPregunta1.ToString(), GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString ());
				}
				
				if (GUILayout.Button(textoBoton2))
				{
					//Insertar_Label_Tabla(true, textoBoton2, Color.green);
					posicionBarraScrollObjeto.y = Mathf.Infinity;
					Limpiar_Datos();
					Iniciar_Conversacion(numeroPregunta2.ToString(), GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString ());
				}
				
				if (GUILayout.Button(textoBoton3))
				{
					//Insertar_Label_Tabla(true, textoBoton3, Color.green);
					posicionBarraScrollObjeto.y = Mathf.Infinity;
					Limpiar_Datos();
					Iniciar_Conversacion(numeroPregunta3.ToString(), GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString ());
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	public void Mostrar_Inventario()
	{
		GUILayout.Window (0, new Rect (200, Screen.height - 190, Screen.width - 400, 190), ventanaInventario, "Inventario");
	}

	public void ventanaInventario(int id)
	{
		posicionBarraScrollInventario = GUILayout.BeginScrollView (posicionBarraScrollInventario, GUIStyle.none);
		GUILayout.BeginHorizontal ();
		//TODO: chekear xml y poner imagenes en boton
		GUILayout.Box ("prueba", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba2", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba2", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba2", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.Box ("prueba2", GUILayout.Width(130), GUILayout.Height(130));
		GUILayout.EndHorizontal ();
		GUILayout.EndScrollView();
	}

	public string Devolver_Texto_Descriptivo(string nombreObjeto)
	{
		Descripciones nuevaDescripcion = new Descripciones(nombreObjeto);
		string descripcionAuxiliar = nuevaDescripcion.Devolver_Descripcion();
		nuevaDescripcion.Cerrar();
		
		return descripcionAuxiliar;
	}

	public string Devolver_Texto_Descriptivo_Escena()
	{
		Descripciones nuevaDescripcion = new Descripciones(Application.loadedLevelName);
		string descripcionAuxiliar = nuevaDescripcion.Devolver_Descripcion();
		nuevaDescripcion.Cerrar();
		
		return descripcionAuxiliar;
	}

	public string Devolver_Texto_Descriptivo_Objeto()
	{
		Descripciones nuevaDescripcion = new Descripciones(GameCenter.InstanceRef.controladoraJugador.objetoPulsado);
		string descripcionAuxiliar = nuevaDescripcion.Devolver_Descripcion();
		nuevaDescripcion.Cerrar();
		
		return descripcionAuxiliar;
	}

	public void Iniciar_Conversacion(string numeroId, string tagPersonaje)
	{	
		//Construimos el objeto que va a albergar la pregunta y las respuestas
		Pregunta valoresConversacion = new Pregunta();
		
		//creamos una nueva conversacion y la comenzamos.
		Conversaciones nuevaConversacion = new Conversaciones(numeroId, tagPersonaje);
		valoresConversacion = nuevaConversacion.IniciarConversacion();
		
		//Mostramos los Datos
		if(valoresConversacion == null)
		{
			//Encendemos la ventana de Descripcion
			ventanaIDTiradas = 1;
		}
		else
		{
			Mostrar_Preguntas(valoresConversacion);
		}
	}
	
	public void Mostrar_Preguntas(Pregunta datosMostrar)
	{
		//Insertar_Label_Tabla (true, datosMostrar.textoPregunta, Color.white);
		//textoPregunta = datosMostrar.textoPregunta;
		byte cont = 1;
		foreach(RespuestaSinTirada nuevaRespuesta in datosMostrar.listaRespuestas.Values)
		{
			switch(cont)
			{
				case 1: 
					textoBoton1 = nuevaRespuesta.textoRespuesta;
					numeroPregunta1 = nuevaRespuesta.idSiguientePregunta;
					break;
				case 2: 
					textoBoton2 = nuevaRespuesta.textoRespuesta;
					numeroPregunta2 = nuevaRespuesta.idSiguientePregunta;
					break;
				case 3: 
					textoBoton3 = nuevaRespuesta.textoRespuesta;
					numeroPregunta3 = nuevaRespuesta.idSiguientePregunta;
					break;
			}
			cont++;
		}
	}
	
	public List<string> Texto_Tirada()
	{
		//Creamos una nueva descripcion segun la tirada
		Descripciones nuevaDescripcion = new Descripciones(GameCenter.InstanceRef.controladoraJugador.objetoPulsado);
		List<string> listaDescripcionAuxiliar = new List<string>(nuevaDescripcion.Devolver_Descripcion_Tiradas());
		nuevaDescripcion.Cerrar();
		
		//Hacemos que el objeto ya no sea inspeccionable;
		//GameCenter.InstanceRef.controladoraObjetos.Cambiar_Opcion_Objeto (GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef.name, false, false, false, true, false);

		return listaDescripcionAuxiliar;		
	}

	public void Limpiar_Datos()
	{
		textoPregunta = "";
		textoBoton1 = "";
		textoBoton2 = "";
		textoBoton3 = "";
	}


	public void Lanzar_Hablar()
	{
		ventanaIDTiradas = 2;

		//Iniciar_Conversacion (GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef.inicioConversacion, GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString ());

	}

	public void Activar_Ventana_Conversaciones()
	{
		ventanaIDTiradas = 2;
	}

	public void Activar_Ventana_Descripciones()
	{
		ventanaIDTiradas = 1;
	}

}




