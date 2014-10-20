using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializadorXMLOscuridad
{
    /// <summary>
    /// Clase base para personaje jugador
    /// </summary>
    public class JugadorBase
    {
        private Personaje tipoPersonaje;
        /// <summary>
        /// Tipo de personaje jugador
        /// </summary>
        /// <value>
        /// enum tipo Personaje
        /// </value>
        public Personaje TipoPersonaje
        {
            get { return tipoPersonaje; }
            set { tipoPersonaje = value; }
        }

        private estadosJugador estadoJugador;
        /// <summary>
        /// estados del personaje jugador
        /// </summary>
        /// <value>
        /// enum tipo estadosJugador
        /// </value>
        public estadosJugador EstadoJugador
        {
            get { return estadoJugador; }
            set { estadoJugador = value; }
        }

        private List<Escenas> escenasVisitadas;
        /// <summary>
        /// Lista de escenas visitadas por el personaje jugador
        /// </summary>
        /// <value>
        /// lista generica de tipo enum Escenas
        /// </value>
        public List<Escenas> EscenasVisitadas
        {
            get { return escenasVisitadas; }
            set { escenasVisitadas = value; }
        }

        private List<Objetos> objetosVistos;
        /// <summary>
        /// Lista de objetos vistos por el personaje jugador
        /// </summary>
        /// <value>
        /// Lista generica de tipo Objetos
        /// </value>
        public List<Objetos> ObjetosVistos
        {
            get { return objetosVistos; }
            set { objetosVistos = value; }
        }

        private List<Objetos> inventario;
        /// <summary>
        /// Lista de objetos en el inventario del personaje jugador
        /// </summary>
        /// <value>
        /// lista generica de tipo Objetos
        /// </value>
        public List<Objetos> Inventario
        {
            get { return inventario; }
            set { inventario = value; }
        }

        /// <summary>
        /// Constructor de la clase <see cref="JugadorBase"/> class.
        /// </summary>
        public JugadorBase()
        {
            escenasVisitadas = new List<Escenas>();
            objetosVistos = new List<Objetos>();
            inventario = new List<Objetos>();

            estadoJugador = estadosJugador.enMenus;
        }

        /// <summary>
        /// Constructor de la clase <see cref="JugadorBase"/> class.
        /// </summary>
        /// <param name="nuevoPersonaje">objeto tipo enum Personaje</param>
        public JugadorBase(Personaje nuevoPersonaje)
        {
            escenasVisitadas = new List<Escenas>();
            objetosVistos = new List<Objetos>();
            inventario = new List<Objetos>();

            estadoJugador = estadosJugador.enMenus;
            tipoPersonaje = nuevoPersonaje;
        }

        /// <summary>
        /// Añade una escena visitada
        /// </summary>
        /// <param name="escenaVisitada">objeto tipo Escenas</param>
        public void AddEscenaVisitada(Escenas escenaVisitada)
        {
            escenasVisitadas.Add(escenaVisitada);
        }

        /// <summary>
        /// Añade un objeto visto
        /// </summary>
        /// <param name="objetoVisto">objeto tipo Objetos</param>
        public void AddObjetoVisto(Objetos objetoVisto)
        {
            objetosVistos.Add(objetoVisto);
        }

        /// <summary>
        /// Añade un objeto al inventario
        /// </summary>
        /// <param name="objeto">objeto tipo Objetos</param>
        public void AddInventario(Objetos objeto)
        {
            inventario.Add(objeto);
        }

        /// <summary>
        /// Borra un objeto del invetario
        /// </summary>
        /// <param name="objeto">objeto tipo Objetos</param>
        public void BorrarInventario(Objetos objeto)
        {
            inventario.Remove(objeto);
        }
    }
}
