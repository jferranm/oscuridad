using UnityEngine;
using System.Collections;
using Oscuridad.Interfaces;
using Oscuridad.Enumeraciones;
using Oscuridad.Estados;

[System.Serializable]
public class ControladoraEscenas
{
	#region VARIABLES

	public IEscenario estadoActivo;

	#endregion

	#region CONSTRUCTORES

	public ControladoraEscenas()
	{
	}

	#endregion

	#region METODOS

	public void Update()
	{
		if(estadoActivo != null)
		{
			estadoActivo.EstadoUpdate();
		}
	}
	
	public void OnLevelWasLoaded(int level)
	{
		if (estadoActivo != null)
		{
			estadoActivo.NivelCargado();
		}
	}
	
	private void CambiarEstado(IEscenario nuevoEstado)
	{
		estadoActivo = nuevoEstado;
	}
	
	public void IrMenuPrincipal()
	{
		estadoActivo = new MenuPrincipal(this);
	}
	
	public void IrEscena1()
	{
		estadoActivo = new Escena1(this);
	}
	
	public void IrEscena2()
	{
		estadoActivo = new Escena2(this);
	}
	
	public void IrEscena3()
	{
		estadoActivo = new Escena3(this);
	}
	
	public void IrEscenaWardExterior()
	{
		GameCenter.InstanceRef.controladoraJuego.localizacionActual = Localizaciones.CasaFamiliarWard;
		estadoActivo = new EscenaWardExterior (this);
	}

	public void IrEscenaWardInteriorPlantaBaja()
	{
		GameCenter.InstanceRef.controladoraJuego.localizacionActual = Localizaciones.CasaFamiliarWard;
		estadoActivo = new EscenaWardInteriorPlantaBaja (this);
	}

	public void IrEscenaWardInteriorPlantaSuperior()
	{
		GameCenter.InstanceRef.controladoraJuego.localizacionActual = Localizaciones.CasaFamiliarWard;
		estadoActivo = new EscenaWardInteriorPlantaSuperior (this);
	}
	
	public void CambiarSceneSegunEnum(Escenas temp)
	{
		switch (temp) 
		{
			case Escenas.MenuPrincipal:
			{
				IrMenuPrincipal ();
				break;
			}

			case Escenas.Escena1:
			{
				IrEscena1 ();
				break;
			}

			case Escenas.Escena2:
			{
				IrEscena2 ();
				break;
			}
		
			case Escenas.Escena3:
			{
				IrEscena3 ();
				break;
			}

			case Escenas.EscenaWardExterior:
			{
				IrEscenaWardExterior();
				break;
			}

			case Escenas.EscenaWardInteriorPlantaBaja:
			{
				IrEscenaWardInteriorPlantaBaja();
				break;
			}	

			case Escenas.EscenaWardInteriorPlantaSuperior:
			{
				IrEscenaWardInteriorPlantaSuperior();
				break;
			}
		}
	}

	public void CambiarSceneSegunEnum(Localizaciones localizacion)
	{
		switch (localizacion) 
		{
			case Localizaciones.CasaFamiliarWard:
			{
				IrEscenaWardExterior();
				break;
			}
		}
	}

	#endregion
}
