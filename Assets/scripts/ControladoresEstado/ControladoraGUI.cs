using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

[System.Serializable]
public class ControladoraGUI
{
	#region VARIABLES PUBLICAS
	public RespuestaBase nuevaRespuesta = new RespuestaBase();

	public GameObject imagenCargando;
	public GameObject panelLateral;
	public GameObject panelInferior;
	public GameObject botonDiario;
	public GameObject panelDirecciones;
	public GameObject panelObjetos;

	public Text textoLateral;
	public TextoLateralOpciones textoLateralOpciones;
	public Text textoInferior;
	public TextoInferiorOpciones textoInferiorOpciones;
	public PanelPreguntasOpciones panelPreguntasOpciones;
	public ListaPreguntas listaPreguntas;
	public PanelObjetosOpciones panelObjetosOpciones;

	public string textoDescriptivo;

	#endregion

	#region CONSTRUCTORES
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

	#endregion

	#region ESTADOS JUGADOR

	public void CambioEnEstado()
	{
		switch (GameCenter.InstanceRef.controladoraJugador.EstadoJugador) 
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

	private void JugadorEnEspera()
	{
		Activar_Opciones_Basicas();
	}
	
	private void JugadorEnZoomIn()
	{
		//Desactivamos Ventanas
		DesactivarGUI ();
	}
	
	private void JugadorEnZoomOut()
	{
		//Desactivamos Ventanas
		DesactivarGUI ();
	}
	
	private void JugadorEnMenu()
	{
		
	}
	
	private void JugadorEnZoomEspera()
	{
		//Activamos el Menu
		Activar_Opciones_Basicas();
		panelLateral.SetActive (true);
		panelObjetos.SetActive (true);
		panelDirecciones.SetActive (false);

		//Añadimos el objeto a objetos vistos del personaje
		if(GameCenter.InstanceRef.controladoraJuego.objetoPulsado != null)
			GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddObjetoVisto (GameCenter.InstanceRef.controladoraJuego.objetoPulsado.Objeto);
	}

	public void Activar_Opciones_Basicas ()
	{
		botonDiario.SetActive (true);
		panelInferior.SetActive (true);
		panelDirecciones.SetActive(true);
	}

	public void DesactivarGUI()
	{
		panelObjetos.SetActive (false);
		panelDirecciones.SetActive (false);
		botonDiario.SetActive (false);
		panelLateral.SetActive (false);
		panelInferior.SetActive (false);
	}

	#endregion

	#region METODOS CARGA NIVEL Y TRANSICIONES

	public void Activar_Cargando()
	{
		imagenCargando.SetActive (true);
	}

	public void Activar_Fade()
	{
		imagenCargando.GetComponent<PantallaCarga> ().comenzarFade = true;
	}

	#endregion

	#region PANEL INTERACCION OBJETOS Y PERSONAJES

	public void Lanzar_Inspeccionar()
	{
		bool tiradaConExito = false;

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
				Insertar_Ventana_Lateral_Texto(GameCenter.InstanceRef.controladoraJuego.objetoPulsado.BuscarTirada(Habilidades.Fallo).TextoDescriptivo, Color.white);
				
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
				Insertar_Ventana_Inferior_Texto(true, tirada.HabilidadTirada, resultado);

				//Mostramos la descripcion anidada a la tirada de la habilidad
				Insertar_Ventana_Lateral_Texto(tirada.TextoDescriptivo, Color.white);

				//Añadimos a la descripcion minima, la descripcion nueva de la tirada
				GameCenter.InstanceRef.controladoraJuego.Modificar_Tirada_Objeto (tirada.TextoDescriptivo, Habilidades.Ninguna);

				//Ejecutamos Accion si la tuviese
				if(tirada.Accion)
				{
					//TODO Añadir Localizacion
					foreach(Localizaciones localizacion in tirada.LocalizacionAccion)
					{
						Insertar_Ventana_Inferior_Texto(localizacion, Color.yellow);
					}
				}

				//Marcamos que a sido una tirada con exito para no mostrar la tirada de fallo
				return true;
			} 
			else 
			{
				Insertar_Ventana_Inferior_Texto(false, tirada.HabilidadTirada, resultado);
				return false;
			}
		}

		return false;
	}

	public void Lanzar_Hablar()
	{
		Reestructurar_Respuestas (GameCenter.InstanceRef.controladoraJuego.personajePulsado.InicioConversacion);
	}

	public void Reestructurar_Respuestas(int numeroPregunta)
	{
		nuevaRespuesta = GameCenter.InstanceRef.controladoraJuego.personajePulsado.Devolver_Respuesta (numeroPregunta);

		if (nuevaRespuesta != null) 
		{
			if (textoLateral.text != string.Empty) 
			{
				float anteriorSizeCajaTexto = textoLateralOpciones.rectCajaTexto.rect.height;
				Insertar_Ventana_Lateral_Texto (nuevaRespuesta.TextoRespuesta, Color.yellow);
				Deslizar_Ventana_Lateral (anteriorSizeCajaTexto);
			} 
			else 
			{
				Blanquear_Texto_Lateral ("yellow", "white");
				Insertar_Ventana_Lateral_Texto (nuevaRespuesta.TextoRespuesta, Color.yellow);
			}

			listaPreguntas.Generar_Preguntas (Filtrar_Preguntas (nuevaRespuesta.MostrarPreguntas ()));
		}
		else 
		{
			Vaciar_Texto_Lateral();
			Vaciar_Panel_Preguntas();
			textoInferiorOpciones.gameObject.SetActive (true);
			panelPreguntasOpciones.gameObject.SetActive (false);
			panelObjetosOpciones.Activar ("Hablar");
		}

	}

	/// <summary>
	/// Filtra las respuestas a la pregunta segun haya avanzado el personaje o tirada
	/// </summary>
	/// <param name="lista">Lista de PreguntaBase a evaluar</param>
	/// <value>
	/// Lista de PreguntaBase evaluadas y filtradas
	/// </value>
	private PreguntaBase[] Filtrar_Preguntas(PreguntaBase[] lista)
	{
		List<PreguntaBase> nuevasPreguntas = new List<PreguntaBase> ();

		foreach (PreguntaBase preguntaNueva in lista) 
		{
			if (preguntaNueva.ComprobacionPregunta)
			{
				if (preguntaNueva.ComprobacionAccion != null)
				{
					if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.AccionRealizada(preguntaNueva.ComprobacionAccion))
						nuevasPreguntas.Add(preguntaNueva);
				}
				else
				{
					if (preguntaNueva.ComprobacionEscenas != null)
					{
						if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.EscenaVista(preguntaNueva.ComprobacionEscenas))
							nuevasPreguntas.Add(preguntaNueva);
					}
					else
					{
						if (preguntaNueva.ComprobacionHabilidad != null)
						{
							//Tirada de Habilidad
							nuevasPreguntas.Add(preguntaNueva);
						}
						else
						{
							if (preguntaNueva.ComprobacionObjetos != null)
							{
								if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.ObjetoVisto(preguntaNueva.ComprobacionObjetos))
									nuevasPreguntas.Add(preguntaNueva);
							}
							else
							{
								if(preguntaNueva.PreguntaTirada)
								{
									//Tirada de pregunta
									nuevasPreguntas.Add(preguntaNueva);
								}
							}
						}
					}
				}
			}
			else
				nuevasPreguntas.Add(preguntaNueva);
		}

		return nuevasPreguntas.ToArray ();
	}

	public void Boton_Pulsado(string textoPregunta, int idRespuesta)
	{
		Insertar_Ventana_Lateral_Texto(textoPregunta, Color.green);
		Reestructurar_Respuestas (idRespuesta);
	}

	public bool Comprobar_Pregunta(PreguntaBase pregunta)
	{
		if (pregunta.ComprobacionPregunta) 
		{
			//Comprobamos que haya visitado la escena antes
			if (!pregunta.ComprobacionEscenas.Equals (Escenas.ninguna))
					return GameCenter.InstanceRef.controladoraJuego.jugadorActual.EscenasVisitadas.Contains (pregunta.ComprobacionEscenas);

			//Comprobamos que haya alguna accion que active cierta pregunta
			if (!pregunta.ComprobacionAccion.Equals (Acciones.Ninguna))
					return GameCenter.InstanceRef.controladoraJuego.jugadorActual.AccionesRealizadas.Contains (pregunta.ComprobacionAccion);

			//Comprobamos que haya visto el objeto antes de formular la pregunta
			if (!pregunta.ComprobacionObjetos.Equals (Objetos.Ninguno))
					return GameCenter.InstanceRef.controladoraJuego.jugadorActual.ObjetosVistos.Contains (pregunta.ComprobacionObjetos);

			//Comprobamos con tirada de dados antes de mostrar pregunta
			if (!pregunta.ComprobacionHabilidad.Equals (Habilidades.Ninguna)) 
			{
				//Rescatamos el valor de la habilidad
				int valorHabilidad = GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Valor_Segun_Enum (pregunta.ComprobacionHabilidad);

				//Rescatamos valor de tirada de dados
				int resultado = GameCenter.InstanceRef.controladoraJuego.Lanzar_Dados ("1D100");

				if (resultado < valorHabilidad) 
					return true;
			}
		} 
		else 
		{
			return true;
		}

		return false;
	}

	public void Insertar_Ventana_Inferior_Texto(string textoDescriptivo, Color color)
	{
		textoInferior.text += Environment.NewLine + ObtenerColor(color) + Comillas() + textoDescriptivo + Comillas() + FinDeLineaColor ();
	}
	
	public void Insertar_Ventana_Lateral_Texto(string textoDescriptivo, Color color)
	{
		if(textoLateral.text.Equals(string.Empty))
			textoLateral.text = ObtenerColor(Color.white) + textoDescriptivo + FinDeLineaColor ();
		else
			textoLateral.text += Environment.NewLine + Environment.NewLine + ObtenerColor(color) + textoDescriptivo + FinDeLineaColor ();
	}

	public void Insertar_Ventana_Inferior_Texto(bool tirada, Habilidades habilidad, int resultado)
	{
		string aux = "";
		Color colorTirada;
		
		if (tirada) 
		{
			aux = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.Exito;
			colorTirada = Color.green;
		} 
		else 
		{
			aux = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.Fracaso;
			colorTirada = Color.red;
		}
		
		textoInferior.text += Environment.NewLine + ObtenerColor(Color.white) + "- " + GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.Tirada + " " + GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Descripcion_Segun_Enum(habilidad) + "(" + GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Valor_Segun_Enum(habilidad) + "%): " + resultado.ToString () + "." + FinDeLineaColor() + ObtenerColor(colorTirada) + aux + FinDeLineaColor();
	}
	
	public void Insertar_Ventana_Inferior_Texto(Localizaciones nombreLocalizacion, Color color)
	{
		textoInferior.text = Environment.NewLine + ObtenerColor(color) + GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.LocalizacionDescubierta + " " + Comillas() + GameCenter.InstanceRef.controladoraJuego.Devolver_Descripcion_Localizacion_Segun_Enum(nombreLocalizacion) + Comillas() + FinDeLineaColor ();
	}

	private string ObtenerColor(Color color)
	{
		if (color.Equals(Color.red))
			return "<color=red>";
		if (color.Equals(Color.green))
			return "<color=green>";	
		if (color.Equals(Color.white))
			return "<color=white>";
		if (color.Equals (Color.yellow))
			return "<color=yellow>";
		
		return null;
	}
	
	private string FinDeLineaColor()
	{
		return "</color>";
	}
	
	private string Comillas()
	{
		return "\"";
	}

	private void Deslizar_Ventana_Lateral(float anteriorSizeCajaTexto)
	{
		textoLateralOpciones.Deslizar_Texto (anteriorSizeCajaTexto);
	}

	private void Blanquear_Texto_Lateral(string colorACambiar, string colorNuevo)
	{
		textoLateral.text = textoLateral.text.Replace(colorACambiar, colorNuevo);
	}

	public void Vaciar_Texto_Lateral()
	{
		textoLateral.text = string.Empty;
	}

	public void Vaciar_Panel_Preguntas()
	{
		panelPreguntasOpciones.Vaciar_Panel();
	}

	#endregion
}