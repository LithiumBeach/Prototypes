﻿using System;
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
        public GUIStyle m_ActorDropdownGUIStyle = "toolbarDropDown";

        public DialogConnectionPoint m_InPin;
        public DialogConnectionPoint m_OutPin;

        //TODO: ISelectable
        public bool m_IsSelected;

        public Rect m_LocalizedTextDisplayRect;
        public Rect m_IDRect;
        public Rect m_SpeakerRect;
        public Rect m_SpeakerIndexRect;

        //misleading names, but calls back to existing function in DSWindow
        public Action<DialogConnectionPoint> m_OnClickDownInPoint;
        public Action<DialogConnectionPoint> m_OnClickDownOutPoint;
        public Action<DialogConnectionPoint> m_OnClickReleaseInPoint;
        public Action<DialogConnectionPoint> m_OnClickReleaseOutPoint;
        public Action<DialogNode> m_OnBecomeStartNode;

        public Action<DialogNode> OnRemoveNode;

        //data variables:
        public string m_IDText = "";
        public string m_LocalizedText = "";

        public int m_NodeID = -1;
        public int m_SpeakerIndex = 0;

        public readonly float m_Width = 200;
        public readonly float m_Height = 250;

        public Vector2 GetPositionForSave()
        {
            //in the constructor, we start with an offset and update as normal.
            //we need to un-do that offset at the end, so saving doesn't shift upon load.
            return new Vector2(m_Rect.position.x + m_Width * .5f, m_Rect.position.y + m_Height * .5f);
        }

        public DialogNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedNodeStyle, GUIStyle inPointStyleNormal, GUIStyle inPointStylePressed, GUIStyle outPointStyleNormal, GUIStyle outPointStylePressed,
            Action<DialogConnectionPoint> onClickDownInPoint, Action<DialogConnectionPoint> onClickDownOutPoint, Action<DialogConnectionPoint> onClickReleaseInPoint, Action<DialogConnectionPoint> onClickReleaseOutPoint,
            Action<DialogNode> onClickRemoveNode, Action<DialogNode> onBecomeStartNode)
        {
            m_Rect = new Rect(position.x - m_Width * .5f, position.y - m_Height * .5f, m_Width, m_Height);
            m_Style = new GUIStyle(nodeStyle);
            m_DefaultNodeStyle = nodeStyle;
            m_SelectedNodeStyle = new GUIStyle(selectedNodeStyle);

            m_DisabledTextBoxGuiStyle.wordWrap = true;

            //m_GuiContent = new GUIContent("Dialog Node", "this is a tooltip");
            m_GuiContent = new GUIContent();//set the title

            m_InPin = new DialogConnectionPoint(this, EConnectionPointType.In, inPointStyleNormal, inPointStylePressed);
            m_OutPin = new DialogConnectionPoint(this, EConnectionPointType.Out, outPointStyleNormal, outPointStylePressed);

            //ID inputfield
            float idRectWidth = 180;
            float idRectHeight = 16;
            m_IDRect = new Rect(position.x - idRectWidth * .5f, position.y - m_Height * .5f + 20, idRectWidth, idRectHeight);

            //Speech Rect
            float speechWidth = 180;
            float speechHeight = 100;
            m_LocalizedTextDisplayRect = new Rect(position.x - speechWidth * .5f, position.y - m_Height * .5f + idRectHeight + 30, speechWidth, speechHeight);

            //speaker dropdown rect
            float speakerWidth = 130;
            float speakerHeight = 20;
            m_SpeakerRect = new Rect(position.x - speakerWidth * .5f - 25, position.y - m_Height * .5f + idRectHeight + speechHeight + 34, speakerWidth, speakerHeight);
            float speakerIndexWidth = 40;
            float speakerIndexHeight = 19;
            m_SpeakerIndexRect = new Rect(m_SpeakerRect.position.x + speakerWidth + 10, position.y - m_Height * .5f + idRectHeight + speechHeight + 33, speakerIndexWidth, speakerIndexHeight);

            //Actions
            m_OnClickDownInPoint = onClickDownInPoint;
            m_OnClickDownOutPoint = onClickDownOutPoint;
            m_OnClickReleaseInPoint = onClickReleaseInPoint;
            m_OnClickReleaseOutPoint = onClickReleaseOutPoint;
            OnRemoveNode = onClickRemoveNode;
            m_OnBecomeStartNode = onBecomeStartNode;

            //default values
            m_IDText = "";
        }

        public virtual void Move(Vector2 delta)
        {
            m_Rect.position += delta;
            m_LocalizedTextDisplayRect.position += delta;
            m_IDRect.position += delta;
            m_SpeakerRect.position += delta;
            m_SpeakerIndexRect.position += delta;
        }

        public void Draw(List<string> speakers, bool isStartNode)
        {
            if (!isStartNode)
            {
                m_InPin.Draw(); 
            }
            m_OutPin.Draw();

            //TODO: you can make each node a certain color from their actor id.
            //Color cacheColor = GUI.color;
            //GUI.color = Color.magenta;
            //background box
            GUI.Box(m_Rect, m_GuiContent, m_Style);
            //GUI.color = cacheColor;

            //ID writable textbox
            GUI.SetNextControlName("m_IDText" + m_NodeID.ToString());
            m_IDText = GUI.TextArea(m_IDRect, m_IDText, 10);

            if (GUIUtility.keyboardControl != 0)
            {
                if (GUI.GetNameOfFocusedControl() == "m_IDText" + m_NodeID.ToString())
                {
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

            #region ID Value Validation
            m_IDText = Regex.Replace(m_IDText, @"[^0-9-]", "");

            int id = -1;
            bool containsMinus = m_IDText.Contains("-");
            if (containsMinus && m_IDText.Length > 1)
            {
                m_IDText = "-1";
                id = -1;
            }
            else if (containsMinus)
            {
                id = -1;
            }
            else
            {
                if (m_IDText.Length == 0)
                {
                    id = -1;
                }
                else if (int.TryParse(m_IDText, out id))
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
            float oldHeight = m_LocalizedTextDisplayRect.height;
            m_LocalizedTextDisplayRect.height = m_DisabledTextBoxGuiStyle.CalcHeight(new GUIContent(m_LocalizedText), m_LocalizedTextDisplayRect.width);
            if (oldHeight != m_LocalizedTextDisplayRect.height)
            {
                OnHeightChange(oldHeight, m_LocalizedTextDisplayRect.height);
            }
            GUI.Label(m_LocalizedTextDisplayRect, m_LocalizedText, m_DisabledTextBoxGuiStyle);

            //speaker dropdown
            //int prevSpeakerIndex = m_SpeakerIndex;
            m_SpeakerIndex = EditorGUI.Popup(m_SpeakerRect, m_SpeakerIndex, speakers.ToArray(), m_ActorDropdownGUIStyle);
            GUI.Label(m_SpeakerIndexRect, "i:" + m_SpeakerIndex.ToString(), m_DisabledTextBoxGuiStyle);

            #region temporarily display node's id for debugging
            //string temp = m_NodeID.ToString();
            //Rect tmpRect = new Rect(m_IDRect);
            //tmpRect.height *= .45f;
            //tmpRect.position -= new Vector2(0, tmpRect.height * 2);
            //GUI.Label(new Rect(tmpRect), temp, m_DisabledTextBoxGuiStyle);
            #endregion
        }

        private void OnHeightChange(float oldHeight, float newHeight)
        {
            float heightDelta = newHeight - oldHeight;
            m_Rect.height += heightDelta;
            m_SpeakerRect.y += heightDelta;
            m_SpeakerIndexRect.y += heightDelta;
        }

        private static int s_OpenContextMenuNextFrameID = -1;

        /// <returns>needs repaint?</returns>
        public bool ProcessEvents(Event e)
        {
            #region Handle Context Menu Opening
            if (s_OpenContextMenuNextFrameID != -1)
            {
                if (s_OpenContextMenuNextFrameID == m_NodeID)
                {
                    ProcessContextMenu();
                    s_OpenContextMenuNextFrameID = -1;
                }
                else
                {
                    m_IsSelected = false;
                    m_Style = m_DefaultNodeStyle;

                    GUI.changed = true;
                    return true;
                }
            }
            #endregion

            bool mouseInNode = m_Rect.Contains(e.mousePosition);
            //mouseInNode && is a minor optimization
            bool mouseInIDTextBox = mouseInNode && m_IDRect.Contains(e.mousePosition);
            bool mouseInInPin = m_InPin.m_Rect.Contains(e.mousePosition);
            bool mouseInOutPin = m_OutPin.m_Rect.Contains(e.mousePosition);

            switch (e.type)
            {
                case EventType.MouseDown:

                    //if the id text box is selected AND any mouse button is pressed outside the ID text box, deselect it
                    if (!mouseInIDTextBox)
                    {
                        GUI.FocusControl(null);
                    }

                    //LMB down
                    if (e.button == 0)
                    {
                        if (mouseInInPin)
                        {
                            m_OnClickDownInPoint(m_InPin);
                            e.Use();
                        }
                        else if (mouseInOutPin)
                        {
                            m_OnClickDownOutPoint(m_OutPin);
                            e.Use();
                        }
                        else if (mouseInNode)
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
                    //RMB down
                    else if (e.button == 1)
                    {
                        if (mouseInNode)
                        {
                            m_IsSelected = true;
                            m_Style = m_SelectedNodeStyle;
                            GUI.changed = true;

                            s_OpenContextMenuNextFrameID = m_NodeID;
                            e.Use();
                        }
                        else
                        {
                            m_IsSelected = false;
                            m_Style = m_DefaultNodeStyle;

                            GUI.changed = true;
                        }
                    }
                    break;

                case EventType.MouseUp:
                    m_IsMoving = false;
                    if (e.button == 0)
                    {
                        if (mouseInInPin)
                        {
                            m_OnClickReleaseInPoint(m_InPin);
                        }
                        else if (mouseInOutPin)
                        {
                            m_OnClickReleaseOutPoint(m_OutPin);
                        }
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && m_IsMoving)
                    {
                        Move(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
                case EventType.KeyUp:
                    //when delete key  is pressed && this node is selected && the user is not in the scope of a textfield
                    if (e.keyCode == KeyCode.Delete && m_IsSelected == true && GUIUtility.keyboardControl == 0)
                    {
                        HandleRemoveNode();
                        GUI.changed = true;
                    }
                    break;
            }
            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, HandleRemoveNode);
            genericMenu.AddItem(new GUIContent("Make this Start Node"), false, HandleBecomeStartNode);
            genericMenu.ShowAsContext();
        }

        private void HandleRemoveNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }

        private void HandleBecomeStartNode()
        {
            if (m_OnBecomeStartNode != null)
            {
                m_OnBecomeStartNode(this);
            }
        }

        //TODO: Select(), Deselect()
        //manage which node(s) is/are selected.
    }
}