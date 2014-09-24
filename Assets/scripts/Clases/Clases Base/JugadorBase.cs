using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    public class JugadorBase
    {
        private Personaje tipoPersonaje;
        public Personaje TipoPersonaje
        {
            get { return tipoPersonaje; }
            set { tipoPersonaje = value; }
        }

        private EstadosJugador estadoJugador;
        public EstadosJugador EstadoJugador
        {
            get { return estadoJugador; }
            set { estadoJugador = value; }
        }

        private List<EstadoJuego> escenasVisitadas;
		public List<EstadoJuego> EscenasVisitadas
        {
            get { return escenasVisitadas; }
            set { escenasVisitadas = value; }
        }

        private List<Objetos> objetosVistos;
        public List<Objetos> ObjetosVistos
        {
            get { return objetosVistos; }
            set { objetosVistos = value; }
        }

        private List<Objetos> inventario;
        public List<Objetos> Inventario
        {
            get { return inventario; }
            set { inventario = value; }
        }

        public JugadorBase()
        {
			escenasVisitadas = new List<EstadoJuego>();
            objetosVistos = new List<Objetos>();
            inventario = new List<Objetos>();

            estadoJugador = EstadosJugador.enMenus;
        }

        public JugadorBase(Personaje nuevoPersonaje)
        {
			escenasVisitadas = new List<EstadoJuego>();
            objetosVistos = new List<Objetos>();
            inventario = new List<Objetos>();

            estadoJugador = EstadosJugador.enMenus;
            tipoPersonaje = nuevoPersonaje;
        }

        public void AddEscenaVisitada(EstadoJuego escenaVisitada)
        {
            escenasVisitadas.Add(escenaVisitada);
        }

        public void AddObjetoVisto(Objetos objetoVisto)
        {
            objetosVistos.Add(objetoVisto);
        }

        public void AddInventario(Objetos objeto)
        {
            inventario.Add(objeto);
        }

        public void BorrarInventario(Objetos objeto)
        {
            inventario.Remove(objeto);
        }
    }
}
