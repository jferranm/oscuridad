using UnityEngine;
using System.Collections;

public class ObjetoAnimacion : MonoBehaviour 
{
	public Animator animacion;
	public string nombreAnimacion;

	void Start ()
	{
		animacion.Stop();
	}

	public void Ejecutar_Animacion()
	{
		//animacion.StartPlayback ();
		animacion.Play (nombreAnimacion);
	}
}
