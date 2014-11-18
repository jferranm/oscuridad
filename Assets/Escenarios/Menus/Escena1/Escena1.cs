using UnityEngine;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using System.IO;

namespace Oscuridad.Estados
{
	public class Escena1: IEscenario
	{
		public Escena1(ControladoraEscenas managerRef)
		{
			if(Application.loadedLevelName != Escenas.Escena1.ToString())
			{
				Application.LoadLevel(Escenas.Escena1.ToString());
			}
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
			//GUI.skin = ControladorEstadoGUI.instanceRef.guiSkinJuego;

			//GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), GameCenter.InstanceRef.controladoraGUI.texturaMenuPersonaje, ScaleMode.StretchToFill);
			
			//Area de Botones
			/*GUILayout.BeginArea (new Rect((Screen.width/2)-148,(Screen.height/2)-100,256,220));
			
				//Boton de Personaje 1
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.texturaBotonMarlaGibbs))
				{
					GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
					GameCenter.InstanceRef.controladoraJugador.Nombre_Jugador("Maria Gibbs");
					manager.IrEscena3();
				}
				
				///Boton de Personaje 2
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.texturaBotonWarrenBedford))
				{
					GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
					GameCenter.InstanceRef.controladoraJugador.Nombre_Jugador("Warren Bedford");
					manager.IrEscena3();//manager.CambiarEstado(new EstadoEscena3(manager));
				}
				
				//Boton de Personaje 3
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.texturaBotonRobertDuncan))
				{
					GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
					GameCenter.InstanceRef.controladoraJugador.Nombre_Jugador("Robert Duncan");
					manager.IrEscena3();
				}
				
			GUILayout.EndArea ();

			//Area de Botones
			GUILayout.BeginArea (new Rect(Screen.width-150,Screen.height-100,125,75));
				//Boton de Personaje 3
				if (GUILayout.Button(GameCenter.InstanceRef.controladoraGUI.texturaBotonAtras))
				{
					//TODO: Pasarle valores al constructor de la escena del personaje con los valores del personaje 3
					GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
					manager.IrMenuPrincipal();
				}
			GUILayout.EndArea ();*/

		}

		public void NivelCargado()
		{
		}
	}
}