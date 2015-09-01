using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

namespace Oscuridad.Clases
{
    /// <summary>
    /// Clase base para los Objetos/Personajes interactuables en la escena
    /// </summary>
	[System.Serializable]
    public class InteractuableGenerico
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

		private string descripcionNombre;
		/// <summary>
		/// Descripcion del nombre del Interactuable
		/// </summary>
		/// <value>
		/// string de descripcion del nombre de Interactuable
		/// </value>
		public string DescripcionNombre
		{
			get { return descripcionNombre; }
			set { descripcionNombre = value; }
		}
		
		private Interactuables interactuable;
		/// <summary>
		/// Objeto en enumeracion
		/// </summary>
		/// <value>
		/// valor de tipo enumeracion Interactuables
		/// </value>
		public Interactuables Interactuable
		{
			get { return interactuable; }
			set { interactuable = value; }
		}

        private List<OpcionInteractuable> interactuableOpciones;
        /// <summary>
        /// lista de opciones de interaccion con los interactuables
        /// </summary>
        /// <value>
        /// lista generica de tipo OpcionInteractuable
        /// </value>
        public List<OpcionInteractuable> InteractuableOpciones
        {
            get { return interactuableOpciones; }
            set { interactuableOpciones = value; }
        }

        private List<InteractuableTiradaBase> tiradasInteractuable;
        /// <summary>
        /// Lista de ObjetosTadaBase segun descripciones por tiradas
        /// </summary>
        /// <value>
        /// Lista Generica de tipo InteractuableTiradaBase
        /// </value>
        public List<InteractuableTiradaBase> TiradasInteractuable
        {
            get { return tiradasInteractuable; }
            set { tiradasInteractuable = value; }
        }

		private List<RespuestaNPCBase> conversacionInteractuable;
		/// <summary>
		/// Lista de Frases del NPC a mostrar
		/// </summary>
		/// <value>
		/// lista generica de RespuestaNPCBase
		/// </value>
		public List<RespuestaNPCBase> ConversacionInteractuable
		{
			get { return conversacionInteractuable; }
			set { conversacionInteractuable = value; }
		}
		
		private int inicioConversacion;
		/// <summary>
		/// numero de identificador de respuesta para el inicio de conversacion
		/// </summary>
		/// <value>
		/// valor entero de idRespuesta
		/// </value>
		public int InicioConversacion
		{
			get { return inicioConversacion; }
			set { inicioConversacion = value; }
		}


		private Vector3 posicionNueva;
		/// <summary>
		/// Posicion de la camara para el zoom
		/// </summary>
		/// <value>
		/// valor tipo Vector3 para posicion en mundo del objeto en zoom
		/// </value>
		public Vector3 PosicionNueva
		{
			get { return posicionNueva; }
			set { posicionNueva = value; }
		}

		private Vector3 rotacionNueva;
		/// <summary>
		/// Posicion de rotacion de la camara para el zoom
		/// </summary>
		/// <value>
		/// valor tipo Vector3 para el valor de rotacion de la camara en mundo del objeto en zoom
		/// </value>
		public Vector3 RotacionNueva
		{
			get { return rotacionNueva; }
			set { rotacionNueva = value; }
		}

		private float smooth;
		/// <summary>
		/// Suavidad de frenado del zoom
		/// </summary>
		/// <value>
		/// valor tipo float para el calculo del frenado del zoom
		/// </value>
		public float Smooth
		{
			get { return smooth; }
			set { smooth = value; }
		}

		private bool interactuableActivo;
		/// <summary>
		/// mostrar el interactuable
		/// </summary>
		/// <value>
		/// valor tipo bool para el mostrar el interactuable
		/// </value>
		public bool InteractuableActivo
		{
			get { return interactuableActivo; }
			set { interactuableActivo = value; }
		}

		private bool interactuableInspeccionado;
		/// <summary>
		/// Determina si el interactuable a sido inspeccionado
		/// </summary>
		/// <value>
		/// valor tipo bool para determinar si el interactuable a sido inspeccionado
		/// </value>
		public bool InteractuableInspeccionado
		{
			get { return interactuableInspeccionado; }
			set { interactuableInspeccionado = value; }
		}

		#endregion

		#region CONSTRUCTORES

        /// <summary>
        /// Contructor de la clase <see cref="InteractuableGenerico"/>.
        /// </summary>
        public InteractuableGenerico()
        {
			Inicializar_Listas();
        }

        /// <summary>
        /// Constructor de la clase <see cref="InteractuableGenerico"/>
        /// </summary>
        /// <param name="nombre">string de nombre del Interactuable</param>
        public InteractuableGenerico(string nombre)
        {
			Inicializar_Listas();

            this.nombre = nombre;
        }

        /// <summary>
        /// Constructor de la clase <see cref="InteractuableGenerico"/>
        /// </summary>
        /// <param name="opciones">array de enum OpcionInteractuable de interaccion con interactuables</param>
        public InteractuableGenerico(OpcionInteractuable[] opciones)
        {
			Inicializar_Listas();

            AddOpciones(opciones);
        }

        /// <summary>
        /// Constructor de la clase <see cref="InteractuableGenerico"/>
        /// </summary>
        /// <param name="opciones">array de enum OpcionInteractuable de interaccion con interactuables</param>
        /// <param name="tiradas">array de tipo ObjetoTiradasBase con las diferente descripciones segun tirada</param>
        public InteractuableGenerico(OpcionInteractuable[] opciones, InteractuableTiradaBase[] tiradas)
        {
			Inicializar_Listas ();

            AddOpciones(opciones);
            tiradasInteractuable.AddRange(tiradas);
        }

        /// <summary>
        /// Constructor de la clase <see cref="InteractuableGenerico"/>
        /// </summary>
        /// <param name="tiradas">array de tipo ObjetoTiradasBase con las diferente descripciones segun tirada</param>
        public InteractuableGenerico(InteractuableTiradaBase[] tiradas)
        {
			Inicializar_Listas ();

            tiradasInteractuable.AddRange(tiradas);
        }

        /// <summary>
        /// Constructor de la clase <see cref="InteractuableGenerico"/>
        /// </summary>
        /// <param name="opciones">array de enum OpcionInteractuable de interaccion con interactuables</param>
        /// <param name="tiradas">array de tipo ObjetoTiradasBase con las diferente descripciones segun tirada</param>
        /// <param name="nombre">string de nombre del Interactuable</param>
        public InteractuableGenerico(OpcionInteractuable[] opciones, InteractuableTiradaBase[] tiradas, string nombre)
        {
			Inicializar_Listas ();

            AddOpciones(opciones);
            tiradasInteractuable.AddRange(tiradas);

            this.nombre = nombre;
        }

		/// <summary>
		/// Constructor sobrecargado de la Clase
		/// </summary>
		/// <param name="respuestas">array de tipo RespuestaNPCBase de respuestas a mostrar del interactuable</param>
		public InteractuableGenerico(RespuestaNPCBase[] respuestas)
		{
			Inicializar_Listas ();
			
			AddRespuesta(respuestas);
		}
		
		/// <summary>
		/// Constructor sobrecargado de la Clase
		/// </summary>
		/// <param name="nombre">nombre del interactuable</param>
		/// <param name="respuestas">array de tipo RespuestaNPCBase de respuestas a mostrar del personaje</param>
		public InteractuableGenerico(string nombre, RespuestaNPCBase[] respuestas)
		{
			Inicializar_Listas ();
			
			this.nombre = nombre;
			AddRespuesta(respuestas);
		}
		
		/// <summary>
		/// Constructor sobrecargado de la Clase
		/// </summary>
		/// <param name="nombre">nombre del interactuable</param>
		/// <param name="respuestas">array de tipo RespuestaNPCBase de respuestas a mostrar del interactuable</param>
		/// <param name="idComienzo">valor entero que marca cual sera la RespuestaNPCBase donde comenzara la conversacion</param>
		public InteractuableGenerico(string nombre, RespuestaNPCBase[] respuestas, int idComienzo)
		{
			Inicializar_Listas ();
			
			this.nombre = nombre;
			inicioConversacion = idComienzo;
			AddRespuesta(respuestas);
		}

		
		#endregion
		
		#region METODOS
		
		/// <summary>
		/// Inicializa las listas de la clase
		/// </summary>
		public void Inicializar_Listas()
		{
			interactuableOpciones = new List<OpcionInteractuable> ();
			tiradasInteractuable = new List<InteractuableTiradaBase> ();
			conversacionInteractuable = new List<RespuestaNPCBase> ();
		}

        /// <summary>
        /// Añade una opcion al interactuable
        /// </summary>
        /// <param name="opcion">enum de tipo OpcionInteractuable</param>
        public void AddOpciones(OpcionInteractuable opcion)
        {
            interactuableOpciones.Add(opcion);
        }

        /// <summary>
        /// Añadir varias opciones al interactuable
        /// </summary>
        /// <param name="opciones">array de enum OpcionInteractuable</param>
        public void AddOpciones(OpcionInteractuable[] opciones)
        {
            interactuableOpciones.AddRange(opciones);
        }

        /// <summary>
		/// Borra una opcion del interactuable
		/// </summary>
		/// <param name="opcion">enum de tipo OpcionInteractuable</param>
		public void BorrarOpciones(OpcionInteractuable opcion)
        {
			interactuableOpciones.Remove(opcion);
        }

        /// <summary>
		/// Borra varias opciones del interactuable
		/// </summary>
		/// <param name="opciones">array de enum OpcionInteractuable</param>
        public void BorrarOpciones(OpcionInteractuable[] opciones)
        {
            foreach (OpcionInteractuable opcion in opciones)
            {
				interactuableOpciones.Remove(opcion);
            }
        }

        /// <summary>
		/// Muesta la lista de opciones del interactuable
		/// </summary>
        /// <returns>array de enum OpcionInteractuable</returns>
        public OpcionInteractuable[] MostrarOpciones()
        {
			return interactuableOpciones.ToArray();
        }

        /// <summary>
		/// Añade una descripcion con tirada al interactuable
		/// </summary>
        /// <param name="tirada">objeto tipo InteractuableTiradaBase</param>
        public void AddTiradas(InteractuableTiradaBase tirada)
        {
            tiradasInteractuable.Add(tirada);
        }

        /// <summary>
		/// Añade varias descripciones con tirada al interactuable
		/// </summary>
        /// <param name="tiradas">array de objetos tipo InteractuableTiradaBase</param>
        public void AddTiradas(InteractuableTiradaBase[] tiradas)
        {
            tiradasInteractuable.AddRange(tiradas);
        }

        /// <summary>
		/// Borra una descripcion con tirada en el interactuable
		/// </summary>
        /// <param name="tirada">objeto tipo InteractuableTiradaBase</param>
        public void BorrarTiradas(InteractuableTiradaBase tirada)
        {
            tiradasInteractuable.Remove(tirada);
        }

        /// <summary>
		/// Borrar varias descripciones con tirada del interactuable
		/// </summary>
        /// <param name="tiradas">array de tipo InteractuableTiradaBase</param>
        public void BorrarTiradas(InteractuableTiradaBase[] tiradas)
        {
            foreach (InteractuableTiradaBase opcion in tiradas)
            {
                tiradasInteractuable.Remove(opcion);
            }
        }

        /// <summary>
		/// Mostrar lista de decripciones con tirada del interactuable
		/// </summary>
        /// <returns>Array de tipo InteractuableTiradaBase</returns>
        public InteractuableTiradaBase[] MostrarTiradas()
        {
            return tiradasInteractuable.ToArray();
        }

		/// <summary>
		/// Mostrar lista de decripciones con tirada del interactuable que no sean ni Fallo ni Ninguno
		/// </summary>
		/// <returns>Array de tipo InteractuableTiradaBase</returns>
		public InteractuableTiradaBase[] MostrarTiradasInspeccionar()
		{
			return tiradasInteractuable.FindAll (x => (x.HabilidadTirada != Habilidades.Ninguna && x.HabilidadTirada != Habilidades.Fallo)).ToArray();
		}

		/// <summary>
		/// Muestra la descripcion del interactuable sin tiradas
		/// </summary>
		/// <returns>string con la descripcion del interactuable</returns>
		public string MostrarDescripcionBasica()
		{
			return tiradasInteractuable.Find (x => x.HabilidadTirada == Habilidades.Ninguna).TextoDescriptivo;
		}

		/// <summary>
		/// Busca una tirada de habilidad especifica dentro de las tiradas del interactuable
		/// </summary>
		/// <param name="habilidad">habilidad a buscar</param>
		/// <returns>la tirada que trabajaria con esa habilidad</returns>
		public InteractuableTiradaBase BuscarTirada(Habilidades habilidad)
		{
			return tiradasInteractuable.Find (x => x.HabilidadTirada == habilidad);
		}

		/// <summary>
		/// Busca la existencia de una tirada de habilidad especifica dentro de las tiradas del interactuable
		/// </summary>
		/// <param name="habilidad">habilidad a buscar</param>
		/// <returns>true si existe, false sino</returns>
		public bool ExisteTirada(Habilidades habilidad)
		{
			return !(tiradasInteractuable.Find (x => x.HabilidadTirada == habilidad) == null);
		}

		/// <summary>
		/// Añade una respuesta a la conversacion
		/// </summary>
		/// <param name="respuesta">respuesta tipo RespuestaNPCBase</param>
		public void AddRespuesta(RespuestaNPCBase respuesta)
		{
			conversacionInteractuable.Add(respuesta);
		}
		
		/// <summary>
		/// Añade varias respuestas a la conversacion
		/// </summary>
		/// <param name="respuestas">array de tipo RespuestaNPCBase</param>
		public void AddRespuesta(RespuestaNPCBase[] respuestas)
		{
			conversacionInteractuable.AddRange(respuestas);
		}
		
		/// <summary>
		/// Borra una respuesta
		/// </summary>
		/// <param name="respuesta">respuesta tipo RespuestaNPCBase</param>
		public void BorrarRespuesta(RespuestaNPCBase respuesta)
		{
			conversacionInteractuable.Remove(respuesta);
		}
		
		/// <summary>
		/// Borra varias respuestas
		/// </summary>
		/// <param name="respuestas">array de tipo RespuestaNPCBase</param>
		public void BorrarRespuesta(RespuestaNPCBase[] respuestas)
		{
			foreach (RespuestaNPCBase respuesta in respuestas)
			{
				conversacionInteractuable.Remove(respuesta);
			}
		}

		/// <summary>
		/// Muestra la conversacion del interactuable
		/// </summary>
		/// <returns> array de tipo RespuestaNPCBase</returns>
		public RespuestaNPCBase[] MostrarRespuestas()
		{
			return conversacionInteractuable.ToArray();
		}
		
		/// <summary>
		/// Devuelve una Respuesta a partir de su id de Respuesta
		/// </summary>
		/// <returns> objeto tipo RespuestaNPCBase</returns>
		public RespuestaNPCBase Devolver_Respuesta(int numRespuesta)
		{
			return  MostrarRespuestas().ToList().Find (x => x.IdRespuestaNPC == numRespuesta);
		}
	
		public bool PreguntaConTirada(int numeroPregunta)
		{
			foreach (RespuestaNPCBase respuesta in MostrarRespuestas()) 
			{
				foreach(PreguntaUsuarioBase pregunta in respuesta.MostrarPreguntas())
				{
					if (pregunta.IdPreguntaUsuario.Equals(numeroPregunta))
					{
						return pregunta.PreguntaTirada;
					}
				}
			}

			return false;
		}

		public PreguntaUsuarioBase Devolver_Pregunta(int numeroPregunta)
		{
			foreach (RespuestaNPCBase respuesta in MostrarRespuestas()) 
			{
				foreach(PreguntaUsuarioBase pregunta in respuesta.MostrarPreguntas())
				{
					if (pregunta.IdPreguntaUsuario.Equals(numeroPregunta))
					{
						return pregunta;
					}
				}
			}

			return null;
		}

		#endregion
	}
}
