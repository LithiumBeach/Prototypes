using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace dd
{
    //responsible for drawing itself, and processing events.
    public class DialogNode
    {
        public Rect m_Rect;
        public string m_Title = "Dialog Node";
        private bool m_IsMoving = false;

        public GUIStyle m_Style; //current style
        public GUIStyle m_DefaultNodeStyle;
        public GUIStyle m_SelectedNodeStyle;
        public GUIContent m_GuiContent;//for the whole node.

        public DialogConnectionPoint m_InPoint;
        public DialogConnectionPoint m_OutPoint;

        //TODO: ISelectable
        public bool m_IsSelected;

        public Rect m_SpeechRect;

        public Action<DialogNode> OnRemoveNode;

        //data variables:
        public string m_SpeechText = "";

        public DialogNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedNodeStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<DialogConnectionPoint> OnClickInPoint, Action<DialogConnectionPoint> OnClickOutPoint, Action<DialogNode> OnClickRemoveNode)
        {
            float width = 200;
            float height = 250;
            m_Rect = new Rect(position.x - width*.5f, position.y - height*.5f, width, height);
            m_Style = nodeStyle;
            m_DefaultNodeStyle = nodeStyle;
            m_SelectedNodeStyle = selectedNodeStyle;
            //m_GuiContent = new GUIContent("Dialog Node", "this is a tooltip");
            m_GuiContent = new GUIContent("Dialog Node");//set the title

            m_InPoint = new DialogConnectionPoint(this, EConnectionPointType.In, inPointStyle, OnClickInPoint);
            m_OutPoint = new DialogConnectionPoint(this, EConnectionPointType.Out, outPointStyle, OnClickOutPoint);

            //Speech Rect
            float speechWidth = 180;
            float speechHeight = 150;
            m_SpeechRect = new Rect(position.x - speechWidth * .5f, position.y - height*.5f + 20, speechWidth, speechHeight);

            //Actions
            OnRemoveNode = OnClickRemoveNode;
        }

        public void Move(Vector2 delta)
        {
            m_Rect.position += delta;
            m_SpeechRect.position += delta;
        }

        public void Draw()
        {
            m_InPoint.Draw();
            m_OutPoint.Draw();
            GUI.Box(m_Rect, m_GuiContent, m_Style);
            m_SpeechText = GUI.TextArea(m_SpeechRect, m_SpeechText, 500);
        }

        /// <returns>needs repaint?</returns>
        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (m_Rect.Contains(e.mousePosition))
                        {
                            m_IsMoving = true;

                            m_IsSelected = true;
                            m_Style = m_SelectedNodeStyle;

                            GUI.changed = true;
                        }
                        else
                        {
                            m_IsSelected = false;
                            m_Style = m_DefaultNodeStyle;

                            GUI.changed = true;
                        }
                    }
                    else if (e.button == 1 && m_IsSelected && m_Rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    m_IsMoving = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && m_IsMoving)
                    {
                        Move(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, HandleRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void HandleRemoveNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }
    }
}