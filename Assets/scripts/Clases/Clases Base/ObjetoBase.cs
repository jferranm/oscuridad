using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializadorXMLOscuridad
{
    /// <summary>
    /// Clase base para los objetos interactuables en la escena
    /// </summary>
    [Serializable()]
    public class ObjetoBase
    {
        private string nombre;
        /// <summary>
        /// Nombre del objeto
        /// </summary>
        /// <value>
        /// string de nombre de objeto
        /// </value>
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private List<opcionObjeto> objetoOpciones;
        /// <summary>
        /// lista de opciones de interaccion con los objetos
        /// </summary>
        /// <value>
        /// lista generica de tipo opcionObjeto
        /// </value>
        public List<opcionObjeto> ObjetoOpciones
        {
            get { return objetoOpciones; }
            set { objetoOpciones = value; }
        }

        private List<ObjetoTiradaBase> tiradasObjeto;
        /// <summary>
        /// Lista de ObjetosTadaBase segun descripciones por tiradas
        /// </summary>
        /// <value>
        /// Lista Generica de tipo ObjetoTiradaBase
        /// </value>
        public List<ObjetoTiradaBase> TiradasObjeto
        {
            get { return tiradasObjeto; }
            set { tiradasObjeto = value; }
        }

        /// <summary>
        /// Contructor de la clase <see cref="ObjetoBase"/>.
        /// </summary>
        public ObjetoBase()
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoBase"/>
        /// </summary>
        /// <param name="nombre">string de nombre del Objeto</param>
        public ObjetoBase(string nombre)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            this.nombre = nombre;
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoBase"/>
        /// </summary>
        /// <param name="opciones">array de enum opcionObjeto de interaccion con objeto</param>
        public ObjetoBase(opcionObjeto[] opciones)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            AddOpciones(opciones);
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoBase"/>
        /// </summary>
        /// <param name="opciones">array de enum opcionObjeto de interaccion con objeto</param>
        /// <param name="tiradas">array de tipo ObjetoTiradasBase con las diferente descripciones segun tirada</param>
        public ObjetoBase(opcionObjeto[] opciones, ObjetoTiradaBase[] tiradas)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            AddOpciones(opciones);
            tiradasObjeto.AddRange(tiradas);
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoBase"/>
        /// </summary>
        /// <param name="tiradas">array de tipo ObjetoTiradasBase con las diferente descripciones segun tirada</param>
        public ObjetoBase(ObjetoTiradaBase[] tiradas)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            tiradasObjeto.AddRange(tiradas);
        }

        /// <summary>
        /// Constructor de la clase <see cref="ObjetoBase"/>
        /// </summary>
        /// <param name="opciones">array de enum opcionObjeto de interaccion con objeto</param>
        /// <param name="tiradas">array de tipo ObjetoTiradasBase con las diferente descripciones segun tirada</param>
        /// <param name="nombre">string de nombre del Objeto</param>
        public ObjetoBase(opcionObjeto[] opciones, ObjetoTiradaBase[] tiradas, string nombre)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            AddOpciones(opciones);
            tiradasObjeto.AddRange(tiradas);

            this.nombre = nombre;
        }

        /// <summary>
        /// Añade una opcion al objeto
        /// </summary>
        /// <param name="opcion">enum de tipo opcionObjeto</param>
        public void AddOpciones(opcionObjeto opcion)
        {
            objetoOpciones.Add(opcion);
        }

        /// <summary>
        /// Añadir varias opciones al objeto
        /// </summary>
        /// <param name="opciones">array de enum opcionObjeto</param>
        public void AddOpciones(opcionObjeto[] opciones)
        {
            objetoOpciones.AddRange(opciones);
        }

        /// <summary>
        /// Borra una opcion del objeto
        /// </summary>
        /// <param name="opcion">enum de tipo opcionObjeto</param>
        public void BorrarOpciones(opcionObjeto opcion)
        {
            objetoOpciones.Remove(opcion);
        }

        /// <summary>
        /// Borra varias opciones del objeto
        /// </summary>
        /// <param name="opciones">array de enum opcionObjeto</param>
        public void BorrarOpciones(opcionObjeto[] opciones)
        {
            foreach (opcionObjeto opcion in opciones)
            {
                objetoOpciones.Remove(opcion);
            }
        }

        /// <summary>
        /// Muesta la lista de opciones del objeto
        /// </summary>
        /// <returns>array de enum opcionObjeto</returns>
        public opcionObjeto[] MostrarOpciones()
        {
            return objetoOpciones.ToArray();
        }

        /// <summary>
        /// Añade una descripcion con tirada al objeto
        /// </summary>
        /// <param name="tirada">objeto tipo ObjetoTiradaBase</param>
        public void AddTiradas(ObjetoTiradaBase tirada)
        {
            tiradasObjeto.Add(tirada);
        }

        /// <summary>
        /// Añade varias descripciones con tirada al objeto
        /// </summary>
        /// <param name="tiradas">array de objetos tipo ObjetoTiradaBase</param>
        public void AddTiradas(ObjetoTiradaBase[] tiradas)
        {
            tiradasObjeto.AddRange(tiradas);
        }

        /// <summary>
        /// Borra una descripcion con tirada en el objeto
        /// </summary>
        /// <param name="tirada">objeto tipo ObjetoTiradaBase</param>
        public void BorrarTiradas(ObjetoTiradaBase tirada)
        {
            tiradasObjeto.Remove(tirada);
        }

        /// <summary>
        /// Borrar varias descripciones con tirada del objeto
        /// </summary>
        /// <param name="tiradas">array de tipo ObjetoTiradaBase</param>
        public void BorrarTiradas(ObjetoTiradaBase[] tiradas)
        {
            foreach (ObjetoTiradaBase opcion in tiradas)
            {
                tiradasObjeto.Remove(opcion);
            }
        }

        /// <summary>
        /// Mostrar lista de decripciones con tirada del objeto
        /// </summary>
        /// <returns>Array de tipo ObjetoTiradaBase</returns>
        public ObjetoTiradaBase[] MostrarTiradas()
        {
            return tiradasObjeto.ToArray();
        }
    }
}
