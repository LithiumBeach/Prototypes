using UnityEngine;
//http://martinecker.com/martincodes/unity-editor-window-zooming/

namespace dd
{
    public class EditorZoomArea
    {
        private const float kEditorWindowTabHeight = 20.0f;
        private static Matrix4x4 _prevGuiMatrix;

        public static Rect Begin(float zoomScale, Rect screenCoordsArea)
        {
            GUI.EndGroup();        // End the group Unity begins automatically for an EditorWindow to clip out the window tab. This allows us to draw outside of the size of the EditorWindow.

            Rect clippedArea = screenCoordsArea.ScaleSizeBy(1.0f / zoomScale, screenCoordsArea.TopLeft());
            clippedArea.y += kEditorWindowTabHeight;
            GUI.BeginGroup(clippedArea);

            _prevGuiMatrix = GUI.matrix;

            GUIUtility.ScaleAroundPivot(zoomScale * Vector2.one, screenCoordsArea.TopLeft());

            //proof that to zoom about the mouse position, this function will not suffice.
            //GUIUtility.ScaleAroundPivot(zoomScale * Vector2.one, screenCoordsArea.center);
            //GUI.matrix = Matrix4x4.Translate(((screenCoordsArea.center * zoomScale) - screenCoordsArea.center)) * GUI.matrix;

            return clippedArea;
        }

        public static void End()
        {
            GUI.matrix = _prevGuiMatrix;
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, kEditorWindowTabHeight, Screen.width, Screen.height));
        }
    }
}