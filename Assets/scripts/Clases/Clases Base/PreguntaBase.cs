using System;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base de Preguntas asociadas a una RespuestaBase del NPC
    /// </summary>
	[System.Serializable]
    public class PreguntaBase
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

        private int idRespuesta;
        /// <summary>
        /// Identificador de la Respuesta donde va dirigida la pregunta tras ser seleccionada
        /// </summary>
        /// <value>
        /// valor entero de identificador
        /// </value>
        public int IdRespuesta
        {
            get { return idRespuesta; }
            set { idRespuesta = value; }
        }

        private int idPregunta;
        /// <summary>
        /// Identificador de pregunta sobre otras preguntas
        /// </summary>
        /// <value>
        /// Valor entero de identificador
        /// </value>
        public int IdPregunta
        {
            get { return idPregunta; }
            set { idPregunta = value; }
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

        private Objetos comprobacionObjetos;
        /// <summary>
        /// Si tiene valor null no tendra tirada de objetos vistos
        /// </summary>
        /// <value>
        /// valor tipo enum Objetos
        /// </value>
        public Objetos ComprobacionObjetos
        {
            get { return comprobacionObjetos; }
            set { comprobacionObjetos = value; }
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
        /// Constructor de la Clase <see cref="PreguntaBase"/>
        /// </summary>
        public PreguntaBase()
        {
        }

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaBase"/>
        /// </summary>
        /// <param name="texto">cadena de texto del enunciado de la pregunta</param>
        public PreguntaBase(string texto)
        {
            textoPregunta = texto;
        }

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaBase"/>
        /// </summary>
        /// <param name="id">identificador de la pregunta</param>
        public PreguntaBase(int id)
        {
            idPregunta = id;
        }

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaBase"/>
        /// </summary>
        /// <param name="texto">cadena de texto con el enunciado de la respuesta</param>
        /// <param name="id">identificador de la respuesta</param>
        /// <param name="habilidad">valor tipo enum Habilidad</param>
        public PreguntaBase(string texto, int id)
        {
            textoPregunta = texto;
            idPregunta = id;
        }

        /// <summary>
        /// Constructor de la Clase <see cref="PreguntaBase" />
        /// </summary>
        /// <param name="texto">cadena de texto con el enunciado de la respuesta</param>
        /// <param name="id">identificador de la respuesta</param>
        /// <param name="comprobacion">Si tiene tirada de comprobacion <c>true</c> sino</param>
        /// <param name="habilidad">valor tipo enum Habilidad</param>
        /// <param name="escena">valor tipo enum Escenas</param>
        /// <param name="objeto">valor tipo enum Objetos</param>
        /// <param name="accion">valor tipo enum Acciones</param>
        public PreguntaBase(string texto, int id, bool comprobacion, Habilidades habilidad, Escenas escena, Objetos objeto, Acciones accion)
        {
            textoPregunta = texto;
            idPregunta = id;
            comprobacionPregunta = comprobacion;
            comprobacionHabilidad = habilidad;
            comprobacionObjetos = objeto;
            comprobacionEscenas = escena;
            comprobacionAccion = accion;
        }

		#endregion                  
    }
}
