using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    public class ObjetoBase
    {
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private List<opcionObjeto> objetoOpciones;
        public List<opcionObjeto> ObjetoOpciones
        {
            get { return objetoOpciones; }
            set { objetoOpciones = value; }
        }

        private List<ObjetoTiradaBase> tiradasObjeto;
        public List<ObjetoTiradaBase> TiradasObjeto
        {
            get { return tiradasObjeto; }
            set { tiradasObjeto = value; }
        }

        public ObjetoBase()
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();
        }

        public ObjetoBase(string nombre)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            this.nombre = nombre;
        }

        public ObjetoBase(opcionObjeto[] opciones)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            AddOpciones(opciones);
        }

        public ObjetoBase(opcionObjeto[] opciones, ObjetoTiradaBase[] tiradas)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            AddOpciones(opciones);
            tiradasObjeto.AddRange(tiradas);
        }

        public ObjetoBase(ObjetoTiradaBase[] tiradas)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            tiradasObjeto.AddRange(tiradas);
        }

        public ObjetoBase(opcionObjeto[] opciones, ObjetoTiradaBase[] tiradas, string nombre)
        {
            objetoOpciones = new List<opcionObjeto>();
            tiradasObjeto = new List<ObjetoTiradaBase>();

            AddOpciones(opciones);
            tiradasObjeto.AddRange(tiradas);

            this.nombre = nombre;
        }

        public void AddOpciones(opcionObjeto opcion)
        {
            objetoOpciones.Add(opcion);
        }

        public void AddOpciones(opcionObjeto[] opciones)
        {
            objetoOpciones.AddRange(opciones);
        }

        public void BorrarOpciones(opcionObjeto opcion)
        {
            objetoOpciones.Remove(opcion);
        }

        public void BorrarOpciones(opcionObjeto[] opciones)
        {
            foreach (opcionObjeto opcion in opciones)
            {
                objetoOpciones.Remove(opcion);
            }
        }

        public opcionObjeto[] MostrarOpciones()
        {
            return objetoOpciones.ToArray();
        }

        public void AddTiradas(ObjetoTiradaBase tirada)
        {
            tiradasObjeto.Add(tirada);
        }

        public void AddTiradas(ObjetoTiradaBase[] tiradas)
        {
            tiradasObjeto.AddRange(tiradas);
        }

        public void BorrarTiradas(ObjetoTiradaBase tirada)
        {
            tiradasObjeto.Remove(tirada);
        }

        public void BorrarTiradas(ObjetoTiradaBase[] tiradas)
        {
            foreach (ObjetoTiradaBase opcion in tiradas)
            {
                tiradasObjeto.Remove(opcion);
            }
        }

        public ObjetoTiradaBase[] MostrarTiradas()
        {
            return tiradasObjeto.ToArray();
        }
    }
}
