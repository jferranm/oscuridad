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
	public Escenas escenaVistaCamara;

	private RaycastHit hit;

	void OnEnabled()
	{

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
		if (GameCenter.InstanceRef.controladoraJugador.EstadoJugador.Equals(EstadosJugador.enEspera))
		{
			if (Input.GetMouseButtonDown(0))
			{
				try
				{
					int layerMask = 1 << 8;

					Ray rayo = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
					if(Physics.Raycast(rayo, out hit, Mathf.Infinity, layerMask))
					{
						GameCenter.InstanceRef.controladoraJuego.interactuablePulsado =  GameCenter.InstanceRef.controladoraJuego.escenaActual.Buscar_Interactuable(hit.collider.tag.ToString());

						try
						{
							GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoAcertarPulsar);
							GameCenter.InstanceRef.controladoraJugador.EstadoJugador = EstadosJugador.enZoomIn;
						}
						catch
						{
							GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
						}
					}
					else
					{
						GameCenter.InstanceRef.controladoraSonidos.Lanzar_Fx(GameCenter.InstanceRef.controladoraSonidos.sonidoFalloPulsar);
					}
				} catch {}
			
			}
		}
	}
}
