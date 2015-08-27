using System;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base de Preguntas asociadas a una RespuestaNPCBase del NPC
    /// </summary>
	[System.Serializable]
    public class PreguntaUsuarioBase
    {
		#region VARIABLES

        private string textoPregunta;
        /// <summary>
        /// Texto de la pregunta
        /// </summary>
        /// <value>
        /// cadena de texto con el valor de la pregunta
        /// </value>
        public string TextoPregunta
        {
            get { return textoPregunta; }
            set { textoPregunta = value; }
        }

        private int idRespuestaNPC;
        /// <summary>
        /// Identificador de la Respuesta donde va dirigida la pregunta tras ser seleccionada
        /// </summary>
        /// <value>
        /// valor entero de identificador
        /// </value>
        public int IdRespuestaNPC
        {
            get { return idRespuestaNPC; }
            set { idRespuestaNPC = value; }
        }

        private int idPreguntaUsuario;
        /// <summary>
        /// Identificador de pregunta sobre otras preguntas
        /// </summary>
        /// <value>
        /// Valor entero de identificador
        /// </value>
        public int IdPreguntaUsuario
        {
            get { return idPreguntaUsuario; }
            set { idPreguntaUsuario = value; }
        }

        private bool preguntaTirada;
        /// <summary>
        /// Identificar si la pregunta tiene tirada
        /// </summary>
        /// <value>
        ///   <c>true</c> si tiene tirada de dados; si no tiene tirada de dados, <c>false</c>.
        /// </value>
        public bool PreguntaTirada
        {
            get { return preguntaTirada; }
            set { preguntaTirada = value; }
        }
        
        private int idrespuestaAcierto;
        /// <summary>
        /// Identificador de la respuesta si a tenido acierto
        /// </summary>
        /// <value>
        /// Valor entero de identificador
        /// </value>
        public int IdRespuestaAcierto
        {
            get { return idrespuestaAcierto; }
            set { idrespuestaAcierto = value; }
        }

        private int idrespuestaFallo;
        /// <summary>
        /// Identificador de la respuesta si a tenido fallo
        /// </summary>
        /// <value>
        /// valor entero de identificador
        /// </value>
        public int IdRespuestaFallo
        {
            get { return idrespuestaFallo; }
            set { idrespuestaFallo = value; }
        }

        private bool comprobacionPregunta;
        /// <summary>
        /// campo para la consulta si la respuesta tiene tirada para mostrarse
        /// </summary>
        /// <value>
        ///   <c>true</c> si tiene tirada de dados; sino tiene tirada de dados, <c>false</c>.
        /// </value>
        public bool ComprobacionPregunta
        {
            get { return comprobacionPregunta; }
            set { comprobacionPregunta = value; }
        }

        private Habilidades comprobacionHabilidad;
        /// <summary>
        /// Si el campo es null no tendria tirada de habilidades
        /// </summary>
        /// <value>
        /// valor tipo enum Habilidades
        /// </value>
        public Habilidades ComprobacionHabilidad
        {
            get { return comprobacionHabilidad; }
            set { comprobacionHabilidad = value; }
        }

        private Escenas comprobacionEscenas;
        /// <summary>
        /// Si tiene valor null no tendra tira de escenas visitadas
        /// </summary>
        /// <value>
        /// valor tipo enum Escenas
        /// </value>
        public Escenas ComprobacionEscenas
        {
            get { return comprobacionEscenas; }
            set { comprobacionEscenas = value; }
        }

        private Interactuables comprobacionInteractuables;
        /// <summary>
        /// Si tiene valor null no tendra tirada de interactuables vistos
        /// </summary>
        /// <value>
		/// valor tipo enum Interactuables
        /// </value>
		public Interactuables ComprobacionInteractuables
        {
			get { return comprobacionInteractuables; }
			set { comprobacionInteractuables = value; }
        }

        private Acciones comprobacionAccion;
        /// <summary>
        /// Obtiene una accion para la comprobacion
        /// </summary>
        /// <value>
        /// valor tipo enum Acciones
        /// </value>
        public Acciones ComprobacionAccion
        {
            get { return comprobacionAccion; }
            set { comprobacionAccion = value; }
        }

		#endregion

		#region CONSTRUCTORES

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaUsuarioBase"/>
        /// </summary>
        public PreguntaUsuarioBase()
        {
        }

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaUsuarioBase"/>
        /// </summary>
        /// <param name="texto">cadena de texto del enunciado de la pregunta</param>
		public PreguntaUsuarioBase(string texto)
        {
            textoPregunta = texto;
        }

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaUsuarioBase"/>
        /// </summary>
        /// <param name="id">identificador de la pregunta</param>
		public PreguntaUsuarioBase(int id)
        {
            idPreguntaUsuario = id;
        }

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaUsuarioBase"/>
        /// </summary>
        /// <param name="texto">cadena de texto con el enunciado de la respuesta</param>
        /// <param name="id">identificador de la respuesta</param>
        /// <param name="habilidad">valor tipo enum Habilidad</param>
		public PreguntaUsuarioBase(string texto, int id)
        {
            textoPregunta = texto;
            idPreguntaUsuario = id;
        }

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaUsuarioBase" />
        /// </summary>
        /// <param name="texto">cadena de texto con el enunciado de la respuesta</param>
        /// <param name="id">identificador de la respuesta</param>
        /// <param name="comprobacion">Si tiene tirada de comprobacion <c>true</c> sino</param>
        /// <param name="habilidad">valor tipo enum Habilidad</param>
        /// <param name="escena">valor tipo enum Escenas</param>
		/// <param name="objeto">valor tipo enum Interactuables</param>
        /// <param name="accion">valor tipo enum Acciones</param>
		public PreguntaUsuarioBase(string texto, int id, bool comprobacion, Habilidades habilidad, Escenas escena, Interactuables objeto, Acciones accion)
        {
            textoPregunta = texto;
            idPreguntaUsuario = id;
            comprobacionPregunta = comprobacion;
            comprobacionHabilidad = habilidad;
			comprobacionInteractuables = objeto;
            comprobacionEscenas = escena;
            comprobacionAccion = accion;
        }

		#endregion                  
    }
}
