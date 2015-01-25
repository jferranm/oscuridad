using UnityEngine;
using System.Collections;
using System.IO;
using Oscuridad.Interfaces;
using Oscuridad.Clases;
using Oscuridad.Enumeraciones;
using System.Collections.Generic;

namespace Oscuridad.Estados
{
	public class EscenaWardInteriorPlantaSuperior: IEscenario
	{
		public EscenaWardInteriorPlantaSuperior(ControladoraEscenas managerRef)
		{
			if(Application.loadedLevelName != Escenas.EscenaWardInteriorPlantaSuperior.ToString())
			{
				Application.LoadLevel(Escenas.EscenaWardInteriorPlantaSuperior.ToString());
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
			GameCenter.InstanceRef.controladoraJuego.escenaActual = GameCenter.InstanceRef.controladoraJuego.Cargar_Escena(Escenas.EscenaWardInteriorPlantaSuperior);
			
			//Inicializamos la escena
			GameCenter.InstanceRef.controladoraGUI.Activar_Fade();
			GameCenter.InstanceRef.Inicializar_Escenario ();
		}
	}
}

