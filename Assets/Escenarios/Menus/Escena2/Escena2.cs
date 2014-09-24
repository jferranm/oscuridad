using UnityEngine;
using Oscuridad.Interfaces;
using Oscuridad.Personajes;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena2: IEscenario
	{
		private ControladoraEscenas manager;

		public Escena2(ControladoraEscenas managerRef)
		{
			manager = managerRef;

			if(Application.loadedLevelName != EstadoJuego.Escena2.ToString())
			{
				Application.LoadLevel(EstadoJuego.Escena2.ToString());
			}

			InicializarDatos ();
		}
				
		public void InicializarDatos()
		{
			GameCenter.InstanceRef.controladoraJugador.Cambiar_Estado (EstadosJugador.enMenus);
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
