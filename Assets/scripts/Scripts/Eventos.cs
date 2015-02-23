using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using Oscuridad.Enumeraciones;

public class Eventos : MonoBehaviour 
{
	public void BotonComenzar()
	{
		string archivoJugador = Path.Combine (GameCenter.InstanceRef.USERPATH, "Jugador.xml");
		OpcionesCanvasMenuPrincipal opciones = GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ();

		if (File.Exists (archivoJugador)) 
		{
			//TODO: empezar por la ultima pantalla
			GameCenter.InstanceRef.controladoraJuego.CargarJugador ();
			GameCenter.InstanceRef.controladoraEscenas.IrEscenaWardExterior ();
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
		Application.Quit ();
	}

	public void ActivarDesactivarMusica(bool seleccion)
	{
		if (seleccion) 
		{
			GameCenter.InstanceRef.controladoraSonidos.volumenMusica = 1f;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderMusica.enabled = true;
		} 
		else
		{
			GameCenter.InstanceRef.controladoraSonidos.volumenMusica = 0;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderMusica.enabled = false;
		}
	}

	public void ActivarDesactivarSonido(bool seleccion)
	{
		if (seleccion) 
		{
			GameCenter.InstanceRef.controladoraSonidos.volumenSonido = 1f;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderSonido.enabled = true;
		} 
		else
		{
			GameCenter.InstanceRef.controladoraSonidos.volumenSonido = 0;
			GameCenter.InstanceRef.CanvasMenuPrincipal.GetComponent<OpcionesCanvasMenuPrincipal> ().sliderSonido.enabled = false;
		}
	}
	
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
				GameCenter.InstanceRef.controladoraEscenas.CambiarSceneSegunEnum(escenaSeleccionada);
			}
			else
			{
				GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddEscenaVisitada (GameCenter.InstanceRef.controladoraJuego.camaraActiva.Escena);
				GameCenter.InstanceRef.controladoraJuego.Cambiar_Camara(camaraSeleccionada);
				GameCenter.InstanceRef.controladoraGUI.panelDirecciones.GetComponent<PanelDireccionesOpciones>().Reiniciar_Direcciones();
				GameCenter.InstanceRef.controladoraGUI.textoInferior.GetComponent<TextoInferiorOpciones>().Reiniciar_Texto();
			}
		}
	}

	public void BotonObjeto(GameObject botonObjeto)
	{
		Color rojo = new Color (255,0,0);
		Image imagenBoton = botonObjeto.GetComponent<Image> ();

		if (!imagenBoton.color.Equals (rojo)) 
		{
			if(botonObjeto.name.Contains("Volver"))
				GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enZoomOut;

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
		GameObject.FindGameObjectWithTag(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Nombre).SetActive(false);
		
		//Deshabilitamos los botones
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Coger");
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Hablar");
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Inspeccionar");

		//Insertar objeto en el inventario del jugador
		GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddInventario ((Objetos)Enum.Parse(typeof(Objetos), GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Nombre));
		
		//Cambiamos a false el valor de objetoActivo a false
		GameCenter.InstanceRef.controladoraJuego.objetoPulsado.ObjetoActivo = false;
		
		//Le indicamos a la caja de texto que esta en el inventario
		GameCenter.InstanceRef.controladoraGUI.Insertar_Ventana_Inferior_Texto(GameCenter.InstanceRef.controladoraJuego.Traduccion_Coger_Objeto(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.DescripcionNombre), Color.yellow);
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
		GameCenter.InstanceRef.controladoraGUI.panelObjetos.GetComponent<PanelObjetosOpciones> ().Desactivar ("Hablar");

		GameCenter.InstanceRef.controladoraGUI.Lanzar_Hablar ();
	}

	public void BotonPregunta()
	{
	}

	public void CajaTexto_OnChange(Vector2 nuevaPosicion)
	{
		Debug.Log (nuevaPosicion);
	}
}
