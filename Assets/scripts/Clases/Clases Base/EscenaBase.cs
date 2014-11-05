using System;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base para datos de la escena actual
    /// </summary>
	[System.Serializable]
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

		private Escenas escenaNorte;
		/// <summary>
		/// Direccion de la salida Norte
		/// </summary>
		/// <value>
		/// valor tipo enum Escenas
		/// </value>
		public Escenas EscenaNorte
		{
			get { return escenaNorte; }
			set { escenaNorte = value; }
		}

		private Escenas escenaSur;
		/// <summary>
		/// Direccion de la salida Sur
		/// </summary>
		/// <value>
		/// valor tipo enum Escenas
		/// </value>
		public Escenas EscenaSur
		{
			get { return escenaSur; }
			set { escenaSur = value; }
		}

		private Escenas escenaEste;
		/// <summary>
		/// Direccion de la salida Este
		/// </summary>
		/// <value>
		/// valor tipo enum Escenas
		/// </value>
		public Escenas EscenaEste
		{
			get { return escenaEste; }
			set { escenaEste = value; }
		}

		private Escenas escenaOeste;
		/// <summary>
		/// Direccion de la salida Oeste
		/// </summary>
		/// <value>
		/// valor tipo enum Escenas
		/// </value>
		public Escenas EscenaOeste
		{
			get { return escenaOeste; }
			set { escenaOeste = value; }
		}

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        public EscenaBase()
        {
			Inicializar ();
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        /// <param name="nombre">string con el nombre de la escena</param>
        public EscenaBase(string nombre)
        {
			Inicializar ();

            this.nombre = nombre;
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        /// <param name="objetos">array de tipo ObjetoBase</param>
        public EscenaBase(ObjetoBase[] objetos)
        {
			Inicializar ();

            AddObjeto(objetos);
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        /// <param name="personajes">array de tipo PersonajeBase</param>
        public EscenaBase(PersonajeBase[] personajes)
        {
			Inicializar ();

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
			Inicializar ();

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

		/// <summary>
		/// Devuelve un objeto segun su nombre
		/// </summary>
		/// <param name="nombreObjeto">string de nombre de objeto</param>
		/// <returns>objeto tipo ObjetoBase</returns>
		public ObjetoBase Buscar_Objeto(string nombreObjeto)
		{
			foreach (ObjetoBase objeto in ObjetosEscena) 
			{
				if(objeto.Nombre.Contains(nombreObjeto))
				   return objeto;
			}

			return null;
		}

		/// <summary>
		/// Metodo de inicializacion en constructores
		/// </summary>
		public void Inicializar()
		{
			objetosEscena = new List<ObjetoBase>();
			personajesEscena = new List<PersonajeBase>();

			escenaNorte = Escenas.ninguna;
			escenaSur = Escenas.ninguna;
			escenaEste = Escenas.ninguna;
			escenaOeste = Escenas.ninguna;
		}
    }
}
