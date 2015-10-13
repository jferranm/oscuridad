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
	private LanzamientoDados lanzamientoDados;

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

	public InteractuableGenerico interactuablePulsado;
	public string camaraActivar;

	#endregion

	#region METODOS INICIALIZACION
	public void Start()
	{
		pathConfig = Path.Combine(GameCenter.InstanceRef.USERPATH, "Config.xml");
		pathJugador = Path.Combine(GameCenter.InstanceRef.USERPATH, "Jugador.xml");

		jugadorActual = new JugadorBase ();
		jugadorActual.EstadoJugador = EstadosJugador.enMenus;

		escenaActual = new EscenaBase ();
		configuracionJuego = new Config(Application.systemLanguage);
		configuracionJuego.MusicaActivada = true;
		configuracionJuego.SonidoActivado = true;
		configuracionJuego.VolumenMusica = 0.5f;
		configuracionJuego.VolumenSonido = 0.5f;
		textosMenusTraduccion = new TextosMenus ();
		
		CopiarXML();
		CargarConfiguracion ();
		CargarTraduccion ();

		lanzamientoDados = new LanzamientoDados ();
	}

	public void InicializarEscena()
	{
		escenaActual = new EscenaBase ();
	}

	public void Inicializar_Partida(Personaje personaje)
	{
		Inicializar_Jugador (personaje);
		jugadorActual.AddLocalizacionDescubierta (Localizaciones.CasaFamiliarWard);
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
				jugadorActual.HabilidadesJugador.HabilidadArtisticaPintar = 55;
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

	public void Inicializar_Interactuables()
	{
		foreach (InteractuableGenerico interactuable in escenaActual.MostrarObjeto()) 
		{
			try 
			{
				GameObject.FindGameObjectWithTag(interactuable.Nombre).SetActive(interactuable.InteractuableActivo);
			}
			catch {}
		}
	}

	public void Inicializar_Interactuables_SinZoom()
	{
		foreach (InteractuableSinZoomGenerico interactuable in escenaActual.MostrarObjetoSinZoomFiltrado()) 
		{
			GameObject.Find(interactuable.Nombre).GetComponent<ObjetoAnimacion>().Ejecutar_Animacion();
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

	public void EjecutarAccion(Acciones accion)
	{
		switch (accion) 
		{
			//Se Intenta abrir la puerta del desvan y se consigue
			case Acciones.AbrirPuertaDesvanAcierto: 
				{
					GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoCerrajeria);
					GameCenter.InstanceRef.controladoraJuego.jugadorActual.BorrarAccionRealizada(Acciones.AbrirPuertaDesvanFallo);
					GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddAccionRealizada(Acciones.AbrirPuertaDesvanAcierto);
					EjecutarAccion(Acciones.AbrirDesvan);
					break;
				}

			//Se Intenta abrir la puerta del desvan pero la tirada de Cerrajeria Falla
			case Acciones.AbrirPuertaDesvanFallo: 
				{
					GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoCerrajeriaFallo);
					break;
				}

			//Se Abre la puerta del Desvan
			case Acciones.AbrirDesvan:
				{
					GameCenter.InstanceRef.controladoraJuego.escenaActual.Buscar_Camara("CamaraPuertaDesvan").EscenaNorte = "CamaraDesvan";
					break;
				}

			//Encontramos la llave en el Altar
			case Acciones.EncontrarLlaveAltar:
				{
					GameCenter.InstanceRef.controladoraGUI.Insertar_Ventana_Inferior_Texto(Interactuables.Llave, colorTexto.amarillo);
					GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddInventario(Interactuables.Llave);
					break;
				}
		}
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
		if(camaraActiva != null)
			DesactivarHijos(GameObject.Find(camaraActiva.EscenaHabilitar+"Padre"), false);
		camaraActiva = this.escenaActual.Buscar_Camara (camara);
		DesactivarHijos(GameObject.Find(camaraActiva.EscenaHabilitar+"Padre"), true);

		if(cameraActiva != null)
			cameraActiva.enabled = false;
		cameraActiva = GameObject.Find (camara).GetComponent<Camera>();
		cameraActiva.enabled = true;
		GameCenter.InstanceRef.controladoraJugador.zoomCamaraRef = cameraActiva.GetComponent<ZoomCamara> ();
	}
	
	public void Desactivar_Camaras()
	{
		foreach (CamaraEscenaBase nueva in this.escenaActual.ListaCamaras) 
		{
			GameObject.Find(nueva.Nombre).GetComponent<Camera>().enabled = false;
			if(nueva.EscenaHabilitar != string.Empty)
				try {DesactivarHijos(GameObject.Find(nueva.EscenaHabilitar+"Padre"), false);} catch {}
		}
	}
	
	public void DesactivarHijos(GameObject g, bool a) 
	{
		foreach (Transform child in g.transform) 
		{
			child.gameObject.SetActive(a);
		}
	}
	
	#endregion

	#region METODOS INTERACTUABLES SIN ZOOM

	public void EjecutarAccion(GameObject objetoSinZoom)
	{
		if (objetoSinZoom.name.Contains ("Candle")) 
		{
			ParticleSystem flame = objetoSinZoom.GetComponentInChildren<ParticleSystem>();
			if(flame.isPlaying)
			{
				GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoApagarVela);
				flame.Stop();
			}
			else
			{
				flame.Play();
				GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoEncenderVela);
			}

			Light luz = objetoSinZoom.GetComponentInChildren<Light>();
			luz.intensity = luz.intensity < 1 ? 1f : 0f;

			return;
		}

		if (objetoSinZoom.name.Contains ("Fuego")) 
		{
			ParticleSystem flame = objetoSinZoom.GetComponentInChildren<ParticleSystem>();
			AudioSource audio = objetoSinZoom.GetComponentInChildren<AudioSource>();
			if(flame.isPlaying)
			{
				flame.Stop();
				audio.Pause();
			}
			else
			{
				flame.Play();
				GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoEncenderVela);
				audio.Play();
			}

			Light luz = objetoSinZoom.GetComponentInChildren<Light>();
			luz.intensity = luz.intensity < 1.5 ? 1.5f : 0f;

			return;
		}

		if (objetoSinZoom.name.Contains ("BotellaSalon")) 
		{
			objetoSinZoom.GetComponent<ObjetoAnimacion>().Ejecutar_Animacion_Con_Sonido();
			return;
		}

		if (objetoSinZoom.name.Contains ("LibroSalon") && escenaActual.Buscar_InteractuableSinZoom(objetoSinZoom.name).EjecutarAnimacion) 
		{
			objetoSinZoom.GetComponent<ObjetoAnimacion>().Ejecutar_Animacion_Con_Sonido();
			escenaActual.Buscar_InteractuableSinZoom(objetoSinZoom.name).EjecutarAnimacion = false;
			return;
		}
	}

	#endregion

	#region METODOS VARIOS
	public int Lanzar_Dados(string expresion)
	{
		return lanzamientoDados.Lanzar(expresion);
	}

	public void Modificar_Tirada_Objeto(string nuevaDescripcion, Habilidades habilidad)
	{
		interactuablePulsado.BuscarTirada(habilidad).TextoDescriptivo += Environment.NewLine + Environment.NewLine + nuevaDescripcion;
	}

	public string Devolver_Descripcion_Objeto_Segun_Enum(Interactuables objeto)
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
