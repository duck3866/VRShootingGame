using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI;
    private LineRenderer _lineRenderer;
    private Vector3 _originScale = Vector3.one * 0.02f;
    public bool isWarp = false;
    public float warpTime = 0.1f;
    public PostProcessVolume post;
    
    private void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            _lineRenderer.enabled = true;
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            _lineRenderer.enabled = false;
            if (teleportCircleUI.gameObject.activeSelf)
            {
                if (isWarp == false)
                {
                    GetComponent<CharacterController>().enabled = false;
                    transform.position = teleportCircleUI.position + Vector3.up;
                    GetComponent<CharacterController>().enabled = true;
                }
                else
                {
                    StartCoroutine(Warp());
                }
            }
            teleportCircleUI.gameObject.SetActive(false);
        }
        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            if (Physics.Raycast(ray, out var hitInfo, 200, layer))
            {
                _lineRenderer.SetPosition(0, ray.origin);
                _lineRenderer.SetPosition(1, hitInfo.point);
                
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitInfo.point;
                teleportCircleUI.forward = hitInfo.normal;
                teleportCircleUI.localScale = _originScale * Mathf.Max(1, hitInfo.distance);
            }
            else
            {
                _lineRenderer.SetPosition(0, ray.origin);
                _lineRenderer.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Warp()
    {
        MotionBlur blur;
        Vector3 pos = transform.position;
        Vector3 targetPos = teleportCircleUI.position + Vector3.up;
        float currentTime = 0;
        post.profile.TryGetSettings<MotionBlur>(out blur);
        blur.active = true;
        GetComponent<CharacterController>().enabled = false;

        while (currentTime < warpTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, targetPos, currentTime / warpTime);

            yield return null;
        }

        transform.position = teleportCircleUI.position + Vector3.up;
        GetComponent<CharacterController>().enabled = true;
        blur.active = false;
    }
}
