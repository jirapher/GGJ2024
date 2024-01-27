using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSelection : MonoBehaviour
{
    RaycastHit2D hit;
    public bool isDragging = false;
    public Vector3 mousePos;
    public Color dragBoxCol;
    public InputManager inputMan;
    static Texture2D whiteTex;
    private Camera mainCam;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (whiteTex == null)
            {
                whiteTex = new Texture2D(1, 1);
                whiteTex.SetPixel(0, 0, Color.white);
                whiteTex.Apply();
            }

            return whiteTex;
        }
    }

    private void Start()
    {
        mainCam = Camera.main;
    }
    private void OnGUI()
    {
        if (isDragging)
        {
            var rect = GetScreenRect(mousePos, Input.mousePosition);
            DrawScreenRect(rect, dragBoxCol);
            DrawScreenRectBorder(rect, 1, Color.green);
        }

    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        //Top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public bool IsWithinSelectionBounds(Transform transform)
    {
        if (isDragging)
        {
            var viewportBounds = GetViewportBounds(mainCam, mousePos, Input.mousePosition);
            return viewportBounds.Contains(mainCam.WorldToViewportPoint(transform.position));
        }
        else
        {
            return false;
        }

    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = camera.ScreenToViewportPoint(screenPosition1);
        var v2 = camera.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();

        bounds.SetMinMax(min, max);
        return bounds;
    }
}
