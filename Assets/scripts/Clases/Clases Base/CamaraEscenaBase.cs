﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base para datos de la escena actual
    /// </summary>
	[System.Serializable]
    public class CamaraEscenaBase
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

		private Escenas escena;
		public Escenas Escena
		{
			get { return escena; }
			set { escena = value; }
		}

		private string nombreEscena;
		/// <summary>
		/// Nombre descriptivo de la Escena
		/// </summary>
		/// <value>
		/// valor string del nombre descriptivo de la escena
		/// </value>
		public string NombreEscena
		{
			get { return nombreEscena; }
			set { nombreEscena = value; }
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

		private string escenaNorte;
		/// <summary>
		/// Direccion de la salida Norte
		/// </summary>
		/// <value>
		/// valor tipo enum Escenas
		/// </value>
		public string EscenaNorte
		{
			get { return escenaNorte; }
			set { escenaNorte = value; }
		}

		private string escenaSur;
		/// <summary>
		/// Direccion de la salida Sur
		/// </summary>
		/// <value>
		/// valor tipo enum Escenas
		/// </value>
		public string EscenaSur
		{
			get { return escenaSur; }
			set { escenaSur = value; }
		}

		private string escenaEste;
		/// <summary>
		/// Direccion de la salida Este
		/// </summary>
		/// <value>
		/// valor tipo enum Escenas
		/// </value>
		public string EscenaEste
		{
			get { return escenaEste; }
			set { escenaEste = value; }
		}

		private string escenaOeste;
		/// <summary>
		/// Direccion de la salida Oeste
		/// </summary>
		/// <value>
		/// valor tipo enum Escenas
		/// </value>
		public string EscenaOeste
		{
			get { return escenaOeste; }
			set { escenaOeste = value; }
		}

		private string escenaHabilitar;
		public string EscenaHabilitar
		{
			get { return escenaHabilitar; }
			set { escenaHabilitar = value; }
		}

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        public CamaraEscenaBase()
        {
			Inicializar ();
        }

        /// <summary>
        /// Constructor de la clase <see cref="EscenaBase"/> class.
        /// </summary>
        /// <param name="nombre">string con el nombre de la escena</param>
        public CamaraEscenaBase(string nombre)
        {
			Inicializar ();

            this.nombre = nombre;
        }

		/// <summary>
		/// Metodo de inicializacion en constructores
		/// </summary>
		public void Inicializar()
		{
			escenaNorte = "ninguna";
			escenaSur = "ninguna";
			escenaEste = "ninguna";
			escenaOeste = "ninguna";
			Escena = Escenas.ninguna;
		}
    }
}
