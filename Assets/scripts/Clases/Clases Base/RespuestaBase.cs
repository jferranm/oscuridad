using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    public class RespuestaBase
    {
        private string textoRespuesta;
        public string TextoRespuesta
        {
            get { return textoRespuesta; }
            set { textoRespuesta = value; }
        }

        private int idPregunta;
        public int IdPregunta
        {
            get { return idPregunta; }
            set { idPregunta = value; }
        }

        private int idRespuesta;
        public int IdRespuesta
        {
            get { return idRespuesta; }
            set { idRespuesta = value; }
        }

        private Habilidades habilidadTirada;
        public Habilidades HabilidadTirada
        {
            get { return habilidadTirada; }
            set { habilidadTirada = value; }
        }

        public RespuestaBase()
        {
        }

        public RespuestaBase(string texto)
        {
            textoRespuesta = texto;
        }

        public RespuestaBase(int id)
        {
            idPregunta = id;
        }

        public RespuestaBase(Habilidades habilidad)
        {
            habilidadTirada = habilidad;
        }

        public RespuestaBase(string texto, int id, Habilidades habilidad)
        {
            textoRespuesta = texto;
            idPregunta = id;
            habilidadTirada = habilidad;
        }
    }
}
