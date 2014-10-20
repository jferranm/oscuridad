using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializadorXMLOscuridad
{
    /// <summary>
    /// Clase Base para el Personaje interactuable en la escena
    /// </summary>
    [Serializable()]
    public class PersonajeBase
    {
        
        private string nombre;
        /// <summary>
        /// Nombre del personaje
        /// </summary>
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private List<opcionObjeto> personajeOpciones;
        /// <summary>
        /// lista de opcion de tipo enumeracion para las opciones que se pueden hacer con el personaje
        /// </summary>
        /// <value>
        /// lista generica de opciones de objeto
        /// </value>
        public List<opcionObjeto> PersonajeOpciones
        {
            get { return personajeOpciones; }
            set { personajeOpciones = value; }
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


        //METODOS

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public PersonajeBase()
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<RespuestaBase>();
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="nombre">nombre del personaje</param>
        public PersonajeBase(string nombre)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<RespuestaBase>();

            this.nombre = nombre;
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="opciones">array de tipo enum de opciones de interaccion de personaje</param>
        public PersonajeBase(opcionObjeto[] opciones)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<RespuestaBase>();

            AddOpcion(opciones);
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="respuestas">array de tipo RespuestaBase de respuestas a mostrar del personaje</param>
        public PersonajeBase(RespuestaBase[] respuestas)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<RespuestaBase>();

            AddRespuesta(respuestas);
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="nombre">nombre del personaje</param>
        /// <param name="opciones">array de tipo enum de opciones de interaccion de personaje</param>
        /// <param name="respuestas">array de tipo RespuestaBase de respuestas a mostrar del personaje</param>
        public PersonajeBase(string nombre, opcionObjeto[] opciones, RespuestaBase[] respuestas)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<RespuestaBase>();

            this.nombre = nombre;
            AddOpcion(opciones);
            AddRespuesta(respuestas);
        }

        /// <summary>
        /// Constructor sobrecargado de la Clase
        /// </summary>
        /// <param name="nombre">nombre del personaje</param>
        /// <param name="opciones">array de tipo enum de opciones de interaccion de personaje</param>
        /// <param name="respuestas">array de tipo RespuestaBase de respuestas a mostrar del personaje</param>
        /// <param name="idComienzo">valor entero que marca cual sera la RespuestaBase donde comenzara la conversacion</param>
        public PersonajeBase(string nombre, opcionObjeto[] opciones, RespuestaBase[] respuestas, int idComienzo)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<RespuestaBase>();

            this.nombre = nombre;
            inicioConversacion = idComienzo;
            AddOpcion(opciones);
            AddRespuesta(respuestas);
        }

        /// <summary>
        /// Añade una opcion
        /// </summary>
        /// <param name="opcion">opcion tipo enum opcionObjeto</param>
        public void AddOpcion(opcionObjeto opcion)
        {
            personajeOpciones.Add(opcion);
        }

        /// <summary>
        /// Añade array de opciones
        /// </summary>
        /// <param name="opciones">array de tipo enum de opciones de interaccion de personaje</param>
        public void AddOpcion(opcionObjeto[] opciones)
        {
            personajeOpciones.AddRange(opciones);
        }

        /// <summary>
        /// Borra una opcion
        /// </summary>
        /// <param name="opcion">opcion tipo enum opcionObjeto</param>
        public void BorrarOpcion(opcionObjeto opcion)
        {
            personajeOpciones.Remove(opcion);
        }

        /// <summary>
        /// Borra array de opciones
        /// </summary>
        /// <param name="opciones">array de tipo enum de opciones de interaccion de personaje</param>
        public void BorrarOpcion(opcionObjeto[] opciones)
        {
            foreach (opcionObjeto opcion in opciones)
            {
                personajeOpciones.Remove(opcion);
            }
        }

        /// <summary>
        /// Muesta la opciones del personaje
        /// </summary>
        /// <returns>
        /// array de tipo enum de opcionObjeto
        /// </returns>
        public opcionObjeto[] MostrarOpciones()
        {
            return personajeOpciones.ToArray();
        }

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
    }
}
