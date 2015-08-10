using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

[System.Serializable]
public class ControladoraJuego
{
	#region VARIABLES
	private string pathConfig;
	private string pathJugador;
	private string pathIdioma;

	public JugadorBase jugadorActual;
	public EscenaBase escenaActual;
	public CamaraEscenaBase camaraActiva;
	public Camera cameraActiva;
	public Config configuracionJuego;
	public TextosMenus textosMenusTraduccion;
	public Idioma idiomaJuego
	{
		set 
		{ 
			configuracionJuego.IdiomaJuego = value;
			pathIdioma = GameCenter.InstanceRef.USERPATH + "/" + configuracionJuego.IdiomaJuego.ToString ();
			CargarTraduccion ();
		}
	}

	public ObjetoBase objetoPulsado;
	public PersonajeBase personajePulsado;

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

	#endregion

	#region METODOS INICIALIZACION
	public void Inicializar()
	{
		pathConfig = Path.Combine(GameCenter.InstanceRef.USERPATH, "Config.xml");
		pathJugador = Path.Combine(GameCenter.InstanceRef.USERPATH, "Jugador.xml");

		jugadorActual = new JugadorBase ();
		jugadorActual.EstadoJugador = EstadosJugador.enMenus;

		escenaActual = new EscenaBase ();
		configuracionJuego = new Config(Application.systemLanguage);
		textosMenusTraduccion = new TextosMenus ();
		
		CopiarXML();
		CargarConfiguracion ();
	}

	public void InicializarEscena()
	{
		escenaActual = new EscenaBase ();
	}

	public void Inicializar_Partida(Personaje personaje)
	{
		Inicializar_Jugador (personaje);
		GrabarJugador ();
	}

	public void Inicializar_Jugador(Personaje personaje)
	{
		switch (personaje) 
		{
			case Personaje.MarlaGibbs:
			{
				jugadorActual.DatosPersonalesJugador.Nombre = "Marla Gibbs";
				jugadorActual.DatosPersonalesJugador.Profesion = "Periodista";
				jugadorActual.DatosPersonalesJugador.Edad = 32;

				jugadorActual.CaracteristicasJugador.Fuerza = 8;
				jugadorActual.CaracteristicasJugador.Constitucion = 11;
				jugadorActual.CaracteristicasJugador.Tamanyo = 9;
				jugadorActual.CaracteristicasJugador.Inteligencia = 16;
				jugadorActual.CaracteristicasJugador.Poder = 13;
				jugadorActual.CaracteristicasJugador.Destreza = 12;
				jugadorActual.CaracteristicasJugador.Aparicencia = 14;
				jugadorActual.CaracteristicasJugador.Cordura = 65;
				jugadorActual.CaracteristicasJugador.Educacion = 17;
				jugadorActual.CaracteristicasJugador.Idea = 85;
				jugadorActual.CaracteristicasJugador.Suerte = 65;
				jugadorActual.CaracteristicasJugador.Conocimiento = 80;
				jugadorActual.CaracteristicasJugador.Vida = 10;

				jugadorActual.HabilidadesJugador.Antropologia = 40;				
				jugadorActual.HabilidadesJugador.BuscarLibros = 75;
				jugadorActual.HabilidadesJugador.CienciasOcultas = 40;
				jugadorActual.HabilidadesJugador.Conducir = 35;
				jugadorActual.HabilidadesJugador.Charlataneria = 45;
				jugadorActual.HabilidadesJugador.Credito = 40;
				jugadorActual.HabilidadesJugador.Derecho = 45;
				jugadorActual.HabilidadesJugador.Descubrir = 25;
				jugadorActual.HabilidadesJugador.Discrecion = 35;
				jugadorActual.HabilidadesJugador.Escuchar = 25;
				jugadorActual.HabilidadesJugador.Esquivar = 24;
				jugadorActual.HabilidadesJugador.Historia = 65;
				jugadorActual.HabilidadesJugador.LenguaPropia = 80;
				jugadorActual.HabilidadesJugador.OtraLenguaLatin = 40;
				jugadorActual.HabilidadesJugador.Mecanica = 20;
				jugadorActual.HabilidadesJugador.Nadar = 40;
				jugadorActual.HabilidadesJugador.Ocultar = 25;
				jugadorActual.HabilidadesJugador.Ocultarse = 35;
				jugadorActual.HabilidadesJugador.OtraLenguaFrances = 55;
				jugadorActual.HabilidadesJugador.Persuasion = 25;
				jugadorActual.HabilidadesJugador.PrimerosAuxilios = 30;
				jugadorActual.HabilidadesJugador.Psicologia = 45;
				jugadorActual.HabilidadesJugador.Saltar = 25;

				jugadorActual.ArmasJugador.ArmaCorta = 40;
				jugadorActual.ArmasJugador.Escopeta = 30;
				jugadorActual.ArmasJugador.Fusil = 25;

				break;
			}

			case Personaje.RobertDuncan:
			{
				jugadorActual.DatosPersonalesJugador.Nombre = "Robert Duncan";
				jugadorActual.DatosPersonalesJugador.Profesion = "Detective";
				jugadorActual.DatosPersonalesJugador.Edad = 36;

				jugadorActual.CaracteristicasJugador.Fuerza = 15;
				jugadorActual.CaracteristicasJugador.Constitucion = 16;
				jugadorActual.CaracteristicasJugador.Tamanyo = 12;
				jugadorActual.CaracteristicasJugador.Inteligencia = 11;
				jugadorActual.CaracteristicasJugador.Poder = 12;
				jugadorActual.CaracteristicasJugador.Destreza = 14;
				jugadorActual.CaracteristicasJugador.Aparicencia = 12;
				jugadorActual.CaracteristicasJugador.Cordura = 60;
				jugadorActual.CaracteristicasJugador.Educacion = 14;
				jugadorActual.CaracteristicasJugador.Idea = 55;
				jugadorActual.CaracteristicasJugador.Suerte = 60;
				jugadorActual.CaracteristicasJugador.Conocimiento = 70;
				jugadorActual.CaracteristicasJugador.Vida = 14;

				jugadorActual.HabilidadesJugador.BuscarLibros = 25;
				jugadorActual.HabilidadesJugador.Cerrajeria = 40;
				jugadorActual.HabilidadesJugador.Conducir = 40;
				jugadorActual.HabilidadesJugador.Charlataneria = 55;
				jugadorActual.HabilidadesJugador.Credito = 15;
				jugadorActual.HabilidadesJugador.Derecho  = 55;
				jugadorActual.HabilidadesJugador.Descubrir = 25;
				jugadorActual.HabilidadesJugador.Discrecion = 40;
				jugadorActual.HabilidadesJugador.Disfrazarse = 20;
				jugadorActual.HabilidadesJugador.Escuchar = 25;
				jugadorActual.HabilidadesJugador.Esquivar = 25;
				jugadorActual.HabilidadesJugador.Fotografia = 45;
				jugadorActual.HabilidadesJugador.Historia = 20;
				jugadorActual.HabilidadesJugador.LenguaPropia = 55;
				jugadorActual.HabilidadesJugador.OtraLenguaLatin = 40;
				jugadorActual.HabilidadesJugador.Mecanica = 20;
				jugadorActual.HabilidadesJugador.Nadar = 25;
				jugadorActual.HabilidadesJugador.Ocultar = 25;
				jugadorActual.HabilidadesJugador.Ocultarse = 40;
				jugadorActual.HabilidadesJugador.Persuasion = 35;
				jugadorActual.HabilidadesJugador.PrimerosAuxilios = 30;
				jugadorActual.HabilidadesJugador.Psicologia = 45;
				jugadorActual.HabilidadesJugador.Regatear = 75;
				jugadorActual.HabilidadesJugador.Saltar = 25;

				jugadorActual.ArmasJugador.ArmaCorta = 65;				
				jugadorActual.ArmasJugador.Escopeta = 30;
				jugadorActual.ArmasJugador.Fusil = 25;

				break;
			}

			case Personaje.WarrenBedford:
			{
				jugadorActual.DatosPersonalesJugador.Nombre = "Warren Bedford";
				jugadorActual.DatosPersonalesJugador.Profesion = "Profesor de Historia";
				jugadorActual.DatosPersonalesJugador.Edad = 56;

				jugadorActual.CaracteristicasJugador.Fuerza = 10;
				jugadorActual.CaracteristicasJugador.Constitucion = 9;
				jugadorActual.CaracteristicasJugador.Tamanyo = 10;
				jugadorActual.CaracteristicasJugador.Inteligencia = 17;
				jugadorActual.CaracteristicasJugador.Poder = 16;
				jugadorActual.CaracteristicasJugador.Destreza = 7;
				jugadorActual.CaracteristicasJugador.Aparicencia =9;				
				jugadorActual.CaracteristicasJugador.Cordura = 80;
				jugadorActual.CaracteristicasJugador.Educacion = 23;
				jugadorActual.CaracteristicasJugador.Idea = 85;
				jugadorActual.CaracteristicasJugador.Suerte = 80;
				jugadorActual.CaracteristicasJugador.Conocimiento = 99;
				jugadorActual.CaracteristicasJugador.Vida = 10;

				jugadorActual.HabilidadesJugador.Antropologia = 25;
				jugadorActual.HabilidadesJugador.Arqueologia = 50;
				jugadorActual.HabilidadesJugador.Astronomia = 20;
				jugadorActual.HabilidadesJugador.BuscarLibros = 75;
				jugadorActual.HabilidadesJugador.CienciasOcultas = 55;
				jugadorActual.HabilidadesJugador.Conducir = 50;
				jugadorActual.HabilidadesJugador.Credito = 75;
				jugadorActual.HabilidadesJugador.Derecho = 30;
				jugadorActual.HabilidadesJugador.Descubrir = 25;
				jugadorActual.HabilidadesJugador.Equitacion = 30;
				jugadorActual.HabilidadesJugador.Discrecion = 40;
				jugadorActual.HabilidadesJugador.Disfrazarse = 20;
				jugadorActual.HabilidadesJugador.Escuchar = 25;
				jugadorActual.HabilidadesJugador.HabilidadArtistica = 55;
				jugadorActual.HabilidadesJugador.Historia = 85;
				jugadorActual.HabilidadesJugador.HistoriaNatural = 35;
				jugadorActual.HabilidadesJugador.LenguaPropia = 85;
				jugadorActual.HabilidadesJugador.Nadar = 25;
				jugadorActual.HabilidadesJugador.Ocultar = 25;
				jugadorActual.HabilidadesJugador.OtraLenguaFrances = 45; 
				jugadorActual.HabilidadesJugador.OtraLenguaAleman = 30; 
				jugadorActual.HabilidadesJugador.OtraLenguaItaliano = 25; 
				jugadorActual.HabilidadesJugador.OtraLenguaLatin = 55; 
				jugadorActual.HabilidadesJugador.Persuasion = 15;
				jugadorActual.HabilidadesJugador.PrimerosAuxilios = 30;
				jugadorActual.HabilidadesJugador.Psicologia = 55;
				jugadorActual.HabilidadesJugador.Saltar = 25;

				jugadorActual.ArmasJugador.ArmaCorta = 30;				
				jugadorActual.ArmasJugador.Escopeta = 30;
				jugadorActual.ArmasJugador.Fusil = 40;

				break;
			}
		}
	}

	public void Inicializar_Objetos()
	{
		foreach (ObjetoBase objeto in GameCenter.InstanceRef.controladoraJuego.escenaActual.MostrarObjeto()) 
		{
			try 
			{
				GameObject.FindGameObjectWithTag(objeto.Nombre).SetActive(objeto.ObjetoActivo);
			}
			catch {}
		}
	}
	#endregion

	#region METODOS IO
	public void CopiarXML()
	{
		string destino = null;
		TextAsset origen = null;

		//Copiamos escenarios
		foreach(string idiomaXml in Enum.GetNames(typeof(Idioma)))
		{
			if(!Directory.Exists(GameCenter.InstanceRef.USERPATH + "/" + idiomaXml))
				Crear_Directorio(GameCenter.InstanceRef.USERPATH + "/" + idiomaXml);

			foreach (string escenario in Enum.GetNames(typeof(XmlDatosEscenas)))
			{
				origen = (TextAsset)Resources.Load("xml/Escenarios/"+ idiomaXml.ToUpper() + "/" + escenario, typeof(TextAsset));
				destino = GameCenter.InstanceRef.USERPATH + "/" + idiomaXml + "/" + escenario + ".xml";

				if (!File.Exists (destino))
					Crear_Fichero (origen, destino);
			}

			origen = (TextAsset)Resources.Load("xml/Escenarios/"+ idiomaXml.ToUpper() + "/TextosMenus", typeof(TextAsset));
			destino = GameCenter.InstanceRef.USERPATH + "/" + idiomaXml + "/TextosMenus.xml";

			if (!File.Exists (destino))
				Crear_Fichero (origen, destino);
		}
	}

	public void Crear_Fichero(TextAsset nuevoOrigen, string nuevoDestino)
	{
		try 
		{
			StreamWriter sw = new StreamWriter(nuevoDestino, false);
			sw.Write(nuevoOrigen.text);
			sw.Close();
		}
		catch (IOException ex) 
		{ 
			//No se a podido crear el archivo	
			Debug.LogError(ex.Message);
		}
	}

	public void Crear_Directorio(string nuevoDestino)
	{
		try
		{
			Directory.CreateDirectory(nuevoDestino);
		}
		catch (IOException ex) 
		{
			Debug.LogError(ex.Message);
		}
	}
	#endregion

	#region METODOS CONFIGURACION
	public void CargarConfiguracion()
	{
		cXML nuevoXML = new cXML ();

		if (File.Exists (pathConfig)) 
		{
			configuracionJuego = nuevoXML.Cargar_Clase_Serializable<Config> (pathConfig, configuracionJuego);
			Modificar_Configuracion_Audio(configuracionJuego.VolumenSonido, configuracionJuego.VolumenMusica, configuracionJuego.SonidoActivado, configuracionJuego.MusicaActivada);
		}
		else 
			GrabarConfiguracion();

		idiomaJuego = configuracionJuego.IdiomaJuego;

		nuevoXML.Cerrar ();
	}

	public void Modificar_Configuracion_Audio(float volumenAudio, float volumenMusica, bool activarAudio, bool activarMusica)
	{
		OpcionesCanvasMenuPrincipal opciones = GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ();
		opciones.sliderSonido.value = volumenAudio;
		opciones.sliderMusica.value = volumenMusica;
		opciones.toggleMusica.isOn = activarMusica;
		opciones.toggleSonido.isOn = activarAudio;
	}

	public void GrabarConfiguracion()
	{
		cXML nuevoXML = new cXML ();
		nuevoXML.Guardar_Clase_Serializable<Config> (pathConfig, configuracionJuego);
		nuevoXML.Cerrar ();
	}
	#endregion

	#region METODOS TRADUCCION
	public void CargarTraduccion()
	{
		cXML nuevoXML = new cXML ();
		string pathTextos = Path.Combine (pathIdioma, "TextosMenus.xml");
		textosMenusTraduccion = nuevoXML.Cargar_Clase_Serializable<TextosMenus> (pathTextos, textosMenusTraduccion);
		nuevoXML.Cerrar ();
	}

	public string Traduccion_Coger_Objeto(string nombreObjeto)
	{
		return nombreObjeto + "Ahora esta en el Inventario";
	}
	#endregion

	#region METODOS JUGADOR
	public void CargarJugador()
	{
		cXML nuevoXML = new cXML ();
		jugadorActual = nuevoXML.Cargar_Clase_Serializable<JugadorBase> (pathJugador, jugadorActual);
		nuevoXML.Cerrar ();
	}

	public void GrabarJugador()
	{
		cXML nuevoXML = new cXML ();
		nuevoXML.Guardar_Clase_Serializable<JugadorBase> (pathJugador, jugadorActual);
		nuevoXML.Cerrar ();
	}
	#endregion

	#region METODOS ESCENA
	public EscenaBase Cargar_Escena(Escenas escena)
	{
		cXML nuevoxml = new cXML ();
		escenaActual = nuevoxml.Cargar_Clase_Serializable<EscenaBase> (Path.Combine (pathIdioma, escena.ToString()+".xml"), GameCenter.InstanceRef.controladoraJuego.escenaActual);
		nuevoxml.Cerrar ();

		return escenaActual;
	}

	public void Guardar_Escena(Escenas escena)
	{
		cXML nuevoxml = new cXML ();
		nuevoxml.Guardar_Clase_Serializable<EscenaBase> (Path.Combine (pathIdioma, escena.ToString () + ".xml"), GameCenter.InstanceRef.controladoraJuego.escenaActual);
		nuevoxml.Cerrar ();
	}
	#endregion

	#region METODOS CAMARAS
	public void Cambiar_Camara(string camara)
	{
		float sourceBSOTime = 0;
		AudioClip sourceBSOAudio = null;
		
		if(this.camaraActiva != null)
		{
			sourceBSOTime = GameCenter.InstanceRef.controladoraSonidos.emisorBSO.time;
			sourceBSOAudio = GameCenter.InstanceRef.controladoraSonidos.emisorBSO.clip;
			GameCenter.InstanceRef.controladoraSonidos.emisorBSO.Stop();
			cameraActiva.enabled = false;
			if(!string.IsNullOrEmpty(camaraActiva.EscenaHabilitar))
				DesactivarHijos(GameObject.Find(camaraActiva.EscenaHabilitar), false);
		}
		
		cameraActiva = GameObject.Find (camara).GetComponent<Camera>();
		cameraActiva.enabled = true;
		GameCenter.InstanceRef.controladoraJugador.zoomCamaraRef = cameraActiva.GetComponent<ZoomCamara> ();
		camaraActiva = this.escenaActual.Buscar_Camara (camara);
		
		if (sourceBSOAudio != null) 
		{
			GameCenter.InstanceRef.controladoraSonidos.emisorBSO.clip = sourceBSOAudio;
			GameCenter.InstanceRef.controladoraSonidos.emisorBSO.time = sourceBSOTime;
			GameCenter.InstanceRef.controladoraSonidos.emisorBSO.Play();
			if(!string.IsNullOrEmpty(camaraActiva.EscenaHabilitar))
			{
				DesactivarHijos(GameObject.Find(camaraActiva.EscenaHabilitar+"Padre"), true);
				Inicializar_Objetos();
			}
		}

		GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddEscenaVisitada(cameraActiva.GetComponent<ZoomCamara>().escenaVistaCamara);;
	}
	
	public void Desactivar_Camaras()
	{
		foreach (CamaraEscenaBase nueva in this.escenaActual.ListaCamaras) 
		{
			GameObject.Find(nueva.Nombre).GetComponent<Camera>().enabled = false;
		}
	}
	
	public void DesactivarHijos(GameObject g, bool a) 
	{
		g.SetActive (a);
		
		foreach (Transform child in g.transform) {
			DesactivarHijos(child.gameObject, a);
		}
	}
	#endregion

	#region METODOS VARIOS
	public int Lanzar_Dados(string expresion)
	{
		//LanzamientoDados nuevoLanzamiento = new LanzamientoDados();

		//return nuevoLanzamiento.Lanzar(expresion);

		return UnityEngine.Random.Range (0, 101);
	}

	public void Modificar_Tirada_Objeto(string nuevaDescripcion, Habilidades habilidad)
	{
		ObjetoTiradaBase aux = GameCenter.InstanceRef.controladoraJuego.objetoPulsado.BuscarTirada(habilidad);
		aux.TextoDescriptivo +=  Environment.NewLine + Environment.NewLine + nuevaDescripcion;
	}

	public string Devolver_Descripcion_Objeto_Segun_Enum(Objetos objeto)
	{
		string aux = "";

		foreach (char c in objeto.ToString()) 
		{
			if(Char.IsUpper(c))
				aux += " "+c;
			else
				aux += c;
		}

		return aux;
	}

	public string Devolver_Descripcion_Localizacion_Segun_Enum(Localizaciones objeto)
	{
		string aux = "";

		for (byte cont = 0; cont < objeto.ToString().Length; cont++) 
		{
				if(Char.IsUpper(objeto.ToString()[cont]))
				{
					if(cont!=0)
						aux += " "+objeto.ToString()[cont];
					else
						aux += objeto.ToString()[cont];
				}
				else
					aux += objeto.ToString()[cont];
		}
		
		return aux;
	}

	public float Devolver_Tamanyo_Cadena(string texto, int sizeFuente)
	{
		GUIStyle miEstilo = new GUIStyle ();
		miEstilo.fontSize = sizeFuente;
		return miEstilo.CalcSize(new GUIContent(texto)).x;
	}

	#endregion
}
