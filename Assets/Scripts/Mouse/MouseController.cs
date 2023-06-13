using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    public Texture2D cursorTexture; // 自定义鼠标图标
    public Vector2 hotspot = Vector2.zero; // 鼠标热点位置

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    private void Update()
    {
       
    }

}
