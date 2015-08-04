using System;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base para Respuesta del NPC en una conversacion
    /// </summary>
	[System.Serializable]
    public class RespuestaBase
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

        private int idRespuesta;
        /// <summary>
        /// Id de la respuesta del NPC
        /// </summary>
        /// <value>
        /// valor entero para diferenciar respuestas
        /// </value>
        public int IdRespuesta
        {
            get { return idRespuesta; }
            set { idRespuesta = value; }
        }

        private List<PreguntaBase> preguntasRespuesta;
        /// <summary>
        /// Lista de preguntas generadas por la respuesta del NPC
        /// </summary>
        /// <value>
        /// Lista generica de tipo PreguntaBase
        /// </value>
        public List<PreguntaBase> PreguntaRespuesta
        {
            get { return preguntasRespuesta; }
            set { preguntasRespuesta = value; }
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
        /// Constructor de la clase <see cref="RespuestaBase"/>
        /// </summary>
        public RespuestaBase()
        {
            preguntasRespuesta = new List<PreguntaBase>();
            PreguntaRespuesta = new List<PreguntaBase>();
        }

        /// <summary>
        /// Constructor de la Clase <see cref="RespuestaBase"/>
        /// </summary>
        /// <param name="texto">texto de respuesta del NPC</param>
        public RespuestaBase(string texto)
        {
            textoRespuesta = texto;
            preguntasRespuesta = new List<PreguntaBase>();
            PreguntaRespuesta = new List<PreguntaBase>();
        }

        /// <summary>
        /// Constructor de la clase <see cref="RespuestaBase"/>
        /// </summary>
        /// <param name="id">id de la Respuesta</param>
        public RespuestaBase(int id)
        {
            idRespuesta = id;
            preguntasRespuesta = new List<PreguntaBase>();
            PreguntaRespuesta = new List<PreguntaBase>();
        }

        /// <summary>
        /// Constructor de la clase <see cref="RespuestaBase"/>
        /// </summary>
        /// <param name="preguntas">array de tipo Preguntas Base con respuestas a la pregunta</param>
        public RespuestaBase(PreguntaBase[] preguntas)
        {
            preguntasRespuesta = new List<PreguntaBase>();
            PreguntaRespuesta = new List<PreguntaBase>();

            AddPregunta(preguntas);
        }

        /// <summary>
        /// Constructor de la Clase<see cref="RespuestaBase"/>
        /// </summary>
        /// <param name="texto">Texto de la respuesta del NPC</param>
        /// <param name="id">Id de la respuesta</param>
        /// <param name="preguntas">Array de tipo PreguntaBase con respuesta a la pregunta del NPC</param>
        public RespuestaBase(string texto, int id, PreguntaBase[] preguntas)
        {
            preguntasRespuesta = new List<PreguntaBase>();
            PreguntaRespuesta = new List<PreguntaBase>();

            textoRespuesta = texto;
            idRespuesta = id;
            AddPregunta(preguntas);
        }

        /// <summary>
        /// Constructor de la Clase<see cref="RespuestaBase"/>
        /// </summary>
        /// <param name="texto">Texto de la respuesta del NPC</param>
        /// <param name="id">Id de la respuesta</param>
        /// <param name="preguntas">Array de tipo PreguntaBase con respuesta a la pregunta del NPC</param>
        /// <param name="comp">Valor booleano para la comprobacion si la respuesta tiene una accion posterior a su visualizacion</param>
        /// <param name="localizacion">valor tipo enum localizacion para desbloquear una Localizacion</param>
        public RespuestaBase(string texto, int id, PreguntaBase[] preguntas, bool comp, Localizaciones localizacion)
        {
            preguntasRespuesta = new List<PreguntaBase>();
            PreguntaRespuesta = new List<PreguntaBase>();

            textoRespuesta = texto;
            idRespuesta = id;
            AddPregunta(preguntas);
            comprobacion = comp;
            localizacionSeleccionada = localizacion;

        }

        public RespuestaBase(string texto, int id, PreguntaBase[] preguntas, bool comp, Localizaciones localizacion, bool ciega, int direccion)
        {
            preguntasRespuesta = new List<PreguntaBase>();
            PreguntaRespuesta = new List<PreguntaBase>();

            textoRespuesta = texto;
            idRespuesta = id;
            AddPregunta(preguntas);
            comprobacion = comp;
            localizacionSeleccionada = localizacion;
            sinRespuesta = ciega;
            direccionRespuesta = direccion;
        }

		#endregion

		#region METODOS

        /// <summary>
        /// Añade una pregunta a la respuesta del NPC
        /// </summary>
        /// <param name="pregunta">objeto tipo PreguntaBase</param>
        public void AddPregunta(PreguntaBase pregunta)
        {
            preguntasRespuesta.Add(pregunta);
        }

        /// <summary>
        /// Añade varias preguntas a la respuesta
        /// </summary>
        /// <param name="preguntas">Array de tipo PreguntaBase</param>
        public void AddPregunta(PreguntaBase[] preguntas)
        {
            preguntasRespuesta.AddRange(preguntas);
        }

        /// <summary>
        /// Borrar una pregunta
        /// </summary>
        /// <param name="pregunta">objeto tipo PreguntaBase</param>
        public void BorrarPregunta(PreguntaBase pregunta)
        {
            preguntasRespuesta.Remove(pregunta);
        }

        /// <summary>
        /// Borrar varias preguntas
        /// </summary>
        /// <param name="preguntas">Array de tipo PreguntaBase</param>
        public void BorrarPregunta(PreguntaBase[] preguntas)
        {
            foreach (PreguntaBase pregunta in preguntas)
            {
                preguntasRespuesta.Remove(pregunta);
            }
        }

        /// <summary>
        /// Mostrar lista de preguntas a la respuesta del NPC
        /// </summary>
        /// <returns>Array de tipo PreguntaBase</returns>
        public PreguntaBase[] MostrarPreguntas()
        {
            return preguntasRespuesta.ToArray();
        }

		#endregion
    }
}
