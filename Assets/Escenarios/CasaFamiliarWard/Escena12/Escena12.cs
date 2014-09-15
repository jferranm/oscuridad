using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.Collections.Generic;

namespace Oscuridad.Estados
{
	public class Escena12: IEscenario
	{
		private ControladoraEscenas manager;

		public Escena12(ControladoraEscenas managerRef)
		{
			manager = managerRef;

			if(Application.loadedLevelName != EstadoJuego.Escena12.ToString())
			{
				Application.LoadLevel(EstadoJuego.Escena12.ToString());
			}

			InicializarDatos ();
		}

		public void InicializarDatos()
		{
			GameCenter.InstanceRef.controladoraGUI.Activar_Cargando();
		}
		
		public void EstadoUpdate()
		{
		}
		
		public void Mostrar()
		{
		}

		public void NivelCargado()
		{
			//Inicializamos la escena
			GameCenter.InstanceRef.controladoraGUI.Devolver_Pantalla_Carga().comenzarFade = true;
			GameCenter.InstanceRef.Inicializar_Escenario ();
		}
	}
}
