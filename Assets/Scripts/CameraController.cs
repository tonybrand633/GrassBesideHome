using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class CameraController : XSingletonHelper<CameraController>
{
    public float speed = 8.0f;
    public float boundOffset = 1.8f;
    public bool moveCameraByMouse = false;
    // ��ȡ��Ļ�߽�
    public Vector3 screenBounds = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);

    public void Update() 
    {
        if (moveCameraByMouse) 
        {
            MoveCameraByMouse();
        }
    }

    public void MoveCameraByMouse() 
    {
        Vector3 mousePos = Input.mousePosition;
        // �����������Ļ���ƶ�
        mousePos.x = Mathf.Clamp(mousePos.x, -screenBounds.x, screenBounds.x);
        mousePos.y = Mathf.Clamp(mousePos.y, -screenBounds.y, screenBounds.y);

        if (mousePos.x <= boundOffset)
        {
            //Debug.Log("���Ӵ�������Ļ���Ե");
            MoveCameraDirection(Vector2.left);
        }
        else if (mousePos.x >= screenBounds.x - boundOffset)
        {
            //Debug.Log("���Ӵ�������Ļ�ұ�Ե");
            MoveCameraDirection(Vector2.right);
        }
        else if (mousePos.y <= boundOffset)
        {
            //Debug.Log("���Ӵ�������Ļ�±�Ե");
            MoveCameraDirection(Vector2.down);
        }
        else if (mousePos.y >= screenBounds.y - boundOffset)
        {
            //Debug.Log("���Ӵ�������Ļ�ϱ�Ե");
            MoveCameraDirection(Vector2.up);
        }
        // �����λ������Ϊ���ƺ��λ��
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void MoveCameraDirection(Vector3 dir) 
    {
        Camera.main.transform.position += dir * speed * Time.deltaTime;
    }
}
