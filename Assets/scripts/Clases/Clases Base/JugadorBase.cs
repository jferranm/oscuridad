using System;
using System.Collections.Generic;
using System.Collections;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase base para personaje jugador
    /// </summary>
	[System.Serializable]
    public class JugadorBase
    {
		#region VARIABLES
		private string nombrePersonaje;
		/// <summary>
		/// Nombre del Jugador
		/// </summary>
		/// <value>
		/// string con nombre de jugador
		/// </value>
		public string NombrePersonaje
		{
			get { return nombrePersonaje; }
			set { nombrePersonaje = value; }
		}

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

        private EstadosJugador estadoJugador;
        /// <summary>
        /// estados del personaje jugador
        /// </summary>
        /// <value>
        /// enum tipo estadosJugador
        /// </value>
        public EstadosJugador EstadoJugador
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

		private List<Acciones> accionesRealizadas;
		/// <summary>
		/// Lista de acciones realizadas por el personaje jugador
		/// </summary>
		/// <value>
		/// Lista generica de tipo Acciones
		/// </value>
		public List<Acciones> AccionesRealizadas
		{
			get { return accionesRealizadas; }
			set { accionesRealizadas = value; }
		}

		private List<Localizaciones> localizacionesDescubiertas;
		/// <summary>
		/// Lista de localizaciones Descubiertas por el personajeJugador
		/// </summary>
		/// <value>
		/// Lista generica de tipo Localizaciones
		/// </value>
		public List<Localizaciones> LocalizacionesDescubiertas
		{
			get { return localizacionesDescubiertas; }
			set { localizacionesDescubiertas = value; }
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

		public struct habilidadesJugador
		{
			public int Antropologia;
			public int Arqueologia;
			public int Astronomia;
			public int BuscarLibros;
			public int CienciasOcultas;
			public int Conducir;
			public int Cerrajeria;
			public int Charlataneria;
			public int Credito;
			public int Derecho;
			public int Descubrir;
			public int Disfrazarse;
			public int Discrecion;
			public int Escuchar;
			public int Equitacion;
			public int Esquivar;
			public int Fotografia;
			public int HabilidadArtisticaPintar;
			public int Historia;
			public int HistoriaNatural;
			public int LenguaPropia;
			public int Mecanica;
			public int MitosDeCthulhu;
			public int Nadar;
			public int Ocultar;
			public int Ocultarse;
			public int OtraLenguaFrances;
			public int OtraLenguaLatin;
			public int OtraLenguaAleman;
			public int OtraLenguaItaliano;
			public int Persuasion;
			public int PrimerosAuxilios;
			public int Psicologia;
			public int Quimica;
			public int Regatear;
			public int Saltar;
			public int Examinar;

			public int Devolver_Valor_Segun_Enum(Habilidades habilidad)
			{
				int aux = 0;

				switch (habilidad) 
				{
					case Habilidades.Antropologia:
						aux =  Antropologia;
						break;
					case Habilidades.Arqueologia:
						aux =  Arqueologia;
						break;
					case Habilidades.Astronomia:
						aux =  Astronomia;
						break;
					case Habilidades.BuscarLibros:
						aux =  BuscarLibros;
						break;
					case Habilidades.CienciasOcultas:
						aux =  CienciasOcultas;
						break;
					case Habilidades.Conducir:
						aux =  Conducir;
						break;
					case Habilidades.Cerrajeria:
						aux =  Cerrajeria;
						break;
					case Habilidades.Charlataneria:
						aux =  Charlataneria;
						break;
					case Habilidades.Credito:
						aux =  Credito;
						break;
					case Habilidades.Derecho:
						aux =  Derecho;
						break;
					case Habilidades.Descubrir:
						aux =  Descubrir;
						break;
					case Habilidades.Disfrazarse:
						aux =  Disfrazarse;
						break;
					case Habilidades.Discrecion:
						aux =  Discrecion;
						break;
					case Habilidades.Escuchar:
						aux =  Escuchar;
						break;
					case Habilidades.Equitacion:
						aux =  Equitacion;
						break;
					case Habilidades.Esquivar:
						aux =  Esquivar;
						break;
					case Habilidades.Fotografia:
						aux =  Fotografia;
						break;
					case Habilidades.HabilidadArtisticaPintar:
						aux =  HabilidadArtisticaPintar;
						break;
					case Habilidades.Historia:
						aux =  Historia;
						break;
					case Habilidades.HistoriaNatural:
						aux =  HistoriaNatural;
						break;
					case Habilidades.LenguaPropia:
						aux =  LenguaPropia;
						break;
					case Habilidades.Mecanica:
						aux =  Mecanica;
						break;
					case Habilidades.MitosDeCthulhu:
						aux =  MitosDeCthulhu;
						break;
					case Habilidades.Nadar:
						aux =  Nadar;
						break;
					case Habilidades.Ocultar:
						aux =  Ocultar;
						break;
					case Habilidades.Ocultarse:
						aux =  Ocultarse;
						break;
					case Habilidades.OtraLenguaFrances:
						aux =  OtraLenguaFrances;
						break;
					case Habilidades.OtraLenguaLatin:
						aux =  OtraLenguaLatin;
						break;
					case Habilidades.OtraLenguaAleman:
						aux =  OtraLenguaAleman;
						break;
					case Habilidades.OtraLenguaItaliano:
						aux =  OtraLenguaItaliano;
						break;
					case Habilidades.Persuasion:
						aux =  Persuasion;
						break;
					case Habilidades.PrimerosAuxilios:
						aux =  PrimerosAuxilios;
						break;
					case Habilidades.Psicologia:
						aux =  Psicologia;
						break;
					case Habilidades.Quimica:
						aux =  Quimica;
						break;
					case Habilidades.Regatear:
						aux =  Regatear;
						break;
					case Habilidades.Saltar:
						aux =  Saltar;
						break;
					case Habilidades.Examinar:
						aux = Examinar;
						break;
				}
				return aux;
			}

			public string Devolver_Descripcion_Segun_Enum(Habilidades habilidad)
			{
				string aux = "";
				
				switch (habilidad) 
				{
				case Habilidades.Antropologia:
					aux =  "Antropologia";
					break;
				case Habilidades.Arqueologia:
					aux =  "Arqueologia";
					break;
				case Habilidades.Astronomia:
					aux =  "Astronomia";
					break;
				case Habilidades.BuscarLibros:
					aux =  "Buscar Libros";
					break;
				case Habilidades.CienciasOcultas:
					aux =  "Ciencias Ocultas";
					break;
				case Habilidades.Conducir:
					aux =  "Conducir";
					break;
				case Habilidades.Cerrajeria:
					aux =  "Cerrajeria";
					break;
				case Habilidades.Charlataneria:
					aux =  "Charlataneria";
					break;
				case Habilidades.Credito:
					aux =  "Credito";
					break;
				case Habilidades.Derecho:
					aux =  "Derecho";
					break;
				case Habilidades.Descubrir:
					aux =  "Descubrir";
					break;
				case Habilidades.Disfrazarse:
					aux =  "Disfrazarse";
					break;
				case Habilidades.Discrecion:
					aux =  "Discrecion";
					break;
				case Habilidades.Escuchar:
					aux =  "Escuchar";
					break;
				case Habilidades.Equitacion:
					aux =  "Equitacion";
					break;
				case Habilidades.Esquivar:
					aux =  "Esquivar";
					break;
				case Habilidades.Fotografia:
					aux =  "Fotografia";
					break;
				case Habilidades.HabilidadArtisticaPintar:
					aux =  "Habilidad Artistica (Pintar)";
					break;
				case Habilidades.Historia:
					aux =  "Historia";
					break;
				case Habilidades.HistoriaNatural:
					aux =  "Historia Natural";
					break;
				case Habilidades.LenguaPropia:
					aux =  "Lengua Propia";
					break;
				case Habilidades.Mecanica:
					aux =  "Mecanica";
					break;
				case Habilidades.MitosDeCthulhu:
					aux =  "Mitos De Cthulhu";
					break;
				case Habilidades.Nadar:
					aux =  "Nadar";
					break;
				case Habilidades.Ocultar:
					aux =  "Ocultar";
					break;
				case Habilidades.Ocultarse:
					aux =  "Ocultarse";
					break;
				case Habilidades.OtraLenguaFrances:
					aux =  "Otra Lengua (Frances)";
					break;
				case Habilidades.OtraLenguaLatin:
					aux =  "Otra Lengua (Latin)";
					break;
				case Habilidades.OtraLenguaAleman:
					aux =  "Otra Lengua (Aleman)";
					break;
				case Habilidades.OtraLenguaItaliano:
					aux =  "Otra Lengua (Italiano)";
					break;
				case Habilidades.Persuasion:
					aux =  "Persuasion";
					break;
				case Habilidades.PrimerosAuxilios:
					aux =  "Primeros Auxilios";
					break;
				case Habilidades.Psicologia:
					aux =  "Psicologia";
					break;
				case Habilidades.Quimica:
					aux =  "Quimica";
					break;
				case Habilidades.Regatear:
					aux =  "Regatear";
					break;
				case Habilidades.Saltar:
					aux =  "Saltar";
					break;
				case Habilidades.Examinar:
					aux = "Examinar";
					break;
				}
				return aux;
			}
		}
		/// <summary>
		/// Estructura para los valores de las habilidades del jugador
		/// </summary>
		/// <value>
		/// valores enteros para las habilidades del jugador
		/// </value>
		public habilidadesJugador HabilidadesJugador;

		public struct caracteristicasJugador
		{
			public int Fuerza;
			public int Constitucion;
			public int Tamanyo;
			public int Destreza;
			public int Aparicencia;
			public int Cordura;
			public int Inteligencia;
			public int Poder;
			public int Educacion;
			public int Idea;
			public int Suerte;
			public int Conocimiento;
			public int Vida;
			public int BonificacionDanyo;
		}
		/// <summary>
		/// Estructura para los valores de las caracteristicas del jugador
		/// </summary>
		/// <value>
		/// valores enteros para las caracteristicas del jugador
		/// </value>
		public caracteristicasJugador CaracteristicasJugador;

		public struct datosPersonalesJugador
		{
			public string Nombre;
			public string Profesion;
			public int Edad;
		}
		/// <summary>
		/// Estructura para los valores de los datos personales del jugador
		/// </summary>
		/// <value>
		/// valores para los datos personales del jugador
		/// </value>
		public datosPersonalesJugador DatosPersonalesJugador;

		public struct armasJugador
		{
			public int ArmaCorta;
			public int Fusil;
			public int Escopeta;
		}
		/// <summary>
		/// Estructura para los valores de las armas del jugador
		/// </summary>
		/// <value>
		/// valores enteros para las armas del jugador
		/// </value>
		public armasJugador ArmasJugador;

		#endregion

		#region CONSTRUCTORES

        /// <summary>
        /// Constructor de la clase <see cref="JugadorBase"/> class.
        /// </summary>
        public JugadorBase()
        {
            escenasVisitadas = new List<Escenas>();
            objetosVistos = new List<Objetos>();
            inventario = new List<Objetos>();
			accionesRealizadas = new List<Acciones> ();
			localizacionesDescubiertas = new List<Localizaciones> ();
			CaracteristicasJugador = new caracteristicasJugador ();
			DatosPersonalesJugador = new datosPersonalesJugador ();
			ArmasJugador = new armasJugador ();

            estadoJugador = EstadosJugador.enMenus;
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
			accionesRealizadas = new List<Acciones> ();
			localizacionesDescubiertas = new List<Localizaciones> ();
			CaracteristicasJugador = new caracteristicasJugador ();
			DatosPersonalesJugador = new datosPersonalesJugador ();
			ArmasJugador = new armasJugador ();

            estadoJugador = EstadosJugador.enMenus;
            tipoPersonaje = nuevoPersonaje;
        }

		#endregion

		#region METODOS

        /// <summary>
        /// Añade una escena visitada
        /// </summary>
        /// <param name="escenaVisitada">objeto tipo Escenas</param>
        public void AddEscenaVisitada(Escenas escenaVisitada)
        {
			if(!escenasVisitadas.Contains(escenaVisitada) && escenaVisitada != Escenas.ninguna)
            	escenasVisitadas.Add(escenaVisitada);
        }

        /// <summary>
        /// Añade un objeto visto
        /// </summary>
        /// <param name="objetoVisto">objeto tipo Objetos</param>
        public void AddObjetoVisto(Objetos objetoVisto)
        {
			if(!objetosVistos.Contains(objetoVisto))
            	objetosVistos.Add(objetoVisto);
        }

		/// <summary>
		/// Añade una accion realizada
		/// </summary>
		/// <param name="accionRealizada">objeto tipo Acciones</param>
		public void AddEscenaVisitada(Acciones accionRealizada)
		{
			if(!accionesRealizadas.Contains(accionRealizada))
				accionesRealizadas.Add(accionRealizada);
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

		/// <summary>
		/// Checkea que se haya visitado una escena
		/// </summary>
		/// <param name="escena">objeto tipo Escenas</param>
		/// <returns> true si se a visitado, false sino</returns>
		public bool EscenaVista (Escenas escena)
		{
			return EscenasVisitadas.Contains (escena);
		}

		/// <summary>
		/// Checkea que se haya interactuado con un objeto
		/// </summary>
		/// <param name="objeto">objeto tipo Objetos</param>
		/// <returns> true si se a interactuado con el objeto, false sino</returns>
		public bool ObjetoVisto (Objetos objeto)
		{
			return ObjetosVistos.Contains (objeto);
		}

		/// <summary>
		/// Checkea que se haya realizado un tipo de accion
		/// </summary>
		/// <param name="accion">objeto tipo Acciones</param>
		/// <returns> true si se a realizado la accion, false sino</returns>
		public bool AccionRealizada (Acciones accion)
		{
			return AccionesRealizadas.Contains(accion);
		}

		/// <summary>
		/// Añade una localizacion a las localizaciones Descubiertas
		/// </summary>
		/// <param name="localizacion">objeto tipo Localizaciones</param>
		public void AddLocalizacionDescubierta(Localizaciones localizacion)
		{
			LocalizacionesDescubiertas.Add (localizacion);
		}

		/// <summary>
		/// Añade una accion a las Acciones Realizadas
		/// </summary>
		/// <param name="accion">objeto tipo Accion</param>
		public void AddAccionRealizada(Acciones accion)
		{
			AccionesRealizadas.Add (accion);
		}

		#endregion
    }
}
