using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
		#region VARIABLES
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

        private List<InteractuableGenerico> interactuablesEscena;
        /// <summary>
        /// Lista de interactuables en la escena
        /// </summary>
        /// <value>
        /// lista generica de tipo InteractuableGenerico
        /// </value>
		public List<InteractuableGenerico> InteractuablesEscena
        {
			get { return interactuablesEscena; }
			set { interactuablesEscena = value; }
        }

		private List<InteractuableSinZoomGenerico> interactuablesSinZoomEscena;
		/// <summary>
		/// Lista de interactuables sin zoom en la escena
		/// </summary>
		/// <value>
		/// lista generica de tipo InteractuableSinZoomGenerico
		/// </value>
		public List<InteractuableSinZoomGenerico> InteractuablesSinZoomEscena
		{
			get { return interactuablesSinZoomEscena; }
			set { interactuablesSinZoomEscena = value; }
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
		#endregion

		#region CONSTRUCTORES
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
        /// <param name="objetos">array de tipo InteractuableGenerico</param>
		public EscenaBase(InteractuableGenerico[] objetos)
        {
			Inicializar ();

            AddObjeto(objetos);
        }

		#endregion

		#region METODOS
        /// <summary>
        /// Añade un interactuable a la escena
        /// </summary>
		/// <param name="objeto">objeto tipo InteractuableGenerico</param>
        public void AddObjeto(InteractuableGenerico objeto)
        {
            interactuablesEscena.Add(objeto);
        }

        /// <summary>
        /// Añade varios interactuables a la escena
        /// </summary>
		/// <param name="objetos">array de tipo InteractuableGenerico</param>
		public void AddObjeto(InteractuableGenerico[] objetos)
        {
            interactuablesEscena.AddRange(objetos);
        }

        /// <summary>
        /// Borra el interactuable de la escena
        /// </summary>
		/// <param name="objeto">objeto tipo InteractuableGenerico</param>
		public void BorrarObjeto(InteractuableGenerico objeto)
        {
            interactuablesEscena.Remove(objeto);
        }

        /// <summary>
        /// Borra varios interactuables de la escena
        /// </summary>
		/// <param name="objetos">array de tipo InteractuableGenerico</param>
		public void BorrarObjeto(InteractuableGenerico[] objetos)
        {
			foreach (InteractuableGenerico objeto in objetos)
            {
                interactuablesEscena.Remove(objeto);
            }
        }

        /// <summary>
        /// Mostrar lista de interactuables
        /// </summary>
		/// <returns>array de tipo InteractuableGenerico</returns>
		public InteractuableGenerico[] MostrarObjeto()
        {
            return interactuablesEscena.ToArray();
        }

		/// <summary>
		/// Devuelve un interactuable segun su nombre
		/// </summary>
		/// <param name="nombreInteractuable">string de nombre de interactuable</param>
		/// <returns>objeto tipo InteractuableGenerico</returns>
		public InteractuableGenerico Buscar_Interactuable(string nombreInteractuable)
		{
			return interactuablesEscena.Find (x => x.Nombre == nombreInteractuable);
		}

		/// <summary>
		/// Añade un interactuable sin zoom a la escena
		/// </summary>
		/// <param name="objeto">objeto tipo InteractuableSinZoomGenerico</param>
		public void AddObjeto(InteractuableSinZoomGenerico objeto)
		{
			interactuablesSinZoomEscena.Add(objeto);
		}
		
		/// <summary>
		/// Añade varios interactuables sin zoom a la escena
		/// </summary>
		/// <param name="objetos">array de tipo InteractuableGenerico</param>
		public void AddObjeto(InteractuableSinZoomGenerico[] objetos)
		{
			interactuablesSinZoomEscena.AddRange(objetos);
		}
		
		/// <summary>
		/// Borra el interactuable sin zoom de la escena
		/// </summary>
		/// <param name="objeto">objeto tipo InteractuableSinZoomGenerico</param>
		public void BorrarObjeto(InteractuableSinZoomGenerico objeto)
		{
			interactuablesSinZoomEscena.Remove(objeto);
		}
		
		/// <summary>
		/// Borra varios interactuables sin zoom de la escena
		/// </summary>
		/// <param name="objetos">array de tipo InteractuableSinZoomGenerico</param>
		public void BorrarObjeto(InteractuableSinZoomGenerico[] objetos)
		{
			foreach (InteractuableSinZoomGenerico objeto in objetos)
			{
				interactuablesSinZoomEscena.Remove(objeto);
			}
		}
		
		/// <summary>
		/// Mostrar lista de interactuables sin zoom
		/// </summary>
		/// <returns>array de tipo InteractuableSinZoomGenerico</returns>
		public InteractuableSinZoomGenerico[] MostrarObjetoSinZoom()
		{
			return interactuablesSinZoomEscena.ToArray();
		}

		/// <summary>
		/// Mostrar lista de interactuables sin zoom de un solo uso
		/// </summary>
		/// <returns>array de tipo InteractuableSinZoomGenerico</returns>
		public InteractuableSinZoomGenerico[] MostrarObjetoSinZoomFiltrado()
		{
			return interactuablesSinZoomEscena.FindAll (x => x.EjecutarAnimacion == false).ToArray();
		}
		
		/// <summary>
		/// Devuelve un interactuable sin zoom segun su nombre
		/// </summary>
		/// <param name="nombreInteractuable">string de nombre de interactuable sin Zoom</param>
		/// <returns>objeto tipo InteractuableSinZoomGenerico</returns>
		public InteractuableSinZoomGenerico Buscar_InteractuableSinZoom(string nombreInteractuablesinZoom)
		{
			return interactuablesSinZoomEscena.Find (x => x.Nombre == nombreInteractuablesinZoom);
		}


		/// <summary>
		/// Devuelve una camara segun su nombre
		/// </summary>
		/// <param name="nombreCamara">string de nombre de camara</param>
		/// <returns>objeto tipo CamaraEscenaBase</returns>
		public CamaraEscenaBase Buscar_Camara(string nombreCamara)
		{
			return listaCamaras.Find (x => x.Nombre == nombreCamara);
		}

		/// <summary>
		/// Metodo de inicializacion en constructores
		/// </summary>
		public void Inicializar()
		{
			interactuablesEscena = new List<InteractuableGenerico>();
			listaCamaras = new List<CamaraEscenaBase> ();
			escenasDeshabilitar = new List<string> ();
		}

		#endregion
    }
}
