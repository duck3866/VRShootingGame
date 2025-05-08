using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    private CharacterController _cc;
    public float gravity = -20f;
    private float yVelocity = 0.0f;
    public float jumpPower = 5.0f;
    private Vector3 angle;
    public float sensitivity = 200;
    
    private void Start()
    {
        _cc = GetComponent<CharacterController>();
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float h = ARAVRInput.GetAxisLeft("Horizontal");
        float v = ARAVRInput.GetAxisLeft("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        
        dir = Camera.main.transform.TransformDirection(dir);
        
        yVelocity += gravity * Time.deltaTime;
        
        float x = ARAVRInput.GetAxisRight("Horizontal");
        float y = ARAVRInput.GetAxisRight("Vertical");

        // 2. 방향이 필요하다.
        // 이동 공식에 대입하여 각 속성별로 회전 값을 누적 시킨다.
        angle.x += x * sensitivity * Time.unscaledDeltaTime;
        angle.y += y * sensitivity * Time.unscaledDeltaTime;

        angle.y = Mathf.Clamp(angle.y, -90, 90);
        // 3. 회전 시키고 싶다.
        // 카메라의 회전값에 새로 만들어진 회전 값을 할당한다.
        transform.eulerAngles = new Vector3(0, angle.x, transform.eulerAngles.z);
        
        if (_cc.isGrounded)
        {
            yVelocity = 0;
        }

        // if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
        // {
        //     yVelocity = jumpPower;
        // }
        
        dir.y = yVelocity;
        
        _cc.Move(dir * (speed * Time.unscaledDeltaTime));
    }
}
