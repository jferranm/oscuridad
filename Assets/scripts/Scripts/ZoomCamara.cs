using UnityEngine;
using Oscuridad.Estados;
using Oscuridad.Enumeraciones;
using Oscuridad.Clases;

public class ZoomCamara : MonoBehaviour 
{
	[HideInInspector]
	public Vector3 posicionInicial;
	[HideInInspector]
	public Quaternion rotacionInicial;

	private RaycastHit hit;

	void OnEnable()
	{
		GameCenter.InstanceRef.controladoraJugador.zoomCamaraRef = this;
	}

	void Start() 
	{
		posicionInicial.x = transform.position.x;
		posicionInicial.y = transform.position.y;
		posicionInicial.z = transform.position.z;
		
		rotacionInicial = transform.rotation;
	}

	void Update()
	{
		if (GameCenter.InstanceRef.controladoraJugador.Devolver_Estado() == EstadosJugador.enEspera)
		{
			if (Input.GetMouseButtonDown(0))
			{
				try
				{
					Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
					if(Physics.Raycast(rayo, out hit, Mathf.Infinity))
					{
						GameCenter.InstanceRef.controladoraJuego.objetoPulsado =  GameCenter.InstanceRef.controladoraJuego.escenaActual.Buscar_Objeto(hit.collider.tag.ToString());
						try
						{
							ObjetoInteractuablev2 objetoInteractuableRef = GameObject.FindGameObjectWithTag(hit.collider.tag.ToString()).GetComponent<ObjetoInteractuablev2>();
							ObjetoBase pruebaObjeto = GameCenter.InstanceRef.controladoraJuego.escenaActual.Buscar_Objeto(hit.collider.tag.ToString());
							pruebaObjeto.PosicionNueva = objetoInteractuableRef.posicionNueva;
							pruebaObjeto.RotacionNueva = objetoInteractuableRef.rotacionNueva;
							pruebaObjeto.Smooth = objetoInteractuableRef.smooth;

							//TODO: arreglar el tema de sonidos
							GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoAcertarPulsar);
							GameCenter.InstanceRef.controladoraJugador.Cambiar_Estado(EstadosJugador.enZoomIn);
						}
						catch
						{
							GameCenter.InstanceRef.audio.PlayOneShot(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
						}
					}
				} catch {}
			
			}
		}
	}
}
