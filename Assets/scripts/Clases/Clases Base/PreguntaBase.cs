using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    public class PreguntaBase
    {
        private string textoPregunta;
        public string TextoPregunta
        {
            get { return textoPregunta; }
            set { textoPregunta = value; }
        }

        private int idPregunta;
        public int IdPregunta
        {
            get { return idPregunta; }
            set { idPregunta = value; }
        }

        private List<RespuestaBase> respuestasPregunta;
        public List<RespuestaBase> RespuestasPregunta
        {
            get { return respuestasPregunta; }
            set { respuestasPregunta = value; }
        }

        public PreguntaBase()
        {
            respuestasPregunta = new List<RespuestaBase>();
        }

        public PreguntaBase(string texto)
        {
            textoPregunta = texto;
            respuestasPregunta = new List<RespuestaBase>();
        }

        public PreguntaBase(int id)
        {
            idPregunta = id;
            respuestasPregunta = new List<RespuestaBase>();
        }

        public PreguntaBase(RespuestaBase[] respuestas)
        {
            respuestasPregunta = new List<RespuestaBase>();

            AddRespuesta(respuestas);
        }

        public PreguntaBase(string texto, int id, RespuestaBase[] respuestas)
        {
            respuestasPregunta = new List<RespuestaBase>();
            
            textoPregunta = texto;
            idPregunta = id;
            AddRespuesta(respuestas);
        }

        public void AddRespuesta(RespuestaBase respuesta)
        {
            respuestasPregunta.Add(respuesta);
        }

        public void AddRespuesta(RespuestaBase[] respuestas)
        {
            respuestasPregunta.AddRange(respuestas);
        }

        public void BorrarRespuesta(RespuestaBase respuesta)
        {
            respuestasPregunta.Remove(respuesta);
        }

        public void BorrarRespuesta(RespuestaBase[] respuestas)
        {
            foreach (RespuestaBase respuesta in respuestas)
            {
                respuestasPregunta.Remove(respuesta);
            }
        }

        public RespuestaBase[] MostrarRespuestas()
        {
            return respuestasPregunta.ToArray();
        }
    }
}
