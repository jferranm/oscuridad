using UnityEngine;
using System.Collections;
using Oscuridad.Clases;

public class Eventos : MonoBehaviour 
{
	public void BotonComenzar()
	{
		GameCenter.InstanceRef.controladoraEscenas.IrEscena1();
	}
}
