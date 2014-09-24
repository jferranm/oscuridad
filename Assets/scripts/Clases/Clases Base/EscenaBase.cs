using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oscuridad.Clases;

namespace Oscuridad.Clases
{
    public class EscenaBase
    {
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private List<ObjetoBase> objetosEscena;
        public List<ObjetoBase> ObjetosEscena
        {
            get { return objetosEscena; }
            set { objetosEscena = value; }
        }

        private List<PersonajeBase> personajesEscena;
        public List<PersonajeBase> PersonajesEscena
        {
            get { return personajesEscena; }
            set { personajesEscena = value; }
        }

        public EscenaBase()
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();
        }

        public EscenaBase(string nombre)
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();

            this.nombre = nombre;
        }

        public EscenaBase(ObjetoBase[] objetos)
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();

            AddObjeto(objetos);
        }

        public EscenaBase(PersonajeBase[] personajes)
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();

            AddPersonaje(personajes);
        }

        public EscenaBase(string nombre, ObjetoBase[] objetos, PersonajeBase[] personajes)
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();

            this.nombre = nombre;
            AddObjeto(objetos);
            AddPersonaje(personajes);
        }

        public void AddObjeto(ObjetoBase objeto)
        {
            objetosEscena.Add(objeto);
        }

        public void AddObjeto(ObjetoBase[] objetos)
        {
            objetosEscena.AddRange(objetos);
        }

        public void BorrarObjeto(ObjetoBase objeto)
        {
            objetosEscena.Remove(objeto);
        }

        public void BorrarObjeto(ObjetoBase[] objetos)
        {
            foreach (ObjetoBase objeto in objetos)
            {
                objetosEscena.Remove(objeto);
            }
        }

        public ObjetoBase[] MostrarObjeto()
        {
            return objetosEscena.ToArray();
        }

        public void AddPersonaje(PersonajeBase personaje)
        {
            personajesEscena.Add(personaje);
        }

        public void AddPersonaje(PersonajeBase[] personajes)
        {
            personajesEscena.AddRange(personajes);
        }

        public void BorrarPersonaje(PersonajeBase personaje)
        {
            personajesEscena.Remove(personaje);
        }

        public void BorrarPersonaje(PersonajeBase[] personajes)
        {
            foreach (PersonajeBase personaje in personajes)
            {
                personajesEscena.Remove(personaje);
            }
        }

        public PersonajeBase[] MostrarPersonaje()
        {
            return personajesEscena.ToArray();
        }
    }
}
