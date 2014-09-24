using UnityEngine;
using Oscuridad.Interfaces;
using Oscuridad.Personajes;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena3: IEscenario
	{
		private ControladoraEscenas manager;

		public Escena3(ControladoraEscenas managerRef)
		{
			manager = managerRef;

			if(Application.loadedLevelName != EstadoJuego.Escena3.ToString())
			{
				Application.LoadLevel(EstadoJuego.Escena3.ToString());
			}

			InicializarDatos ();
		}
		
		public void InicializarDatos()
		{
			//GUI a Mostrar
			GameCenter.InstanceRef.controladoraJugador.Cambiar_Estado (EstadosJugador.enMenus);
		}
		
		public void EstadoUpdate()
		{
			
		}
		
		public void Mostrar()
		{
			//GUI.skin = ControladorEstadoGUI.instanceRef.guiSkinJuego;

			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), GameCenter.InstanceRef.controladoraGUI.texturaMenuHojaPersonaje, ScaleMode.StretchToFill);
			
			//Area de Botones
			GUILayout.BeginArea (new Rect(Screen.width-150,Screen.height-150,125,150));
				
				//Boton de Seleccionar
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.texturaBotonSeleccionar))
				{
					//Creamos el personaje seleccionado
					Seleccion_Personaje();

					//Lanzamos la escena de Inicio
					GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
					manager.IrEscena10();
				}

				//Boton de volver
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.texturaBotonAtras))
				{
					GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
					manager.IrEscena1();
				}

			GUILayout.EndArea ();		
		}

		public void Seleccion_Personaje()
		{
			Utils util = new Utils();
			switch (GameCenter.InstanceRef.controladoraJugador.Devolver_Nombre_Jugador()) 
			{
				case "Maria Gibbs":	util.crearHojaPersonaje(Personaje.MarlaGibbs);
									break;

				case "Warren Bedford": 	util.crearHojaPersonaje(Personaje.WarrenBedford);
										break;

				case "Robert Duncan":	util.crearHojaPersonaje(Personaje.RobertDuncan);
										break;
			}
			util.crearHojaPersonaje(Personaje.MarlaGibbs);
			util.Inicializar_XML();
		}

		public void NivelCargado()
		{
		}
	}
}
