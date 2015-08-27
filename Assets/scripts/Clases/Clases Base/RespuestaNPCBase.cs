using System;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base para Respuesta del NPC en una conversacion
    /// </summary>
	[System.Serializable]
    public class RespuestaNPCBase
    {
		#region VARIABLES
        private string textoRespuesta;
        /// <summary>
        /// Texto de la respuesta del NPC
        /// </summary>
        /// <value>
        /// cadena de texto con la respuesta del NPC
        /// </value>
        public string TextoRespuesta
        {
            get { return textoRespuesta; }
            set { textoRespuesta = value; }
        }

        private int idRespuestaNPC;
        /// <summary>
        /// Id de la respuesta del NPC
        /// </summary>
        /// <value>
        /// valor entero para diferenciar respuestas
        /// </value>
        public int IdRespuestaNPC
        {
            get { return idRespuestaNPC; }
            set { idRespuestaNPC = value; }
        }

        private List<PreguntaUsuarioBase> listaPreguntasUsuario;
        /// <summary>
        /// Lista de preguntas generadas por la respuesta del NPC
        /// </summary>
        /// <value>
        /// Lista generica de tipo PreguntaUsuarioBase
        /// </value>
        public List<PreguntaUsuarioBase> ListaPreguntasUsuario
        {
			get { return listaPreguntasUsuario; }
			set { listaPreguntasUsuario = value; }
        }

        private bool comprobacion;
        /// <summary>
        /// Valor para comprobar si la respuesta tiene accion posterior
        /// </summary>
        /// <value>
        ///   <c>true</c> si tiene accion posterior; sino, <c>false</c>.
        /// </value>
        public bool Comprobacion
        {
            get { return comprobacion; }
            set { comprobacion = value; }
        }

        private Localizaciones localizacionSeleccionada;
        /// <summary>
        /// Valor para saber si la respuesta desbloquea una localizacion
        /// </summary>
        /// <value>
        /// valor tipo enum Localizaciones
        /// </value>
        public Localizaciones LocalizacionSeleccionada
        {
            get { return localizacionSeleccionada; }
            set { localizacionSeleccionada = value; }
        }

        private bool sinRespuesta;
        /// <summary>
        /// Obtiene si la respuesta tiene preguntas asociadas.
        /// </summary>
        /// <value>
        ///   <c>true</c> si tiene preguntas asociadas; si es una respuesta ciega, <c>false</c>.
        /// </value>
        public bool SinRespuesta
        {
            get { return sinRespuesta; }
            set { sinRespuesta = value; }
        }

        private int direccionRespuesta;
        /// <summary>
        /// Id de la respuesta a mostrar.
        /// </summary>
        /// <value>
        /// valor tipo int con el valor del id de la respuesta
        /// </value>
        public int DireccionRespuesta
        {
            get { return direccionRespuesta; }
            set { direccionRespuesta = value; }
        }

		#endregion

		#region CONSTRUCTORES
        /// <summary>
        /// Constructor de la clase <see cref="RespuestaNPCBase"/>
        /// </summary>
        public RespuestaNPCBase()
        {
			Inicializar_Listas ();
        }

        /// <summary>
        /// Constructor de la Clase <see cref="RespuestaNPCBase"/>
        /// </summary>
        /// <param name="texto">texto de respuesta del NPC</param>
        public RespuestaNPCBase(string texto)
        {
            textoRespuesta = texto;
			Inicializar_Listas ();
        }

        /// <summary>
        /// Constructor de la clase <see cref="RespuestaNPCBase"/>
        /// </summary>
        /// <param name="id">id de la Respuesta</param>
        public RespuestaNPCBase(int id)
        {
            idRespuestaNPC = id;
			Inicializar_Listas();
        }

        /// <summary>
        /// Constructor de la clase <see cref="RespuestaNPCBase"/>
        /// </summary>
        /// <param name="preguntas">array de tipo Preguntas Base con respuestas a la pregunta</param>
        public RespuestaNPCBase(PreguntaUsuarioBase[] preguntas)
        {
			Inicializar_Listas ();

            AddPregunta(preguntas);
        }

        /// <summary>
        /// Constructor de la Clase<see cref="RespuestaNPCBase"/>
        /// </summary>
        /// <param name="texto">Texto de la respuesta del NPC</param>
        /// <param name="id">Id de la respuesta</param>
        /// <param name="preguntas">Array de tipo PreguntaUsuarioBase con respuesta a la pregunta del NPC</param>
        public RespuestaNPCBase(string texto, int id, PreguntaUsuarioBase[] preguntas)
        {
			Inicializar_Listas();

            textoRespuesta = texto;
            idRespuestaNPC = id;
            AddPregunta(preguntas);
        }

        /// <summary>
        /// Constructor de la Clase<see cref="RespuestaNPCBase"/>
        /// </summary>
        /// <param name="texto">Texto de la respuesta del NPC</param>
        /// <param name="id">Id de la respuesta</param>
        /// <param name="preguntas">Array de tipo PreguntaUsuarioBase con respuesta a la pregunta del NPC</param>
        /// <param name="comp">Valor booleano para la comprobacion si la respuesta tiene una accion posterior a su visualizacion</param>
        /// <param name="localizacion">valor tipo enum localizacion para desbloquear una Localizacion</param>
        public RespuestaNPCBase(string texto, int id, PreguntaUsuarioBase[] preguntas, bool comp, Localizaciones localizacion)
        {
			Inicializar_Listas ();

            textoRespuesta = texto;
            idRespuestaNPC = id;
            AddPregunta(preguntas);
            comprobacion = comp;
            localizacionSeleccionada = localizacion;

        }

        public RespuestaNPCBase(string texto, int id, PreguntaUsuarioBase[] preguntas, bool comp, Localizaciones localizacion, bool ciega, int direccion)
        {
			Inicializar_Listas ();

            textoRespuesta = texto;
            idRespuestaNPC = id;
            AddPregunta(preguntas);
            comprobacion = comp;
            localizacionSeleccionada = localizacion;
            sinRespuesta = ciega;
            direccionRespuesta = direccion;
        }

		#endregion

		#region METODOS

		/// <summary>
		/// Inicializa las Listas
		/// </summary>
		private void Inicializar_Listas()
		{
			listaPreguntasUsuario = new List<PreguntaUsuarioBase>();
		}

        /// <summary>
        /// Añade una pregunta a la respuesta del NPC
        /// </summary>
        /// <param name="pregunta">objeto tipo PreguntaUsuarioBase</param>
        public void AddPregunta(PreguntaUsuarioBase pregunta)
        {
            listaPreguntasUsuario.Add(pregunta);
        }

        /// <summary>
        /// Añade varias preguntas a la respuesta
        /// </summary>
        /// <param name="preguntas">Array de tipo PreguntaUsuarioBase</param>
        public void AddPregunta(PreguntaUsuarioBase[] preguntas)
        {
			listaPreguntasUsuario.AddRange(preguntas);
        }

        /// <summary>
        /// Borrar una pregunta
        /// </summary>
        /// <param name="pregunta">objeto tipo PreguntaUsuarioBase</param>
        public void BorrarPregunta(PreguntaUsuarioBase pregunta)
        {
			listaPreguntasUsuario.Remove(pregunta);
        }

        /// <summary>
        /// Borrar varias preguntas
        /// </summary>
        /// <param name="preguntas">Array de tipo PreguntaUsuarioBase</param>
        public void BorrarPregunta(PreguntaUsuarioBase[] preguntas)
        {
            foreach (PreguntaUsuarioBase pregunta in preguntas)
            {
				listaPreguntasUsuario.Remove(pregunta);
            }
        }

        /// <summary>
        /// Mostrar lista de preguntas a la respuesta del NPC
        /// </summary>
        /// <returns>Array de tipo PreguntaUsuarioBase</returns>
        public PreguntaUsuarioBase[] MostrarPreguntas()
        {
			return listaPreguntasUsuario.ToArray();
        }

		#endregion
    }
}
