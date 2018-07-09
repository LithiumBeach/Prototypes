using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

//node editor basics tutorial:
//http://gram.gs/gramlog/creating-node-based-editor-unity/

namespace dd
{
    public class DialogSequenceWindow : EditorWindow
    {
        private List<DialogNode> m_Nodes;
        private List<DialogConnection> m_Connections;

        private GUIStyle m_NodeStyle;
        private GUIStyle m_FromPointStyle;
        private GUIStyle m_ToPointStyle;
        private GUIStyle m_SelectedNodeStyle;

        private DialogConnectionPoint m_SelectedFromPoint;
        private DialogConnectionPoint m_SelectedToPoint;

        private Vector2 m_Offset;//total offset from dragging
        private Vector2 m_Drag;//resets every frame: drag delta for current frame

        private DialogGraphData m_Data;

        [MenuItem("Window/Dialog Director")]
        public static void OpenWindow()
        {
            //get type of Scene View
            Type t = Type.GetType("UnityEditor.SceneView,UnityEditor.dll");
            //by default, dock next to scene view.
            EditorWindow window = GetWindow<DialogSequenceWindow>(new Type[] { t });
            window.titleContent = new GUIContent("Dialog Director");
        }

        public void Initialize(DialogGraphData data)
        {
            m_Data = data;

            //clear lists.
            m_Nodes = new List<DialogNode>();
            m_Connections = new List<DialogConnection>();

            //clear connection selection
            m_SelectedFromPoint = null;
            m_SelectedToPoint = null;

            if (data.m_Nodes != null)
            {
                //for each node
                for (int i = 0; i < data.m_Nodes.Count; i++)
                {
                    //create visual node from data
                    DialogNode newNode = OnClickAddNode(data.m_Nodes[i].m_Position);
                    newNode.m_IDText = data.m_Nodes[i].m_LocalizationID.ToString();
                    newNode.m_NodeID = data.m_Nodes[i].m_NodeID;
                }
                //this loop must occur after all nodes have been initialized
                for (int i = 0; i < data.m_Nodes.Count; i++)
                {
                    //create TO connection if applicable
                    if (data.m_Nodes[i].m_ToNodeID != 0)
                    {
                        CreateConnection(data.m_Nodes[i].m_NodeID, data.m_Nodes[i].m_ToNodeID);
                    }
                }
            }

            m_Offset = Vector2.zero;
            m_Drag = Vector2.zero;
        }

        public void OnUnitySave()
        {
            Debug.Assert(m_Data != null);

            m_Data.Clear();
            for (int nodeIndex = 0; nodeIndex < m_Nodes.Count; nodeIndex++)
            {
                int iterID = -1;
                if (!int.TryParse(m_Nodes[nodeIndex].m_IDText, out iterID))
                {
                    iterID = -1;
                }
                DialogNodeData dnd = new DialogNodeData(m_Nodes[nodeIndex].m_NodeID, iterID, m_Nodes[nodeIndex].GetPositionForSave() - m_Offset);

                m_Data.m_Nodes.Add(dnd);
            }

            for (int connectionIndex = 0; connectionIndex < m_Connections.Count; connectionIndex++)
            {
                //find the node data with the same ID as this connection's OUT node.
                DialogNodeData connectionFromNodeData = m_Data.m_Nodes.Find((item) => item.m_NodeID == m_Connections[connectionIndex].m_FromPoint.m_Node.m_NodeID);

                connectionFromNodeData.m_ToNodeID = m_Connections[connectionIndex].m_ToPoint.m_Node.m_NodeID;
            }
        }

        private void OnEnable()
        {
            m_NodeStyle = new GUIStyle();
            m_NodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            m_NodeStyle.border = new RectOffset(10, 10, 10, 10);
            m_NodeStyle.alignment = TextAnchor.UpperCenter;

            m_SelectedNodeStyle = new GUIStyle();
            m_SelectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            m_SelectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
            m_SelectedNodeStyle.alignment = TextAnchor.UpperCenter;

            m_ToPointStyle = new GUIStyle();
            m_ToPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            m_ToPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            m_ToPointStyle.border = new RectOffset(4, 4, 12, 12);

            m_FromPointStyle = new GUIStyle();
            m_FromPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            m_FromPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            m_FromPointStyle.border = new RectOffset(4, 4, 12, 12);

            //TODO: don't load this every time
            DialogDBSerializer.LoadDialogLines(CultureInfo.GetCultureInfo("en"));

            m_Offset = Vector2.zero;
            m_Drag = Vector2.zero;
        }

        private void OnGUI()
        {
            #region Handle Invalid Data
            //don't give a dialog graph unless you have data loaded. Prompt to load data.
            if (m_Data == null)
            {
                m_Data = (DialogGraphData)EditorGUI.ObjectField(new Rect(this.position.width * .5f - 250, this.position.height * .5f - 10, 500, 20), "Select Graph to Load", m_Data, typeof(DialogGraphData), false);
                if (m_Data != null)
                {
                    Initialize(m_Data);
                }
                return;
            }
            //if you have data loaded, but it hasn't been initialized yet.
            else if (m_Data.m_Nodes != null && m_Data.m_Nodes.Count > 0 && (m_Nodes == null || m_Nodes.Count == 0))
            {
                Initialize(m_Data);
                return;
            }
            #endregion

            DrawGrid(20f, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            m_Offset += m_Drag * 0.5f;
            Vector3 newOffset = new Vector3(m_Offset.x % gridSpacing, m_Offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes()
        {
            if (m_Nodes != null)
            {
                for (int i = 0; i < m_Nodes.Count; i++)
                {
                    m_Nodes[i].Draw();
                }
            }
        }

        private void DrawConnections()
        {
            if (m_Connections != null)
            {
                for (int i = 0; i < m_Connections.Count; i++)
                {
                    m_Connections[i].Draw();
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            m_Drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    //if we are creating a connection, and we click on not a pin, cancel the new connection.
                    if ((m_SelectedFromPoint == null || m_SelectedToPoint == null) && (m_SelectedFromPoint != null || m_SelectedToPoint != null))
                    {
                        ClearConnectionSelection();
                    }
                    if (e.button == 1)//Right Mouse Button
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                break;
                case EventType.MouseDrag:
                    if (e.button == 2)//middle mouse button
                    {
                        OnDrag(e.delta);
                    }
                break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (m_Nodes != null)
            {
                //backwards so nodes added last are drawn first
                for (int i = m_Nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = m_Nodes[i].ProcessEvents(e);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
            genericMenu.ShowAsContext();
        }

        private void OnDrag(Vector2 delta)
        {
            m_Drag = delta;

            if (m_Nodes != null)
            {
                for (int i = 0; i < m_Nodes.Count; i++)
                {
                    m_Nodes[i].Move(delta);
                }
            }

            GUI.changed = true;
        }

        private DialogNode OnClickAddNode(Vector2 mousePosition)
        {
            if (m_Nodes == null)
            {
                m_Nodes = new List<DialogNode>();
            }

            DialogNode node = new DialogNode(mousePosition, m_NodeStyle, m_SelectedNodeStyle, m_ToPointStyle, m_FromPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
            node.m_NodeID = m_Data.GetUniqueNodeID();
            m_Nodes.Add(node);
            GUI.changed = true;
            Repaint();
            return node;
        }

        private void OnClickInPoint(DialogConnectionPoint inPoint)
        {
            //if CTRL is pressed when clicked, remove all connections with this in point.
            if (Event.current.control)
            {
                ClearConnectionSelection();
                List<DialogConnection> toRemove = new List<DialogConnection>();
                for (int i = 0; i < m_Connections.Count; i++)
                {
                    if (m_Connections[i].m_ToPoint == inPoint)
                    {
                        toRemove.Add(m_Connections[i]);
                    }
                }
                for (int i = 0; i < toRemove.Count; i++)
                {
                    m_Connections.Remove(toRemove[i]);
                }
                return;
            }

            m_SelectedToPoint = inPoint;

            if (m_SelectedFromPoint != null)
            {
                if (m_SelectedFromPoint.m_Node != m_SelectedToPoint.m_Node)
                {
                    //check all existing connections, don't add a duplicate.
                    for (int i = 0; i < (m_Connections != null ? m_Connections.Count : 0); i++)
                    {
                        if ((m_Connections[i].m_ToPoint == inPoint && m_Connections[i].m_FromPoint == m_SelectedFromPoint) ||
                            (m_Connections[i].m_ToPoint == m_SelectedFromPoint && m_Connections[i].m_FromPoint == inPoint))
                        {
                            ClearConnectionSelection();
                            return;
                        }
                    }
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(DialogConnectionPoint outPoint)
        {
            //if CTRL is pressed when clicked, remove all connections with this out point.
            if (Event.current.control)
            {
                ClearConnectionSelection();
                List<DialogConnection> toRemove = new List<DialogConnection>();
                for (int i = 0; i < m_Connections.Count; i++)
                {
                    if (m_Connections[i].m_FromPoint == outPoint)
                    {
                        toRemove.Add(m_Connections[i]);
                    }
                }
                for (int i = 0; i < toRemove.Count; i++)
                {
                    m_Connections.Remove(toRemove[i]);
                }
                return;
            }

            m_SelectedFromPoint = outPoint;
            if (Event.current.control)
            {
                ClearConnectionSelection();
                return;
            }
            if (m_SelectedToPoint != null)
            {
                if (m_SelectedFromPoint.m_Node != m_SelectedToPoint.m_Node)
                {
                    //check all existing connections, don't add a duplicate.
                    for (int i = 0; i < (m_Connections != null ? m_Connections.Count : 0); i++)
                    {
                        if ((m_Connections[i].m_ToPoint == outPoint && m_Connections[i].m_FromPoint == m_SelectedToPoint) ||
                            (m_Connections[i].m_ToPoint == m_SelectedToPoint && m_Connections[i].m_FromPoint == outPoint))
                        {
                            ClearConnectionSelection();
                            return;
                        }
                    }
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickRemoveConnection(DialogConnection connection)
        {
            m_Connections.Remove(connection);
        }

        private void CreateConnection()
        {
            if (m_Connections == null)
            {
                m_Connections = new List<DialogConnection>();
            }

            DialogConnection existingDC = m_Connections.Find((item) => item.m_FromPoint.m_Node.m_NodeID == m_SelectedFromPoint.m_Node.m_NodeID);
            if (existingDC != null)
            {
                m_Connections.Remove(existingDC);
            }
            m_Connections.Add(new DialogConnection(m_SelectedFromPoint, m_SelectedToPoint, OnClickRemoveConnection));
        }
        private void CreateConnection(int fromID, int toID)
        {
            if (m_Connections == null)
            {
                m_Connections = new List<DialogConnection>();
            }

            DialogConnectionPoint outPoint = m_Nodes.Find((item) => item.m_NodeID == fromID).m_OutPoint;
            DialogConnectionPoint inPoint = m_Nodes.Find((item) => item.m_NodeID == toID).m_InPoint;

            m_Connections.Add(new DialogConnection(outPoint, inPoint, OnClickRemoveConnection));
        }

        private void DrawConnectionLine(Event e)
        {
            if (m_SelectedToPoint != null && m_SelectedFromPoint == null)
            {
                Handles.DrawBezier(
                    m_SelectedToPoint.m_Rect.center,
                    e.mousePosition,
                    m_SelectedToPoint.m_Rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (m_SelectedFromPoint != null && m_SelectedToPoint == null)
            {
                Handles.DrawBezier(
                    m_SelectedFromPoint.m_Rect.center,
                    e.mousePosition,
                    m_SelectedFromPoint.m_Rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private void ClearConnectionSelection()
        {
            m_SelectedToPoint = null;
            m_SelectedFromPoint = null;
        }

        private void OnClickRemoveNode(DialogNode node)
        {
            if (m_Connections != null)
            {
                List<DialogConnection> connectionsToRemove = new List<DialogConnection>();

                for (int i = 0; i < m_Connections.Count; i++)
                {
                    if (m_Connections[i].m_ToPoint == node.m_InPoint || m_Connections[i].m_FromPoint == node.m_OutPoint)
                    {
                        connectionsToRemove.Add(m_Connections[i]);
                    }
                }

                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    m_Connections.Remove(connectionsToRemove[i]);
                }

                connectionsToRemove = null;
            }

            m_Nodes.Remove(node);
        }


    }
}