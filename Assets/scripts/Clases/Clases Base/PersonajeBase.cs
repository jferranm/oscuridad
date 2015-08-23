using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base para el Personaje interactuable en la escena
    /// </summary>
	[System.Serializable]
    public class PersonajeBase
    {
		#region VARIABLES
        private string nombre;
        /// <summary>
        /// Nombre del personaje
        /// </summary>
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

		private string descripcionNombre;
		/// <summary>
		/// Descripcion del nombre del personaje
		/// </summary>
		public string DescripcionNombre
		{
			get { return descripcionNombre; }
			set { descripcionNombre = value; }
		}

        private List<RespuestaBase> conversacionPersonaje;
        /// <summary>
        /// Lista de Frases del NPC a mostrar
        /// </summary>
        /// <value>
        /// lista generica de RespuestaBase
        /// </value>
        public List<RespuestaBase> ConversacionPersonaje
        {
            get { return conversacionPersonaje; }
            set { conversacionPersonaje = value; }
        }

        private int inicioConversacion;
        /// <summary>
        /// numero de identificador de respuesta para el inicio de conversacion
        /// </summary>
        /// <value>
        /// valor entero de idRespuesta
        /// </value>
        public int InicioConversacion
        {
            get { return inicioConversacion; }
            set { inicioConversacion = value; }
        }

		private Vector3 posicionNueva;
		/// <summary>
		/// Posicion de la camara para el zoom
		/// </summary>
		/// <value>
		/// valor tipo Vector3 para posicion en mundo del objeto en zoom
		/// </value>
		public Vector3 PosicionNueva
		{
			get { return posicionNueva; }
			set { posicionNueva = value; }
		}
		
		private Vector3 rotacionNueva;
		/// <summary>
		/// Posicion de rotacion de la camara para el zoom
		/// </summary>
		/// <value>
		/// valor tipo Vector3 para el valor de rotacion de la camara en mundo del objeto en zoom
		/// </value>
		public Vector3 RotacionNueva
		{
			get { return rotacionNueva; }
			set { rotacionNueva = value; }
		}
		
		private float smooth;
		/// <summary>
		/// Suavidad de frenado del zoom
		/// </summary>
		/// <value>
		/// valor tipo float para el calculo del frenado del zoom
		/// </value>
		public float Smooth
		{
			get { return smooth; }
			set { smooth = value; }
		}

		private List<opcionObjeto> personajeOpciones;
		/// <summary>
		/// lista de opciones de interaccion con el personaje
		/// </summary>
		/// <value>
		/// lista generica de tipo opcionObjeto
		/// </value>
		public List<opcionObjeto> PersonajeOpciones
		{
			get { return personajeOpciones; }
			set { personajeOpciones = value; }
		}
		
		private List<ObjetoTiradaBase> tiradasPersonaje;
		/// <summary>
		/// Lista de ObjetosTadaBase segun descripciones por tiradas
		/// </summary>
		/// <value>
		/// Lista Generica de tipo ObjetoTiradaBase
		/// </value>
		public List<ObjetoTiradaBase> TiradasPersonaje
		{
			get { return tiradasPersonaje; }
			set { tiradasPersonaje = value; }
		}

		private bool personajeInspeccionado;
		/// <summary>
		/// Determina si el personaje a sido inspeccionado
		/// </summary>
		/// <value>
		/// valor tipo bool para determinar si el objeto a sido inspeccionado
		/// </value>
		public bool PersonajeInspeccionado
		{
			get { return personajeInspeccionado; }
			set { personajeInspeccionado = value; }
		}

		#endregion

		#region CONSTRUCTORES

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public PersonajeBase()
        {
            conversacionPersonaje = new List<RespuestaBase>();
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="nombre">nombre del personaje</param>
        public PersonajeBase(string nombre)
        {
            conversacionPersonaje = new List<RespuestaBase>();

            this.nombre = nombre;
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="respuestas">array de tipo RespuestaBase de respuestas a mostrar del personaje</param>
        public PersonajeBase(RespuestaBase[] respuestas)
        {
            conversacionPersonaje = new List<RespuestaBase>();

            AddRespuesta(respuestas);
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="nombre">nombre del personaje</param>
        /// <param name="opciones">array de tipo enum de opciones de interaccion de personaje</param>
        /// <param name="respuestas">array de tipo RespuestaBase de respuestas a mostrar del personaje</param>
        public PersonajeBase(string nombre, RespuestaBase[] respuestas)
        {
            conversacionPersonaje = new List<RespuestaBase>();

            this.nombre = nombre;
            AddRespuesta(respuestas);
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="nombre">nombre del personaje</param>
        /// <param name="opciones">array de tipo enum de opciones de interaccion de personaje</param>
        /// <param name="respuestas">array de tipo RespuestaBase de respuestas a mostrar del personaje</param>
        /// <param name="idComienzo">valor entero que marca cual sera la RespuestaBase donde comenzara la conversacion</param>
        public PersonajeBase(string nombre, RespuestaBase[] respuestas, int idComienzo)
        {
            conversacionPersonaje = new List<RespuestaBase>();

            this.nombre = nombre;
            inicioConversacion = idComienzo;
            AddRespuesta(respuestas);
        }

		#endregion

		#region METODOS

        /// <summary>
        /// Añade una respuesta a la conversacion
        /// </summary>
        /// <param name="respuesta">respuesta tipo RespuestaBase</param>
        public void AddRespuesta(RespuestaBase respuesta)
        {
            conversacionPersonaje.Add(respuesta);
        }

        /// <summary>
        /// Añade varias respuestas a la conversacion
        /// </summary>
        /// <param name="respuestas">array de tipo RespuestaBase</param>
        public void AddRespuesta(RespuestaBase[] respuestas)
        {
            conversacionPersonaje.AddRange(respuestas);
        }

        /// <summary>
        /// Borra una respuesta
        /// </summary>
        /// <param name="respuesta">respuesta tipo RespuestaBase</param>
        public void BorrarRespuesta(RespuestaBase respuesta)
        {
            conversacionPersonaje.Remove(respuesta);
        }

        /// <summary>
        /// Borra varias respuestas
        /// </summary>
        /// <param name="respuestas">array de tipo RespuestaBase</param>
        public void BorrarRespuesta(RespuestaBase[] respuestas)
        {
            foreach (RespuestaBase respuesta in respuestas)
            {
                conversacionPersonaje.Remove(respuesta);
            }
        }

        /// <summary>
        /// Muestra la conversacion del personaje
        /// </summary>
        /// <returns> array de tipo RespuestaBase</returns>
        public RespuestaBase[] MostrarRespuestas()
        {
            return conversacionPersonaje.ToArray();
        }

		/// <summary>
		/// Devuelve una Respuesta a partir de su id de Respuesta
		/// </summary>
		/// <returns> objeto tipo RespuestaBase</returns>
		public RespuestaBase Devolver_Respuesta(int numRespuesta)
		{
			return  MostrarRespuestas().ToList().Find (x => x.IdRespuesta == numRespuesta);
		}

		/// <summary>
		/// Añade una opcion al personaje
		/// </summary>
		/// <param name="opcion">enum de tipo opcionObjeto</param>
		public void AddOpciones(opcionObjeto opcion)
		{
			personajeOpciones.Add(opcion);
		}
		
		/// <summary>
		/// Añadir varias opciones al personaje
		/// </summary>
		/// <param name="opciones">array de enum opcionObjeto</param>
		public void AddOpciones(opcionObjeto[] opciones)
		{
			personajeOpciones.AddRange(opciones);
		}
		
		/// <summary>
		/// Borra una opcion del personaje
		/// </summary>
		/// <param name="opcion">enum de tipo opcionObjeto</param>
		public void BorrarOpciones(opcionObjeto opcion)
		{
			personajeOpciones.Remove(opcion);
		}
		
		/// <summary>
		/// Borra varias opciones del personaje
		/// </summary>
		/// <param name="opciones">array de enum opcionObjeto</param>
		public void BorrarOpciones(opcionObjeto[] opciones)
		{
			foreach (opcionObjeto opcion in opciones)
			{
				personajeOpciones.Remove(opcion);
			}
		}
		
		/// <summary>
		/// Muesta la lista de opciones del personaje
		/// </summary>
		/// <returns>array de enum opcionObjeto</returns>
		public opcionObjeto[] MostrarOpciones()
		{
			return personajeOpciones.ToArray();
		}
		
		/// <summary>
		/// Añade una descripcion con tirada al personaje
		/// </summary>
		/// <param name="tirada">objeto tipo ObjetoTiradaBase</param>
		public void AddTiradas(ObjetoTiradaBase tirada)
		{
			tiradasPersonaje.Add(tirada);
		}
		
		/// <summary>
		/// Añade varias descripciones con tirada al personaje
		/// </summary>
		/// <param name="tiradas">array de objetos tipo ObjetoTiradaBase</param>
		public void AddTiradas(ObjetoTiradaBase[] tiradas)
		{
			tiradasPersonaje.AddRange(tiradas);
		}
		
		/// <summary>
		/// Borra una descripcion con tirada en el personaje
		/// </summary>
		/// <param name="tirada">objeto tipo ObjetoTiradaBase</param>
		public void BorrarTiradas(ObjetoTiradaBase tirada)
		{
			tiradasPersonaje.Remove(tirada);
		}
		
		/// <summary>
		/// Borrar varias descripciones con tirada del personaje
		/// </summary>
		/// <param name="tiradas">array de tipo ObjetoTiradaBase</param>
		public void BorrarTiradas(ObjetoTiradaBase[] tiradas)
		{
			foreach (ObjetoTiradaBase opcion in tiradas)
			{
				tiradasPersonaje.Remove(opcion);
			}
		}
		
		/// <summary>
		/// Mostrar lista de decripciones con tirada del personaje
		/// </summary>
		/// <returns>Array de tipo ObjetoTiradaBase</returns>
		public ObjetoTiradaBase[] MostrarTiradas()
		{
			return tiradasPersonaje.ToArray();
		}
		
		/// <summary>
		/// Mostrar lista de decripciones con tirada del personaje que no sean ni Fallo ni Ninguno
		/// </summary>
		/// <returns>Array de tipo ObjetoTiradaBase</returns>
		public ObjetoTiradaBase[] MostrarTiradasInspeccionar()
		{
			return tiradasPersonaje.FindAll (x => (x.HabilidadTirada != Habilidades.Ninguna && x.HabilidadTirada != Habilidades.Fallo)).ToArray();
		}
		
		/// <summary>
		/// Muestra la descripcion del personaje sin tiradas
		/// </summary>
		/// <returns>string con la descripcion del objeto</returns>
		public string MostrarDescripcionBasica()
		{
			return tiradasPersonaje.Find (x => x.HabilidadTirada == Habilidades.Ninguna).TextoDescriptivo;
		}
		
		/// <summary>
		/// Busca una tirada de habilidad especifica dentro de las tiradas del personaje
		/// </summary>
		/// <param name="habilidad">habilidad a buscar</param>
		/// <returns>la tirada que trabajaria con esa habilidad</returns>
		public ObjetoTiradaBase BuscarTirada(Habilidades habilidad)
		{
			return tiradasPersonaje.Find (x => x.HabilidadTirada == habilidad);
		}
		
		/// <summary>
		/// Busca la existencia de una tirada de habilidad especifica dentro de las tiradas del personaje
		/// </summary>
		/// <param name="habilidad">habilidad a buscar</param>
		/// <returns>true si existe, false sino</returns>
		public bool ExisteTirada(Habilidades habilidad)
		{
			return !(tiradasPersonaje.Find (x => x.HabilidadTirada == habilidad) == null);
		}


		#endregion
    }
}
