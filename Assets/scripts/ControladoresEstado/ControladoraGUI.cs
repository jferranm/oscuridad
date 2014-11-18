using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

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

	//---- Listas de Texto para las cajas Descriptivas
	public List<Etiqueta> listaVentanaInferior = new List<Etiqueta>();
	public List<Etiqueta> listaVentanaLateral = new List<Etiqueta>();
	public string cabeceraInferior = "";
	public string cabeceraLateral = "";
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

		//Añadimos el objeto a objetos vistos del personaje
		GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddObjetoVisto (GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Objeto);
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

	public void Vaciar_Cajas_Texto()
	{
		listaVentanaInferior.Clear();
		listaVentanaLateral.Clear();
		cabeceraInferior = "";
		cabeceraLateral = "";
	}

	public void Insertar_Label_Ventana(string tipo, string texto, Color color)
	{
		if (tipo.Contains("Inferior"))   //Para la ventana de Inferior
			listaVentanaInferior.Add(new Etiqueta(texto, color, false));
		else 		//Para la ventana Lateral
			listaVentanaLateral.Add (new Etiqueta(texto, color, false));
	}

	public void Insertar_Label_Ventana(string tipo, string texto, Color color, string textoTirada, Color colorTirada)
	{
		if (tipo.Contains("Inferior"))   //Para la ventana de Inferior
			listaVentanaInferior.Add(new Etiqueta(texto, color, true, textoTirada, colorTirada));
		else 		//Para la ventana Lateral
			listaVentanaLateral.Add (new Etiqueta(texto, color, true, textoTirada, colorTirada));
	}


	public void Lanzar_Inspeccionar()
	{
		bool tiradaConExito = false;

		//Mostramos el enunciado de examinar
		Insertar_Label_Ventana("Inferior", "Examinar \"" + GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre + "\":", Color.white);

		foreach (ObjetoTiradaBase tirada in GameCenter.InstanceRef.controladoraJuego.objetoPulsado.MostrarTiradasInspeccionar()) 
		{
			//Rescatamos el valor de la habilidad
			int valorHabilidad = GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Valor_Segun_Enum (tirada.HabilidadTirada);

			//Rescatamos valor de tirada de dados
			int resultado = GameCenter.InstanceRef.controladoraJuego.Lanzar_Dados ("1D100");

			if(Lanzar_Inspeccionar_Resultado (resultado, valorHabilidad, tirada))
			{
				tiradaConExito = true;
				break;
			}
		}

		if (!tiradaConExito) 
		{
			if(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ExisteTirada(Habilidades.Fallo))
			{
				//Mostramos la descripcion anidada a la tirada de la habilidad
				Insertar_Label_Ventana("Lateral", GameCenter.InstanceRef.controladoraJuego.objetoPulsado.BuscarTirada(Habilidades.Fallo).TextoDescriptivo, Color.white);
				
				//Añadimos a la descripcion minima, la descripcion nueva de la tirada
				GameCenter.InstanceRef.controladoraJuego.Modificar_Tirada_Objeto(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.BuscarTirada(Habilidades.Fallo).TextoDescriptivo, Habilidades.Ninguna);
			}
		}

		//Desconectamos la opcion de inspeccionar la opcion de inspeccionar
		GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ObjetoInspeccionado = true;
	}

	public bool Lanzar_Inspeccionar_Resultado(int resultado, int valorHabilidad, ObjetoTiradaBase tirada)
	{
		bool mostrarResultado = true;

		if (tirada.Comprobacion) 
		{
			if(!tirada.EscenaComprobacion.Equals(Escenas.ninguna))
			{
				if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.EscenaVista(tirada.EscenaComprobacion))
					mostrarResultado = true;
				else
					mostrarResultado = false;
			}

			if(!tirada.ObjetoComprobacion.Equals(Objetos.Ninguno))
			{
				if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.ObjetoVisto(tirada.ObjetoComprobacion))
					mostrarResultado = true;
				else
					mostrarResultado = false;
			}
		}

		if (mostrarResultado) 
		{
			if (resultado < valorHabilidad) 
			{
				//Mostramos cartel de que la tirada a sido exitosa
				Insertar_Label_Ventana ("Inferior", "- Tirada " + GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Descripcion_Segun_Enum(tirada.HabilidadTirada) + "(" + valorHabilidad.ToString () + "%): " + resultado.ToString () + ".", Color.white, "Exito", Color.green);

				//Mostramos la descripcion anidada a la tirada de la habilidad
				Insertar_Label_Ventana ("Lateral", tirada.TextoDescriptivo, Color.white);

				//Añadimos a la descripcion minima, la descripcion nueva de la tirada
				GameCenter.InstanceRef.controladoraJuego.Modificar_Tirada_Objeto (tirada.TextoDescriptivo, Habilidades.Ninguna);

				//Ejecutamos Accion si la tuviese
				if(tirada.Accion)
				{
					//TODO Añadir Localizacion
				}

				//Marcamos que a sido una tirada con exito para no mostrar la tirada de fallo
				return true;
			} 
			else 
			{
				Insertar_Label_Ventana ("Inferior", "- Tirada " + GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Descripcion_Segun_Enum(tirada.HabilidadTirada) + "(" + valorHabilidad.ToString () + "%): " + resultado.ToString () + ".", Color.white, "Fracaso", Color.red);
				return false;
			}
		}

		return false;
	}

	public void Lanzar_Hablar()
	{
	}
}