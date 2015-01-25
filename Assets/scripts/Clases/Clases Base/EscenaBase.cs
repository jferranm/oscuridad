using System;
using System.Collections.Generic;
using UnityEngine;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base para datos de la escena actual
    /// </summary>
	[System.Serializable]
    public class EscenaBase
    {
		private Escenas escena;
		/// <summary>
		/// Escena en Enumeracion
		/// </summary>
		/// <value>
		/// valor de tipo enum Escenas
		/// </value>
		public Escenas Escena
		{
			get { return escena; }
			set { escena = value; }
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

		private List<CamaraEscenaBase> listaCamaras;
		public List<CamaraEscenaBase> ListaCamaras
		{
			get { return listaCamaras; }
			set { listaCamaras = value; }
		}

		private CamaraEscenaBase camaraInicio;
		public CamaraEscenaBase CamaraInicio
		{
			get { return camaraInicio;}
			set { camaraInicio = value; }
		}

		private List<string> escenasDeshabilitar;
		public List<string> EscenasDeshabilitar
		{
			get { return escenasDeshabilitar; }
			set { escenasDeshabilitar = value; }
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
			foreach (ObjetoBase objeto in objetosEscena) 
			{
				if(objeto.Nombre.Contains(nombreObjeto))
				   return objeto;
			}

			return null;
		}

		/// <summary>
		/// Devuelve un personaje segun su nombre
		/// </summary>
		/// <param name="nombrePersonaje">string de nombre de personaje</param>
		/// <returns>objeto tipo ObjetoBase</returns>
		public PersonajeBase Buscar_Personaje(string nombrePersonaje)
		{
			foreach (PersonajeBase personaje in personajesEscena) 
			{
				if(personaje.Nombre.Contains(nombrePersonaje))
					return personaje;
			}
			
			return null;
		}

		public CamaraEscenaBase Buscar_Camara(string nombreCamara)
		{
			foreach (CamaraEscenaBase camara in ListaCamaras) 
			{
				if(camara.Nombre.Contains(nombreCamara))
					return camara;
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
			listaCamaras = new List<CamaraEscenaBase> ();
			escenasDeshabilitar = new List<string> ();
		}
    }
}
