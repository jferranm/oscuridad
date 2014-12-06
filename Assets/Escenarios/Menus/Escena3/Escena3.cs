using UnityEngine;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena3: IEscenario
	{
		public Escena3(ControladoraEscenas managerRef)
		{
			if(Application.loadedLevelName != Escenas.Escena3.ToString())
			{
				Application.LoadLevel(Escenas.Escena3.ToString());
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
