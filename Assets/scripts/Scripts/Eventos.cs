using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using Oscuridad.Enumeraciones;

public class Eventos : MonoBehaviour 
{
	#region MENU PRINCIPAL

	public bool toggleMusica
	{
		set{ ActivarDesactivarMusica(value);}
	}

	public float volumenMusica
	{
		set { AjustarVolumenMusica(value); }
	}

	public bool toggleSonido
	{
		set{ ActivarDesactivarSonido(value);}
	}

	public float volumenSonido
	{
		set { AjustarVolumenSonido(value); }
	}

	public void BotonComenzar()
	{
		string archivoJugador = Path.Combine (GameCenter.InstanceRef.USERPATH, "Jugador.xml");
		OpcionesCanvasMenuPrincipal opciones = GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ();

		if (File.Exists (archivoJugador)) 
		{
			GameCenter.InstanceRef.controladoraJuego.CargarJugador ();
			GameCenter.InstanceRef.controladoraJuego.camaraActivar = GameCenter.InstanceRef.controladoraJuego.configuracionJuego.UltimaCamaraVisitada;
			GameCenter.InstanceRef.controladoraEscenas.CambiarSceneSegunEnum(GameCenter.InstanceRef.controladoraJuego.configuracionJuego.UltimaEscenaVisitada);
		} 
		else 
		{
			opciones.escena0.SetActive (false);
			opciones.escena2.SetActive (false);
			opciones.escena1.SetActive (true);
		}
	}

	public void BotonOpciones()
	{
		OpcionesCanvasMenuPrincipal opciones = GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ();

		opciones.escena0.SetActive (false);
		opciones.escena1.SetActive (false);
		opciones.escena2.SetActive (true);
	}

	public void BotonPartidaNueva()
	{
		OpcionesCanvasMenuPrincipal opciones = GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ();

		opciones.escena0.SetActive (false);
		opciones.escena1.SetActive (true);
		opciones.escena2.SetActive (false);
	}

	public void BotonPersonaje(string nombrePersonaje)
	{
		switch (nombrePersonaje) 
		{
			case "Marla":
				GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.MarlaGibbs);
				break;

			case "Robert":
				GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.RobertDuncan);
				break;

			case "Warren":
				GameCenter.InstanceRef.controladoraJuego.Inicializar_Partida(Personaje.WarrenBedford);
				break;
		}

		GameCenter.InstanceRef.controladoraEscenas.IrEscenaWardExterior ();
	}

	public void BotonSalir()
	{
		GameCenter.InstanceRef.Salir ();
	}

	public void BotonIdioma()
	{
		switch (GameCenter.InstanceRef.controladoraJuego.configuracionJuego.IdiomaJuego) 
		{
			case Idioma.spa : 	GameCenter.InstanceRef.controladoraJuego.idiomaJuego = Idioma.eng;
								break;
			case Idioma.eng : 	GameCenter.InstanceRef.controladoraJuego.idiomaJuego = Idioma.fr;
								break;
			case Idioma.fr : 	GameCenter.InstanceRef.controladoraJuego.idiomaJuego = Idioma.spa;
								break;
		}

		GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().TraduccionMenu ();
	}

	public void ActivarDesactivarMusica(bool eleccion)
	{
		if (eleccion) 
		{
			volumenMusica = 1;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderMusica.value = 1;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderMusica.enabled = true;
		} 
		else 
		{
			volumenMusica = 0;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderMusica.value = 0;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderMusica.enabled = false;
		}
	}

	public void AjustarVolumenMusica(float nuevoVolumen)
	{
		GameCenter.InstanceRef.controladoraSonidos.volumenMusica = nuevoVolumen;
	}

	public void ActivarDesactivarSonido(bool eleccion)
	{
		if (eleccion) 
		{
			volumenSonido = 1;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderSonido.value = 1;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderSonido.enabled = true;
		} 
		else 
		{
			volumenSonido = 0;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderSonido.value = 0;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderSonido.enabled = false;
		}
	}

	public void AjustarVolumenSonido(float nuevoVolumen)
	{
		GameCenter.InstanceRef.controladoraSonidos.volumenSonido = nuevoVolumen;
	}

	#endregion

	#region PANEL DIRECCIONES

	public void BotonDireccion(GameObject botonDireccion)
	{
		Color rojo = new Color (255,0,0);
		Image imagenBoton = botonDireccion.GetComponent<Image> ();
		bool tipoDireccion = false; //true Escena - false Camara

		Escenas escenaSeleccionada = Escenas.ninguna;
		string camaraSeleccionada = "";
		
		if (!imagenBoton.color.Equals (rojo)) 
		{
			if(botonDireccion.name.Contains("Arriba"))
			{
				if(GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaNorte.Contains("Escena"))
				{
					tipoDireccion = true;
					escenaSeleccionada = (Escenas)Enum.Parse (typeof(Escenas), GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaNorte);
				}
				else
				{
					tipoDireccion = false;
					camaraSeleccionada = GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaNorte;
				}
			}
			  
			if(botonDireccion.name.Contains("Abajo"))
			{
				if(GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaSur.Contains("Escena"))
				{
					tipoDireccion = true;
					escenaSeleccionada = (Escenas)Enum.Parse (typeof(Escenas), GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaSur);
				}
				else
				{
					tipoDireccion = false;
					camaraSeleccionada = GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaSur;
				}
			}

			if(botonDireccion.name.Contains("Izquierda"))
			{
				if(GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaEste.Contains("Escena"))
				{
					tipoDireccion = true;
					escenaSeleccionada = (Escenas)Enum.Parse (typeof(Escenas), GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaEste);
				}
				else
				{
					tipoDireccion = false;
					camaraSeleccionada = GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaEste;
				}
			}

			if(botonDireccion.name.Contains("Derecha"))
			{
				if(GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaOeste.Contains("Escena"))
				{
					tipoDireccion = true;
					escenaSeleccionada = (Escenas)Enum.Parse (typeof(Escenas), GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaOeste);
				}
				else
				{
					tipoDireccion = false;
					camaraSeleccionada = GameCenter.InstanceRef.controladoraJuego.camaraActiva.EscenaOeste;
				}
			}

			if(tipoDireccion)
			{
				GameCenter.InstanceRef.controladoraGUI.DesactivarGUI ();
				GameCenter.InstanceRef.controladoraJuego.Guardar_Escena ((Escenas)Enum.Parse (typeof(Escenas), Application.loadedLevelName));
				GameCenter.InstanceRef.controladoraJuego.GrabarConfiguracion();
				GameCenter.InstanceRef.controladoraJuego.GrabarJugador();
				GameCenter.InstanceRef.controladoraEscenas.CambiarSceneSegunEnum(escenaSeleccionada);
				GameCenter.InstanceRef.controladoraJuego.camaraActiva = null;
				GameCenter.InstanceRef.controladoraJuego.cameraActiva = null;
				GameCenter.InstanceRef.controladoraSonidos.emisorBSO.Pause();
			}
			else
			{
				GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddEscenaVisitada (GameCenter.InstanceRef.controladoraJuego.camaraActiva.Escena);
				GameCenter.InstanceRef.controladoraJuego.Cambiar_Camara(camaraSeleccionada);
				GameCenter.InstanceRef.controladoraJuego.configuracionJuego.UltimaCamaraVisitada = GameCenter.InstanceRef.controladoraJuego.camaraActiva.Nombre;
				GameCenter.InstanceRef.controladoraGUI.panelDirecciones.GetComponent<PanelDireccionesOpciones>().Reiniciar_Direcciones();
				GameCenter.InstanceRef.controladoraGUI.textoInferior.GetComponent<TextoInferiorOpciones>().Reiniciar_Texto();
			}
		}
	}

	public void BotonOpcionesDireccion()
	{
		GameCenter.InstanceRef.CanvasMenuOpciones.SetActive (true);
		GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enMenus;
	}

	#endregion

	#region PANEL OBJETOS

	public void BotonObjeto(GameObject botonObjeto)
	{
		Color rojo = new Color (255,0,0);
		Image imagenBoton = botonObjeto.GetComponent<Image> ();

		if (!imagenBoton.color.Equals (rojo)) 
		{
			if(botonObjeto.name.Contains("Volver"))
				Volver_Vista();

			if(botonObjeto.name.Contains("Coger"))
				Coger_Objeto();

			if(botonObjeto.name.Contains("Inspeccionar"))
				Inspeccionar_Objeto();

			if(botonObjeto.name.Contains("Hablar"))
				Hablar_Personaje();
		}
	}

	private void Coger_Objeto()
	{
		//Desactivamos el objeto
		GameObject.FindGameObjectWithTag(GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Nombre).SetActive(false);
		
		//Deshabilitamos los botones
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Coger");
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Hablar");
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Inspeccionar");

		//Insertar objeto en el inventario del jugador
		GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddInventario ((Interactuables)Enum.Parse(typeof(Interactuables), GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Nombre));
		
		//Cambiamos a false el valor de objetoActivo a false
		GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.InteractuableActivo = false;
		
		//Le indicamos a la caja de texto que esta en el inventario
		GameCenter.InstanceRef.controladoraGUI.Insertar_Ventana_Inferior_Texto(GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Nombre, colorTexto.amarillo);
	}

	private void Inspeccionar_Objeto()
	{
		//Deshabilitamos el boton Inspeccionar para no poder pulsar mas veces sobre el mientras se esta inspeccionando
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Inspeccionar");

		GameCenter.InstanceRef.controladoraGUI.Lanzar_Inspeccionar();
	}

	private void Hablar_Personaje()
	{
		//Deshabilitamos el boton Hablar para no poder pulsar mas veces sobre el mientras se esta hablando
		GameCenter.InstanceRef.controladoraGUI.panelObjetosOpciones.Desactivar ("Hablar");
		GameCenter.InstanceRef.controladoraGUI.panelObjetosOpciones.Desactivar ("Inspeccionar");

		GameCenter.InstanceRef.controladoraGUI.Lanzar_Hablar ();
	}

	private void Volver_Vista()
	{
		GameCenter.InstanceRef.controladoraGUI.Vaciar_Texto_Lateral();
		GameCenter.InstanceRef.controladoraGUI.Vaciar_Panel_Preguntas();
		GameCenter.InstanceRef.controladoraGUI.textoInferiorOpciones.gameObject.SetActive (true);
		GameCenter.InstanceRef.controladoraGUI.panelPreguntasOpciones.gameObject.SetActive (false);
		GameCenter.InstanceRef.controladoraGUI.panelObjetosOpciones.Normalizar_Botones();

		GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enZoomOut;
	}

	public void BotonPregunta(GameObject respuesta)
	{
		GameCenter.InstanceRef.controladoraGUI.Reestructurar_Respuestas(int.Parse (respuesta.name), false);
	}

	#endregion

	#region PANEL LIBRO

	public void BotonLibro()
	{
		GameCenter.InstanceRef.controladoraGUI.panelLibro.SetActive (!GameCenter.InstanceRef.controladoraGUI.panelLibro.activeSelf);
	}

	#endregion

	#region PANEL MENU OPCIONES

	public void OpcionesSalir()
	{
		GameCenter.InstanceRef.Salir ();
	}

	#endregion
}
