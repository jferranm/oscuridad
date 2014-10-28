using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

[System.Serializable]
public class ControladoraTextos
{
	[HideInInspector]
	public GameObject objetoSeleccionado;
	[HideInInspector]
	public string escenaActiva;

	[HideInInspector]
	public string textoDescripcion;
	[HideInInspector]
	public string textoCabecera;

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

	[HideInInspector]
	public List<Etiqueta> listaObjetos = new List<Etiqueta>();
	[HideInInspector]
	public List<Etiqueta> listaTiradas = new List<Etiqueta>();

	public ControladoraTextos()
	{
	}
	
	private static ControladoraTextos instanceRef;
	
	public static ControladoraTextos InstanceRef()
	{
		if (instanceRef == null)
		{
			instanceRef = new ControladoraTextos();
		}
		
		return instanceRef;
	}

	public void Generar_Descripcion(string nombre)
	{
		textoCabecera = Devolver_Cabecera_Descripcion(nombre);
		listaTiradas.Add (new Etiqueta(Devolver_Texto_Descriptivo (nombre), Color.white));
	}

	private string Devolver_Cabecera_Descripcion(string nombreObjeto)
	{
		Descripciones nuevaDescripcion = new Descripciones(nombreObjeto);
		string descripcionAuxiliar = nuevaDescripcion.Devolver_Cabecera();
		nuevaDescripcion.Cerrar();
		
		return descripcionAuxiliar;
	}

	private string Devolver_Texto_Descriptivo(string nombreObjeto)
	{
		Descripciones nuevaDescripcion = new Descripciones(nombreObjeto);
		string descripcionAuxiliar = nuevaDescripcion.Devolver_Descripcion();
		nuevaDescripcion.Cerrar();
		
		return descripcionAuxiliar;
	}

	public void Generar_Descripcion_Objeto(string nombreObjeto)
	{
		listaObjetos.Add(new Etiqueta(Devolver_Texto_Descriptivo (objetoSeleccionado.tag.ToString()), Color.white));
	}

	public void Lanzar_Inspeccionar()
	{
		List<string> listaAuxiliar = new List<string> ();
		listaAuxiliar = Texto_Tirada ();
		
		foreach (string label in listaAuxiliar) 
		{
			if(label.Contains("FALLIDA"))
			{
				//labels marcados en rojo
				Insertar_Label_Tabla(false, label, Color.red);
			}
			else
			{
				if(label.Contains("EXITOSA"))
				{
					//Labels marcados en verde
					Insertar_Label_Tabla(true, label.Remove(0, label.IndexOf("\n")+1), Color.green);
					Insertar_Label_Tabla(false, label.Remove(label.IndexOf("\n")), Color.green);
					
					ControlXMLGlobal nuevoControlXML = new ControlXMLGlobal();
					nuevoControlXML.Crear_Elemento_Descripciones(objetoSeleccionado.tag.ToString(), label.Remove(0, label.IndexOf("\n")+1));
					break;
				}
				else
				{
					//Labels sin marcar
					Insertar_Label_Tabla (true, label, Color.white);
					
					ControlXMLGlobal nuevoControlXML = new ControlXMLGlobal();
					nuevoControlXML.Crear_Elemento_Descripciones(objetoSeleccionado.tag.ToString(), label);
				}
			}
			
		}
	}
	
	public void Lanzar_Hablar()
	{
		//ventanaIDTiradas = 2;
		
		Iniciar_Conversacion (GameCenter.InstanceRef.controladoraJugador.objetoInteractuableRef.inicioConversacion, GameCenter.InstanceRef.controladoraJugador.objetoPulsado.tag.ToString ());
		
	}

	public void Insertar_Label_Tabla(bool tipo, string texto, Color color)
	{
		if (tipo)   //Para la ventana de Objetos
		{
			listaObjetos.Add (new Etiqueta(texto, color));
		} 
		else 		//Para la ventana de Tiradas
		{
			listaTiradas.Add (new Etiqueta(texto, color));
		}
	}

	public List<string> Texto_Tirada()
	{
		//Creamos una nueva descripcion segun la tirada
		Descripciones nuevaDescripcion = new Descripciones(objetoSeleccionado);
		List<string> listaDescripcionAuxiliar = new List<string>(nuevaDescripcion.Devolver_Descripcion_Tiradas());
		nuevaDescripcion.Cerrar();
		
		//Hacemos que el objeto ya no sea inspeccionable;
		GameCenter.InstanceRef.controladoraObjetos.Cambiar_Opcion_Objeto (objetoSeleccionado.tag.ToString(), false, false, false, true, false);
		
		return listaDescripcionAuxiliar;		
	}
	
	public void Limpiar_Datos()
	{
		textoPregunta = "";
		textoBoton1 = "";
		textoBoton2 = "";
		textoBoton3 = "";
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
			//ventanaIDTiradas = 1;
		}
		else
		{
			Mostrar_Preguntas(valoresConversacion);
		}
	}
	
	public void Mostrar_Preguntas(Pregunta datosMostrar)
	{
		Insertar_Label_Tabla (true, datosMostrar.textoPregunta, Color.white);
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
}
