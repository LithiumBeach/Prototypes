using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        public GUIStyle m_DisabledTextBoxGuiStyle = "TextField";

        public DialogConnectionPoint m_InPoint;
        public DialogConnectionPoint m_OutPoint;

        //TODO: ISelectable
        public bool m_IsSelected;

        public Rect m_LocalizedTextDisplayRect;
        public Rect m_IDRect;

        public Action<DialogNode> OnRemoveNode;

        //data variables:
        public string m_IDText = "";
        public string m_LocalizedText = "";

        public DialogNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedNodeStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
            Action<DialogConnectionPoint> OnClickInPoint, Action<DialogConnectionPoint> OnClickOutPoint, Action<DialogNode> OnClickRemoveNode)
        {
            float width = 200;
            float height = 250;
            m_Rect = new Rect(position.x - width * .5f, position.y - height * .5f, width, height);
            m_Style = nodeStyle;
            m_DefaultNodeStyle = nodeStyle;
            m_SelectedNodeStyle = selectedNodeStyle;

            //m_GuiContent = new GUIContent("Dialog Node", "this is a tooltip");
            m_GuiContent = new GUIContent();//set the title

            m_InPoint = new DialogConnectionPoint(this, EConnectionPointType.In, inPointStyle, OnClickInPoint);
            m_OutPoint = new DialogConnectionPoint(this, EConnectionPointType.Out, outPointStyle, OnClickOutPoint);

            //ID inputfield
            float idRectWidth = 180;
            float idRectHeight = 32;
            m_IDRect = new Rect(position.x - idRectWidth * .5f, position.y - height * .5f + 20, idRectWidth, idRectHeight);


            //Speech Rect
            float speechWidth = 180;
            float speechHeight = 150;
            m_LocalizedTextDisplayRect = new Rect(position.x - speechWidth * .5f, position.y - height * .5f + idRectHeight + 20, speechWidth, speechHeight);

            //Actions
            OnRemoveNode = OnClickRemoveNode;
        }

        public void Move(Vector2 delta)
        {
            m_Rect.position += delta;
            m_LocalizedTextDisplayRect.position += delta;
            m_IDRect.position += delta;
        }

        public void Draw()
        {
            m_InPoint.Draw();
            m_OutPoint.Draw();

            //background box
            GUI.Box(m_Rect, m_GuiContent, m_Style);

            //ID writable textbox
            GUI.SetNextControlName("m_IDText");
            m_IDText = GUI.TextArea(m_IDRect, m_IDText, 10);

            #region ID Value Validation
            m_IDText = Regex.Replace(m_IDText, @"[^0-9-]", "");

            int id = -1;
            if (m_IDText.Contains("-") && m_IDText.Length > 1)
            {
                m_IDText = "-1";
            }
            else
            {
                if (int.TryParse(m_IDText, out id))
                {
                }
                else if (m_IDText.Length == 10)
                {
                    id = int.MaxValue - 1;
                    m_IDText = id.ToString();
                }
            }
            #endregion

            //localized text readonly box
            GUI.SetNextControlName("m_LocalizedTextDisplayRect");

            m_LocalizedText = DialogDBSerializer.GetTextFromID(id);
            GUI.Label(m_LocalizedTextDisplayRect, m_LocalizedText, m_DisabledTextBoxGuiStyle);
        }

        /// <returns>needs repaint?</returns>
        public bool ProcessEvents(Event e)
        {
            bool mouseInIDTextBox = m_IDRect.Contains(e.mousePosition);

            switch (e.type)
            {
                case EventType.MouseDown:

                    //if the id text box is selected AND any mouse button is pressed outside the ID text box, deselect it
                    if (GUI.GetNameOfFocusedControl() == "m_IDText" && !mouseInIDTextBox)
                    {
                        GUI.FocusControl(null);
                    }

                    //LMB down
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