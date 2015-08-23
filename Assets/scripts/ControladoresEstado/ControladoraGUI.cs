using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
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
	public GameObject panelLibro;

	public Text textoLateral;
	public TextoLateralOpciones textoLateralOpciones;
	public Text textoInferior;
	public TextoInferiorOpciones textoInferiorOpciones;
	public PanelPreguntasOpciones panelPreguntasOpciones;
	public ListaPreguntas listaPreguntas;
	public PanelObjetosOpciones panelObjetosOpciones;

	#endregion

	#region CONSTRUCTORES
	public ControladoraGUI()
	{

	}

	#endregion

	#region METODOS

	public void Awake()
	{
		imagenCargando = GameObject.Find ("ImagenCargando");
		panelLateral = GameObject.Find ("PanelLateral");
		panelInferior = GameObject.Find ("PanelInferior");
		botonDiario = GameObject.Find ("BotonDiario");
		panelDirecciones = GameObject.Find ("PanelDirecciones");
		panelObjetos = GameObject.Find ("PanelObjetos");
		panelLibro = GameObject.Find ("Libro");
		
		textoLateral = panelLateral.GetComponentInChildren<Text>();
		textoLateralOpciones = textoLateral.GetComponent<TextoLateralOpciones> ();
		textoInferior = panelInferior.GetComponentInChildren<Text>();
		textoInferiorOpciones = textoInferior.GetComponent<TextoInferiorOpciones> ();
		panelPreguntasOpciones = GameObject.Find ("PanelPreguntas").GetComponent<PanelPreguntasOpciones> ();
		listaPreguntas = panelInferior.GetComponentInChildren<ScrollRect> ().GetComponent<ListaPreguntas> ();
		panelObjetosOpciones = panelObjetos.GetComponent<PanelObjetosOpciones> ();
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
		foreach (ObjetoTiradaBase tirada in GameCenter.InstanceRef.controladoraJuego.Devolver_Tiradas_Inspeccionar()) 
		{
			if (tirada.Comprobacion) 
			{
				if(!tirada.EscenaComprobacion.Equals(Escenas.ninguna))
					if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.EscenaVista(tirada.EscenaComprobacion))
						return;

				if(!tirada.ObjetoComprobacion.Equals(Objetos.Ninguno))
					if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.ObjetoVisto(tirada.ObjetoComprobacion))
						return;
			}

			//Rescatamos el valor de la habilidad
			int valorHabilidad = GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Valor_Segun_Enum (tirada.HabilidadTirada);
			
			//Rescatamos valor de tirada de dados
			int resultado = GameCenter.InstanceRef.controladoraJuego.Lanzar_Dados ("1D100");

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
					foreach(Localizaciones localizacion in tirada.LocalizacionAccion)
					{
						if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.LocalizacionesDescubiertas.Contains(localizacion))
						{
							Insertar_Ventana_Inferior_Texto(localizacion, Color.yellow);
							GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddLocalizacionDescubierta(localizacion);
						}
					}
					
					foreach(Acciones accion in tirada.AccionesAccion)
					{
						if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.AccionRealizada(accion))
						{
							GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddAccionRealizada(accion);
							//TODO: crear metodo para tipo de acciones desencadenes acciones... (Ejem.Tirada de cerrajeria abre la puerta del desvan)
						}
						
					}
				}

				//Desconectamos la opcion de inspeccionar la opcion de inspeccionar
				GameCenter.InstanceRef.controladoraJuego.ObjetoPersonaje_Inspeccionado(true);

				return;
			} 
			else 
				Insertar_Ventana_Inferior_Texto(false, tirada.HabilidadTirada, resultado);
		}

		if(GameCenter.InstanceRef.controladoraJuego.Devolver_ExisteTirada(Habilidades.Fallo))
		{
			//Mostramos la descripcion anidada a la tirada de la habilidad
			Insertar_Ventana_Lateral_Texto(GameCenter.InstanceRef.controladoraJuego.Devolver_Buscar_Tirada(Habilidades.Fallo).TextoDescriptivo, Color.white);
			
			//Añadimos a la descripcion minima, la descripcion nueva de la tirada
			GameCenter.InstanceRef.controladoraJuego.Modificar_Tirada_Objeto(GameCenter.InstanceRef.controladoraJuego.Devolver_Buscar_Tirada(Habilidades.Fallo).TextoDescriptivo, Habilidades.Ninguna);
		}
		//Desconectamos la opcion de inspeccionar la opcion de inspeccionar
		GameCenter.InstanceRef.controladoraJuego.ObjetoPersonaje_Inspeccionado(true);
	}

	public void Lanzar_Hablar()
	{
		Reestructurar_Respuestas (GameCenter.InstanceRef.controladoraJuego.personajePulsado.InicioConversacion);
	}

	public void Reestructurar_Respuestas(int numeroPregunta)
	{
		nuevaRespuesta = GameCenter.InstanceRef.controladoraJuego.personajePulsado.Devolver_Respuesta (numeroPregunta);

		//Si existe respuesta
		if (nuevaRespuesta != null) 
		{
			Blanquear_Texto_Lateral ("yellow", "white");
			//Si el texto lateral esta vacio
			if (textoLateral.text != string.Empty) 
			{
				float anteriorSizeCajaTexto = textoLateralOpciones.rectCajaTexto.rect.height;
				Insertar_Ventana_Lateral_Texto (nuevaRespuesta.TextoRespuesta, Color.yellow);
				Deslizar_Ventana_Lateral (anteriorSizeCajaTexto);
			} 
			//Si el texto lateral no esta vacio
			else
				Insertar_Ventana_Lateral_Texto (nuevaRespuesta.TextoRespuesta, Color.yellow);

			//Comprobacion si la respuesta tiene un desbloqueo o accion asociada
			if(nuevaRespuesta.Comprobacion)
			{
				//Desbloqueo de localizacion
				if(nuevaRespuesta.LocalizacionSeleccionada != Localizaciones.Ninguna)
				{
					if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.LocalizacionesDescubiertas.Contains(nuevaRespuesta.LocalizacionSeleccionada))
					{
						Insertar_Ventana_Lateral_Texto(nuevaRespuesta.LocalizacionSeleccionada, Color.green);
						GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddLocalizacionDescubierta(nuevaRespuesta.LocalizacionSeleccionada);
					}
				}
			}

			//Comprobacion de que la respuesta no tiene pregunta asociada y vuelve a una pregunta anterior
			if(nuevaRespuesta.SinRespuesta)
				nuevaRespuesta = GameCenter.InstanceRef.controladoraJuego.personajePulsado.Devolver_Respuesta (nuevaRespuesta.DireccionRespuesta);

			//Generamos las preguntas asociadas a la respuesta
			listaPreguntas.Generar_Preguntas (Filtrar_Preguntas (nuevaRespuesta.MostrarPreguntas ()));

		}
		//Sino existe respuesta
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
	/// <return>Lista de PreguntaBase evaluadas y filtradas</return>
	private PreguntaBase[] Filtrar_Preguntas(PreguntaBase[] lista)
	{
		List<PreguntaBase> nuevasPreguntas = new List<PreguntaBase> ();

		foreach (PreguntaBase preguntaNueva in lista) 
		{
			if (preguntaNueva.ComprobacionPregunta)
			{
				if (preguntaNueva.ComprobacionAccion != Acciones.Ninguna)
				{
					if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.AccionRealizada(preguntaNueva.ComprobacionAccion))
						nuevasPreguntas.Add(preguntaNueva);
				}
				else
				{
					if (preguntaNueva.ComprobacionEscenas != Escenas.ninguna)
					{
						if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.EscenaVista(preguntaNueva.ComprobacionEscenas))
							nuevasPreguntas.Add(preguntaNueva);
					}
					else
					{
						if (preguntaNueva.ComprobacionHabilidad != Habilidades.Ninguna)
						{
							int valorHabilidad = GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Valor_Segun_Enum (preguntaNueva.ComprobacionHabilidad);
							int resultado = GameCenter.InstanceRef.controladoraJuego.Lanzar_Dados ("1D100");
							
							if (resultado < valorHabilidad)
							{
								//Tirada Conseguida
								nuevasPreguntas.Add(preguntaNueva);
								preguntaNueva.ComprobacionPregunta = false;
								preguntaNueva.ComprobacionHabilidad = Habilidades.Ninguna;
							}
							else
							{
								//Tirada no Conseguida
								nuevaRespuesta.BorrarPregunta(preguntaNueva);
							}
						}
						else
						{
							if (preguntaNueva.ComprobacionObjetos != Objetos.Ninguno)
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


		return Ordenar_Preguntas(nuevasPreguntas.ToArray());
	}

	/// <summary>
	/// Ordena las preguntas para que la ultima de ellas sea la de volver al menu, salir o volver a la pregunta anterior
	/// </summary>
	/// <param name="lista">Lista de PreguntaBase a ordenar</param>
	/// <return>Lista de PreguntaBase ordenadas</return>
	private PreguntaBase[] Ordenar_Preguntas(PreguntaBase[] preguntas)
	{
		PreguntaBase ultimaPregunta = preguntas.ToList ().Find (x => x.IdRespuesta == 0);

		if (ultimaPregunta != null) 
		{
			List<PreguntaBase> preguntasAux = new List<PreguntaBase>();
			foreach (PreguntaBase pregunta in preguntas) 
			{
				if(!pregunta.Equals(ultimaPregunta))
					preguntasAux.Add(pregunta);
			}

			preguntasAux.Add(ultimaPregunta);
			preguntas = preguntasAux.ToArray();
		}

		return preguntas;
	}

	#endregion

	#region VENTANAS INTERACCION

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

		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx (GameCenter.InstanceRef.controladoraSonidos.sonidoEscribir);
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
		textoInferior.text = textoInferior.text + Environment.NewLine + ObtenerColor(color) + GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.LocalizacionDescubierta + " " + Comillas() + GameCenter.InstanceRef.controladoraJuego.Devolver_Descripcion_Localizacion_Segun_Enum(nombreLocalizacion) + Comillas() + FinDeLineaColor ();
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx (GameCenter.InstanceRef.controladoraSonidos.sonidoEscribir);
	}

	public void Insertar_Ventana_Lateral_Texto(Localizaciones nombreLocalizacion, Color color)
	{
		textoLateral.text = textoLateral.text + Environment.NewLine + Environment.NewLine + ObtenerColor(color) + GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.LocalizacionDescubierta + " " + Comillas() + GameCenter.InstanceRef.controladoraJuego.Devolver_Descripcion_Localizacion_Segun_Enum(nombreLocalizacion) + Comillas() + FinDeLineaColor ();
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx (GameCenter.InstanceRef.controladoraSonidos.sonidoEscribir);
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