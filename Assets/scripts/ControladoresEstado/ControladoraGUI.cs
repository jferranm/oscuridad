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
	public RespuestaNPCBase nuevaRespuesta = new RespuestaNPCBase();

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
		if(GameCenter.InstanceRef.controladoraJuego.interactuablePulsado != null)
			GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddObjetoVisto (GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Interactuable);
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
		foreach (InteractuableTiradaBase tirada in GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.MostrarTiradasInspeccionar()) 
		{
			if (tirada.Comprobacion) 
			{
				if(!tirada.EscenaComprobacion.Equals(Escenas.ninguna))
					if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.EscenaVista(tirada.EscenaComprobacion))
						return;

				if(!tirada.InteractuableComprobacion.Equals(Interactuables.Ninguno))
					if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.ObjetoVisto(tirada.InteractuableComprobacion))
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
				Insertar_Ventana_Lateral_Texto(tirada.TextoDescriptivo, colorTexto.blanco);
				
				//Añadimos a la descripcion minima, la descripcion nueva de la tirada
				GameCenter.InstanceRef.controladoraJuego.Modificar_Tirada_Objeto (tirada.TextoDescriptivo, Habilidades.Ninguna);
				
				//Ejecutamos Accion si la tuviese
				if(tirada.Accion)
				{
					foreach(Localizaciones localizacion in tirada.LocalizacionAccion)
					{
						if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.LocalizacionesDescubiertas.Contains(localizacion))
						{
							Insertar_Ventana_Inferior_Texto(localizacion, colorTexto.amarillo);
							GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddLocalizacionDescubierta(localizacion);
						}
					}
					
					foreach(Acciones accion in tirada.AccionesAccion)
					{
						if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.AccionRealizada(accion))
						{
							GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddAccionRealizada(accion);
							GameCenter.InstanceRef.controladoraJuego.EjecutarAccion(accion);
						}
						
					}
				}

				//Desconectamos la opcion de inspeccionar la opcion de inspeccionar
				GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.InteractuableInspeccionado = true;

				return;
			} 
			else 
				Insertar_Ventana_Inferior_Texto(false, tirada.HabilidadTirada, resultado);
		}

		if(GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.ExisteTirada(Habilidades.Fallo))
		{
			InteractuableTiradaBase tiradaFallo = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.BuscarTirada(Habilidades.Fallo);
			//Mostramos la descripcion anidada a la tirada de la habilidad
			Insertar_Ventana_Lateral_Texto(tiradaFallo.TextoDescriptivo, colorTexto.blanco);
			
			//Añadimos a la descripcion minima, la descripcion nueva de la tirada
			GameCenter.InstanceRef.controladoraJuego.Modificar_Tirada_Objeto(tiradaFallo.TextoDescriptivo, Habilidades.Ninguna);

			//Ejecutamos Accion si la tuviese
			if(tiradaFallo.Accion)
			{
				foreach(Localizaciones localizacion in tiradaFallo.LocalizacionAccion)
				{
					if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.LocalizacionesDescubiertas.Contains(localizacion))
					{
						Insertar_Ventana_Inferior_Texto(localizacion, colorTexto.amarillo);
						GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddLocalizacionDescubierta(localizacion);
					}
				}
				
				foreach(Acciones accion in tiradaFallo.AccionesAccion)
				{
					if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.AccionRealizada(accion))
					{
						GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddAccionRealizada(accion);
						GameCenter.InstanceRef.controladoraJuego.EjecutarAccion(accion);
					}
					
				}
			}
		}
		//Desconectamos la opcion de inspeccionar la opcion de inspeccionar
		GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.InteractuableInspeccionado = true;
	}

	public void Lanzar_Hablar()
	{
		Reestructurar_Respuestas (GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.InicioConversacion, true);
	}

	public void Reestructurar_Respuestas(int numeroPregunta, bool inicio)
	{
		PreguntaUsuarioBase pregunta = new PreguntaUsuarioBase();

		if (inicio)
			nuevaRespuesta = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Devolver_Respuesta (numeroPregunta);
		else 
		{
			pregunta = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Devolver_Pregunta (numeroPregunta);
			pregunta.PreguntaEjecutada = true;

			if (GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.PreguntaConTirada (numeroPregunta)) 
			{
				int valorHabilidad = GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Valor_Segun_Enum (pregunta.ComprobacionHabilidad);
				int resultado = GameCenter.InstanceRef.controladoraJuego.Lanzar_Dados ("1D100");

				pregunta.IdRespuestaNPC = (resultado < valorHabilidad) ? pregunta.IdRespuestaAcierto : pregunta.IdRespuestaFallo;
			}

			nuevaRespuesta = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Devolver_Respuesta (pregunta.IdRespuestaNPC);
		}

		//Si existe respuesta
		if (nuevaRespuesta != null) 
		{
			Blanquear_Texto_Lateral ("yellow", "white");
			//Si el texto lateral esta vacio
			if (textoLateral.text != string.Empty) 
			{
				float anteriorSizeCajaTexto = textoLateralOpciones.rectCajaTexto.rect.height;
				if(pregunta.TextoPregunta != null)
					Insertar_Ventana_Lateral_Texto (pregunta.TextoPregunta, colorTexto.verde, tipoTexto.negrita);
				Insertar_Ventana_Lateral_Texto (nuevaRespuesta.TextoRespuesta, colorTexto.amarillo);
				Deslizar_Ventana_Lateral (anteriorSizeCajaTexto);
			} 
			//Si el texto lateral no esta vacio
			else
				Insertar_Ventana_Lateral_Texto (nuevaRespuesta.TextoRespuesta, colorTexto.amarillo);

			//Comprobacion si la respuesta tiene un desbloqueo o accion asociada
			if(nuevaRespuesta.Comprobacion)
			{
				//Desbloqueo de localizacion
				if(nuevaRespuesta.LocalizacionSeleccionada != Localizaciones.Ninguna)
				{
					if(!GameCenter.InstanceRef.controladoraJuego.jugadorActual.LocalizacionesDescubiertas.Contains(nuevaRespuesta.LocalizacionSeleccionada))
					{
						Insertar_Ventana_Lateral_Texto(nuevaRespuesta.LocalizacionSeleccionada, colorTexto.verde);
						GameCenter.InstanceRef.controladoraJuego.jugadorActual.AddLocalizacionDescubierta(nuevaRespuesta.LocalizacionSeleccionada);
					}
				}
			}

			//Comprobacion de que la respuesta no tiene pregunta asociada y vuelve a una pregunta anterior
			if(nuevaRespuesta.SinRespuesta)
				nuevaRespuesta = GameCenter.InstanceRef.controladoraJuego.interactuablePulsado.Devolver_Respuesta (nuevaRespuesta.DireccionRespuesta);

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
	/// <param name="lista">Lista de PreguntaUsuarioBase a evaluar</param>
	/// <return>Lista de PreguntaUsuarioBase evaluadas y filtradas</return>
	private PreguntaUsuarioBase[] Filtrar_Preguntas(PreguntaUsuarioBase[] lista)
	{
		List<PreguntaUsuarioBase> nuevasPreguntas = new List<PreguntaUsuarioBase> ();

		foreach (PreguntaUsuarioBase preguntaNueva in lista) 
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
							if (preguntaNueva.ComprobacionInteractuables != Interactuables.Ninguno)
							{
								if(GameCenter.InstanceRef.controladoraJuego.jugadorActual.ObjetoVisto(preguntaNueva.ComprobacionInteractuables))
									nuevasPreguntas.Add(preguntaNueva);
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
	/// <param name="lista">Lista de PreguntaUsuarioBase a ordenar</param>
	/// <return>Lista de PreguntaUsuarioBase ordenadas</return>
	private PreguntaUsuarioBase[] Ordenar_Preguntas(PreguntaUsuarioBase[] preguntas)
	{
		PreguntaUsuarioBase ultimaPregunta = preguntas.ToList ().Find (x => x.IdRespuestaNPC == 0);

		if (ultimaPregunta != null) 
		{
			List<PreguntaUsuarioBase> preguntasAux = new List<PreguntaUsuarioBase>();
			foreach (PreguntaUsuarioBase pregunta in preguntas) 
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

	public void Insertar_Ventana_Inferior_Texto(string textoDescriptivo, colorTexto color)
	{
		textoInferior.text += Environment.NewLine + "\"" + FormatearTexto(textoDescriptivo, optionalColorTexto: color) + "\"";
	}
	
	public void Insertar_Ventana_Lateral_Texto(string textoDescriptivo, colorTexto color)
	{
		if(textoLateral.text.Equals(string.Empty))
			textoLateral.text = FormatearTexto(textoDescriptivo, optionalColorTexto: color);
		else
			textoLateral.text += Environment.NewLine + Environment.NewLine + FormatearTexto(textoDescriptivo, optionalColorTexto: color);

		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx (GameCenter.InstanceRef.controladoraSonidos.sonidoEscribir);
	}

	public void Insertar_Ventana_Lateral_Texto(string textoDescriptivo, colorTexto color, tipoTexto tipo)
	{
		if(textoLateral.text.Equals(string.Empty))
			textoLateral.text = FormatearTexto(textoDescriptivo, optionalColorTexto: color, optionalTipoTexto: tipo);
		else
			textoLateral.text += Environment.NewLine + Environment.NewLine + FormatearTexto(textoDescriptivo, optionalColorTexto: color, optionalTipoTexto: tipo);
		
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx (GameCenter.InstanceRef.controladoraSonidos.sonidoEscribir);
	}

	public void Insertar_Ventana_Inferior_Texto(bool tirada, Habilidades habilidad, int resultado)
	{
		string aux = "";
		colorTexto color;
		
		if (tirada) 
		{
			aux = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.Exito;
			color = colorTexto.verde;
		} 
		else 
		{
			aux = GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.Fracaso;
			color = colorTexto.rojo;
		}
		
		textoInferior.text += Environment.NewLine + FormatearTexto("- " + GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.Tirada + " " + GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Descripcion_Segun_Enum(habilidad) + "(" + GameCenter.InstanceRef.controladoraJuego.jugadorActual.HabilidadesJugador.Devolver_Valor_Segun_Enum(habilidad) + "%): " + resultado.ToString () + ".", optionalColorTexto: colorTexto.blanco) + FormatearTexto(aux, optionalColorTexto: color);
	}
	
	public void Insertar_Ventana_Inferior_Texto(Localizaciones nombreLocalizacion, colorTexto color)
	{
		textoInferior.text += Environment.NewLine + FormatearTexto(GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.LocalizacionDescubierta + " \"" + GameCenter.InstanceRef.controladoraJuego.Devolver_Descripcion_Localizacion_Segun_Enum(nombreLocalizacion) + "\"", optionalColorTexto: color);
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx (GameCenter.InstanceRef.controladoraSonidos.sonidoEscribir);
	}

	public void Insertar_Ventana_Inferior_Texto(Interactuables interactuable, colorTexto color)
	{
		textoInferior.text += Environment.NewLine + FormatearTexto("\"" + GameCenter.InstanceRef.controladoraJuego.Devolver_Descripcion_Objeto_Segun_Enum(interactuable) + "\" " + GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.ObjetoInventario, optionalColorTexto: color);
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx (GameCenter.InstanceRef.controladoraSonidos.sonidoEscribir);
	}

	public void Insertar_Ventana_Lateral_Texto(Localizaciones nombreLocalizacion, colorTexto color)
	{
		textoLateral.text += Environment.NewLine + Environment.NewLine + FormatearTexto(GameCenter.InstanceRef.controladoraJuego.textosMenusTraduccion.LocalizacionDescubierta + " \"" + GameCenter.InstanceRef.controladoraJuego.Devolver_Descripcion_Localizacion_Segun_Enum(nombreLocalizacion) + "\"", optionalColorTexto: color);
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx (GameCenter.InstanceRef.controladoraSonidos.sonidoEscribir);
	}

	private string FormatearTexto(string texto, opcionTexto optionalOpcionTexto = opcionTexto.minusculas, tipoTexto optionalTipoTexto = tipoTexto.normal, colorTexto optionalColorTexto = colorTexto.blanco)
	{
		const string COLOR = "<color=";
		const string COLORFIN = "</color>";
		const string NEGRITA = "<b>";
		const string NEGRITAFIN = "</b>";
		const string CURSIVA = "<i>";
		const string CURSIVAFIN = "</i>";
		const string SUBRAYADO = "<u>";
		const string SUBRAYADOFIN = "</u>";

		switch (optionalOpcionTexto) 
		{
			case opcionTexto.mayusculas:
			{
				texto = texto.ToUpper();
				break;
			}
		}

		switch(optionalColorTexto)
		{
			case colorTexto.blanco:
			{
				texto = COLOR + "white>" + texto + COLORFIN;
				break;
			}

			case colorTexto.amarillo:
			{
				texto = COLOR + "yellow>" + texto + COLORFIN;
				break;
			}

			case colorTexto.rojo:
			{
				texto = COLOR + "red>" + texto + COLORFIN;
				break;
			}

			case colorTexto.verde:
			{
				texto = COLOR + "green>" + texto + COLORFIN;
				break;
			}
		}

		switch(optionalTipoTexto)
		{
			case tipoTexto.negrita:
			{
				texto = NEGRITA + texto + NEGRITAFIN;
				break;
			}
				
			case tipoTexto.cursiva:
			{
				texto = CURSIVA + texto + CURSIVAFIN;
				break;
			}

			case tipoTexto.subrayado:
			{
				texto = SUBRAYADO + texto + SUBRAYADOFIN;
				break;
			}
		}

		return texto;
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