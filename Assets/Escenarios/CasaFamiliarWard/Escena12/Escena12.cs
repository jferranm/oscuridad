using UnityEngine;
using System.Collections;
using System.IO;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.Collections.Generic;

namespace Oscuridad.Estados
{
	public class Escena12: IEscenario
	{
		public Escena12(ControladoraEscenas managerRef)
		{
			if(Application.loadedLevelName != Escenas.Escena12.ToString())
			{
				Application.LoadLevel(Escenas.Escena12.ToString());
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
			//Cargamos Datos Serializados
			GameCenter.InstanceRef.controladoraJuego.escenaActual = GameCenter.InstanceRef.controladoraJuego.Cargar_Escena(Escenas.Escena12);

			//Inicializamos la escena
			GameCenter.InstanceRef.controladoraGUI.Activar_Fade();
			GameCenter.InstanceRef.Inicializar_Escenario ();
		}
	}
}
