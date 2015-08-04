using System;
using System.Collections.Generic;
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

		public RespuestaBase Devolver_Respuesta(int numRespuesta)
		{
			foreach (RespuestaBase aux in MostrarRespuestas()) 
			{
				if (aux.IdRespuesta.Equals(numRespuesta))
					return aux;
			}

			return null;
		}

		#endregion
    }
}
