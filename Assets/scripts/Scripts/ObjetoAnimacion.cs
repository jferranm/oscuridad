using UnityEngine;
using System.Collections;

public class ObjetoAnimacion : MonoBehaviour 
{
	public Animation animation;
    public AnimationClip clip;
	public AudioClip clipSonido;

	void Start()
	{
		clip.legacy = true;
	}

	public void Ejecutar_Animacion()
	{
		animation.clip = clip;
        animation.AddClip(clip, clip.name);

        animation.Play(clip.name);
	}

	public void Ejecutar_Animacion_Con_Sonido()
	{
		Ejecutar_Animacion ();
		GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(clipSonido);
	}
}
