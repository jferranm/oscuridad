using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;

namespace Oscuridad.Clases
{
    public class ObjetoTiradaBase
    {
        private string textoDescriptivo;
        public string TextoDescriptivo
        {
            get { return textoDescriptivo; }
            set { textoDescriptivo = value; }
        }

        private Habilidades habilidadTirada;
        public Habilidades HabilidadTirada
        {
            get { return habilidadTirada; }
            set { habilidadTirada = value; }
        }

        public ObjetoTiradaBase()
        {
        }

        public ObjetoTiradaBase(string texto)
        {
            textoDescriptivo = texto;
        }

        public ObjetoTiradaBase(Habilidades habilidad)
        {
            habilidadTirada = habilidad;
        }

        public ObjetoTiradaBase(string texto, Habilidades habilidad)
        {
            textoDescriptivo = texto;
            habilidadTirada = habilidad;
        }
    }
}
