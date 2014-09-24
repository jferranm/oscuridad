using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    public class PersonajeBase
    {
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private List<opcionObjeto> personajeOpciones;
        public List<opcionObjeto> PersonajeOpciones
        {
            get { return personajeOpciones; }
            set { personajeOpciones = value; }
        }

        private List<PreguntaBase> conversacionPersonaje;
        public List<PreguntaBase> ConversacionPersonaje
        {
            get { return conversacionPersonaje; }
            set { conversacionPersonaje = value; }
        }

        public PersonajeBase()
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<PreguntaBase>();
        }

        public PersonajeBase(string nombre)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<PreguntaBase>();

            this.nombre = nombre;
        }

        public PersonajeBase(opcionObjeto[] opciones)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<PreguntaBase>();

            AddOpcion(opciones);
        }

        public PersonajeBase(PreguntaBase[] preguntas)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<PreguntaBase>();

            AddPregunta(preguntas);
        }

        public PersonajeBase(string nombre, opcionObjeto[] opciones, PreguntaBase[] preguntas)
        {
            personajeOpciones = new List<opcionObjeto>();
            conversacionPersonaje = new List<PreguntaBase>();

            this.nombre = nombre;
            AddOpcion(opciones);
            AddPregunta(preguntas);
        }

        public void AddOpcion(opcionObjeto opcion)
        {
            personajeOpciones.Add(opcion);
        }

        public void AddOpcion(opcionObjeto[] opciones)
        {
            personajeOpciones.AddRange(opciones);
        }

        public void BorrarOpcion(opcionObjeto opcion)
        {
            personajeOpciones.Remove(opcion);
        }

        public void BorrarOpcion(opcionObjeto[] opciones)
        {
            foreach (opcionObjeto opcion in opciones)
            {
                personajeOpciones.Remove(opcion);
            }
        }

        public opcionObjeto[] MostrarOpciones()
        {
            return personajeOpciones.ToArray();
        }

        public void AddPregunta(PreguntaBase pregunta)
        {
            conversacionPersonaje.Add(pregunta);
        }

        public void AddPregunta(PreguntaBase[] preguntas)
        {
            conversacionPersonaje.AddRange(preguntas);
        }

        public void BorrarPregunta(PreguntaBase pregunta)
        {
            conversacionPersonaje.Remove(pregunta);
        }

        public void BorrarPregunta(PreguntaBase[] preguntas)
        {
            foreach (PreguntaBase pregunta in preguntas)
            {
                conversacionPersonaje.Remove(pregunta);
            }
        }

        public PreguntaBase[] MostrarPreguntas()
        {
            return conversacionPersonaje.ToArray();
        }
    }
}
