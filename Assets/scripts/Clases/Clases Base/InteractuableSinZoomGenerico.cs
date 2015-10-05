using UnityEngine;
using System;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase base para los Objetos/Personajes interactuables sin Zoom
    /// </summary>
	[System.Serializable]
    public class InteractuableSinZoomGenerico
    {
		#region VARIABLES
        private string nombre;
        /// <summary>
        /// Nombre del Interactuable
        /// </summary>
        /// <value>
        /// string de nombre de Interactuable
        /// </value>
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

		private bool ejecutarAnimacion;
		/// <summary>
		/// booleano para objetos de un solo uso de animacion
		/// </summary>
		/// <value>
		/// true se ejecuta la animacion, false se mantiene en posicion de fin de animacion
		/// </value>
		public bool EjecutarAnimacion
		{
			get { return ejecutarAnimacion; }
			set { ejecutarAnimacion = value; }
		}

		#endregion

		#region CONSTRUCTORES

        /// <summary>
        /// Contructor de la clase <see cref="InteractuableGenerico"/>.
        /// </summary>
		public InteractuableSinZoomGenerico()
        {
        }

        /// <summary>
        /// Constructor de la clase <see cref="InteractuableGenerico"/>
        /// </summary>
        /// <param name="nombre">string de nombre del Interactuable</param>
		public InteractuableSinZoomGenerico(string nombre)
        {
            this.nombre = nombre;
        }

		#endregion
	}
}
