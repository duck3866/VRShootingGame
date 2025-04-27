using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletImpact;
    
    private ParticleSystem _bulletEffect;
    private AudioSource _bulletAudio;

    public Transform crosshair;
    
    private void Start()
    {
        _bulletEffect = bulletImpact.GetComponent<ParticleSystem>();
        _bulletAudio = bulletImpact.GetComponent<AudioSource>();
    }

    private void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair);
        
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
            
            _bulletAudio.Stop();
            _bulletAudio.Play();
            
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);

            int playerLayer = 1 << LayerMask.NameToLayer("Player");
            int towerLayer = 1 << LayerMask.NameToLayer("Tower");
            int layerMask = playerLayer | towerLayer;

            if (Physics.Raycast(ray, out var hitInfo, 200f, ~layerMask))
            {
                _bulletEffect.Stop();
                _bulletEffect.Play();
                
                bulletImpact.position = hitInfo.point;
                bulletImpact.forward = hitInfo.normal;

                if (hitInfo.transform.name.Contains("Drone"))
                {
                    DroneAI drone = hitInfo.transform.GetComponent<DroneAI>();
                    if (drone)
                    {
                        drone.OnDamageProcess();
                    }
                }
            }
        }
    }
}
