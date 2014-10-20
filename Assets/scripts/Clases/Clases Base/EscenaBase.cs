using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializadorXMLOscuridad
{
    /// <summary>
    /// Clase Base para datos de la escena actual
    /// </summary>
    [Serializable()]
    public class EscenaBase
    {
        private string nombre;
        /// <summary>
        /// Nombre de la Escena
        /// </summary>
        /// <value>
        /// valor string del nombre de la escena
        /// </value>
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string descripcion;
        /// <summary>
        /// Descripcion de la escena
        /// </summary>
        /// <value>
        /// valor string de la descripcion de la escena
        /// </value>
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private List<ObjetoBase> objetosEscena;
        /// <summary>
        /// Lista de objetos en la escena
        /// </summary>
        /// <value>
        /// lista generica de tipo ObjetoBase
        /// </value>
        public List<ObjetoBase> ObjetosEscena
        {
            get { return objetosEscena; }
            set { objetosEscena = value; }
        }

        private List<PersonajeBase> personajesEscena;
        /// <summary>
        /// Lista de personajes en la escena
        /// </summary>
        /// <value>
        /// lista generica de tipo PersonajeBase
        /// </value>
        public List<PersonajeBase> PersonajesEscena
        {
            get { return personajesEscena; }
            set { personajesEscena = value; }
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        public EscenaBase()
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        /// <param name="nombre">string con el nombre de la escena</param>
        public EscenaBase(string nombre)
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();

            this.nombre = nombre;
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        /// <param name="objetos">array de tipo ObjetoBase</param>
        public EscenaBase(ObjetoBase[] objetos)
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();

            AddObjeto(objetos);
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        /// <param name="personajes">array de tipo PersonajeBase</param>
        public EscenaBase(PersonajeBase[] personajes)
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();

            AddPersonaje(personajes);
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        /// <param name="nombre">string de nombre de escena</param>
        /// <param name="objetos">array de tipo ObjetoBase</param>
        /// <param name="personajes">array de tipo PersonajeBase</param>
        public EscenaBase(string nombre, ObjetoBase[] objetos, PersonajeBase[] personajes)
        {
            objetosEscena = new List<ObjetoBase>();
            personajesEscena = new List<PersonajeBase>();

            this.nombre = nombre;
            AddObjeto(objetos);
            AddPersonaje(personajes);
        }

        /// <summary>
        /// Añade un objeto a la escena
        /// </summary>
        /// <param name="objeto">objeto tipo ObjetoBase</param>
        public void AddObjeto(ObjetoBase objeto)
        {
            objetosEscena.Add(objeto);
        }

        /// <summary>
        /// Añade varios objetos a la escena
        /// </summary>
        /// <param name="objetos">array de tipo ObjetoBase</param>
        public void AddObjeto(ObjetoBase[] objetos)
        {
            objetosEscena.AddRange(objetos);
        }

        /// <summary>
        /// Borra el objeto de la escena
        /// </summary>
        /// <param name="objeto">objeto tipo ObjetoBase</param>
        public void BorrarObjeto(ObjetoBase objeto)
        {
            objetosEscena.Remove(objeto);
        }

        /// <summary>
        /// Borra varios objetos de la escena
        /// </summary>
        /// <param name="objetos">array de tipo ObjetoBase</param>
        public void BorrarObjeto(ObjetoBase[] objetos)
        {
            foreach (ObjetoBase objeto in objetos)
            {
                objetosEscena.Remove(objeto);
            }
        }

        /// <summary>
        /// Mostrar lista de objetos
        /// </summary>
        /// <returns>array de tipo ObjetoBase</returns>
        public ObjetoBase[] MostrarObjeto()
        {
            return objetosEscena.ToArray();
        }

        /// <summary>
        /// Añade un personaje a la escena
        /// </summary>
        /// <param name="personaje">objeto tipo PersonajeBase</param>
        public void AddPersonaje(PersonajeBase personaje)
        {
            personajesEscena.Add(personaje);
        }

        /// <summary>
        /// Añade varios personajes a la escena
        /// </summary>
        /// <param name="personajes">array de tipo PersonajeBase</param>
        public void AddPersonaje(PersonajeBase[] personajes)
        {
            personajesEscena.AddRange(personajes);
        }

        /// <summary>
        /// Borra un personaje de la escena
        /// </summary>
        /// <param name="personaje">objeto tipo PersonajeBase</param>
        public void BorrarPersonaje(PersonajeBase personaje)
        {
            personajesEscena.Remove(personaje);
        }

        /// <summary>
        /// Borra varios personajes de la escena
        /// </summary>
        /// <param name="personajes">array de tipo PersonajeBase</param>
        public void BorrarPersonaje(PersonajeBase[] personajes)
        {
            foreach (PersonajeBase personaje in personajes)
            {
                personajesEscena.Remove(personaje);
            }
        }

        /// <summary>
        /// Lista de personajes en la escena
        /// </summary>
        /// <returns>array de tipo PersonajeBase</returns>
        public PersonajeBase[] MostrarPersonaje()
        {
            return personajesEscena.ToArray();
        }
    }
}
