using UnityEngine;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena2: IEscenario
	{
		public Escena2(ControladoraEscenas managerRef)
		{
			if(Application.loadedLevelName != Escenas.Escena2.ToString())
			{
				Application.LoadLevel(Escenas.Escena2.ToString());
			}

			InicializarDatos ();
		}
				
		public void InicializarDatos()
		{
			GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enMenus;
		}
		
		public void EstadoUpdate()
		{
		}
		
		public void Mostrar()
		{
		}

		public void NivelCargado()
		{
		}
	}
}
