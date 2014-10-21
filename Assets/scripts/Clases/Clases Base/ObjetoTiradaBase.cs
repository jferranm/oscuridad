using System;
using System.Collections.Generic;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase Base para Descripcion con Tirada de dados
    /// </summary>
	[System.Serializable]
    public class ObjetoTiradaBase
    {
        private string textoDescriptivo;
        /// <summary>
        /// texto de la descripcion del Objeto
        /// </summary>
        /// <value>
        /// cadena descriptiva del objeto
        /// </value>
        public string TextoDescriptivo
        {
            get { return textoDescriptivo; }
            set { textoDescriptivo = value; }
        }

        private Habilidades habilidadTirada;
        /// <summary>
        /// Habilidad para la tirada en la descripcion
        /// </summary>
        /// <value>
        /// enum de tipo Habilidades
        /// </value>
        public Habilidades HabilidadTirada
        {
            get { return habilidadTirada; }
            set { habilidadTirada = value; }
        }

        private bool comprobacion;
        /// <summary>
        /// Valor para saber si hay una comprobacion para mostrar la descripcion.
        /// </summary>
        /// <value>
        ///   <c>true</c> si hay comprobacion; sino hay comprobacion, <c>false</c>.
        /// </value>
        public bool Comprobacion
        {
            get { return comprobacion; }
            set { comprobacion = value; }
        }

        private Escenas escenaComprobacion;
        /// <summary>
        /// Obtiene la escena de comprobacion
        /// </summary>
        /// <value>
        /// valor tipo enum Escenas
        /// </value>
        public Escenas EscenaComprobacion
        {
            get { return escenaComprobacion; }
            set { escenaComprobacion = value; }
        }

        private Objetos objetoComprobacion;
        /// <summary>
        /// Obtiene el objeto para la comprobacion
        /// </summary>
        /// <value>
        /// valor tipo enum Objetos
        /// </value>
        public Objetos ObjetoComprobacion
        {
            get { return objetoComprobacion; }
            set { objetoComprobacion = value; }
        }

        private bool accion;
        /// <summary>
        /// comprobacion si la descripcion tiene una accion posterior a su muestreo
        /// </summary>
        /// <value>
        ///   <c>true</c> si existe una accion posterior; sino existe accion, <c>false</c>.
        /// </value>
        public bool Accion
        {
            get { return accion; }
            set { accion = value; }
        }

        private List<Localizaciones> localizacionAccion;
        /// <summary>
        /// Lista de localizaciones a desbloquear
        /// </summary>
        /// <value>
        /// Lista generica de tipo enum Localizaciones
        /// </value>
        public List<Localizaciones> LocalizacionAccion
        {
            get { return localizacionAccion; }
            set { localizacionAccion = value; }
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoTiradaBase"/>.
        /// </summary>
        public ObjetoTiradaBase()
        {
            localizacionAccion = new List<Localizaciones>();
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoTiradaBase"/>
        /// </summary>
        /// <param name="texto">texto descriptivo del objeto</param>
        public ObjetoTiradaBase(string texto)
        {
            localizacionAccion = new List<Localizaciones>();

            textoDescriptivo = texto;
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoTiradaBase"/>
        /// </summary>
        /// <param name="habilidad">enum de tipo Habilidades</param>
        public ObjetoTiradaBase(Habilidades habilidad)
        {
            localizacionAccion = new List<Localizaciones>();

            habilidadTirada = habilidad;
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoTiradaBase"/>
        /// </summary>
        /// <param name="texto">texto de la descripcion del objeto</param>
        /// <param name="habilidad">enum de tipo Habilidades</param>
        public ObjetoTiradaBase(string texto, Habilidades habilidad)
        {
            localizacionAccion = new List<Localizaciones>();

            textoDescriptivo = texto;
            habilidadTirada = habilidad;
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoTiradaBase"/>
        /// </summary>
        /// <param name="texto">texto de la descripcion del objeto</param>
        /// <param name="habilidad">enum de tipo Habilidades</param>
        /// <param name="comp">comprobacion de descripcion</param>
        /// <param name="escena">enum de tipo Escenas</param>
        /// <param name="objeto">enum de tipo Objetos</param>
        /// <param name="action">comprobacion de accion posterior</param>
        /// <param name="localizacion">array de enum de tipo Localizaciones</param>
        public ObjetoTiradaBase(string texto, Habilidades habilidad, bool comp, Escenas escena, Objetos objeto, bool action, Localizaciones[] localizacion)
        {
            localizacionAccion = new List<Localizaciones>();

            textoDescriptivo = texto;
            habilidadTirada = habilidad;
            comprobacion = comp;
            escenaComprobacion = escena;
            objetoComprobacion = objeto;
            accion = action;
            localizacionAccion.AddRange(localizacion);
        }
    }
}
