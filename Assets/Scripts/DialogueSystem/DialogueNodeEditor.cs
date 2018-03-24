using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;
using System;
using System.IO;

public class DialogueNodeEditor : EditorWindow
{
	public DialogueTree tree;

	public static Action<ConnectionPoint> ClickInPointEvent;

	public static Action<ConnectionPoint> ClickOutPointEvent;

	public static Action<DialogueNode> RemoveNodeEvent;

	public static Action<Connection> RemoveConnectionEvent;

	private ConnectionPoint selectedInPoint;
	private ConnectionPoint selectedOutPoint;

	private Vector2 offset;
	private Vector2 drag;

	private bool needsConnectionFuse = false;

	[MenuItem("Window/Dialogue Editor")]
	private static void OpenWindow()
	{
		DialogueNodeEditor window = GetWindow<DialogueNodeEditor>();
		window.titleContent = new GUIContent("Dialogue Editor");
	}

	private void OnEnable()
	{
		tree = new DialogueTree();
		ClickInPointEvent = OnClickInPoint;
		ClickOutPointEvent = OnClickOutPoint;
		RemoveNodeEvent = OnClickRemoveNode;
		RemoveConnectionEvent = OnClickRemoveConnection;
	}

	private void OnGUI()
	{
		DrawGrid(20, 0.2f, Color.gray);
		DrawGrid(100, 0.4f, Color.gray);

		DrawNodes();
		DrawConnections();

		DrawConnectionLine(Event.current);

		DrawControls();

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

		offset += drag * 0.5f;
		Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

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


	private void DrawControls() {
		GUIContent content = new GUIContent("Character");
		EditorGUI.LabelField(new Rect(5, 5, 60, 15), content);
		tree.characterFile = (DialogueManager.Character)EditorGUI.EnumPopup(
			new Rect(70, 5, 90, 15), tree.characterFile, EditorStyles.popup);
		content = new GUIContent("Type");
		EditorGUI.LabelField(new Rect(5, 25, 60, 15), content);
		tree.type = (DialogueTree.Type)EditorGUI.EnumPopup(
			new Rect(70, 25, 90, 15), tree.type, EditorStyles.popup);
		content = new GUIContent("Dialogue Scene");
		EditorGUI.LabelField(new Rect(5, 45, 100, 20), content);
		tree.scene = EditorGUI.IntField(new Rect(100, 45, 60, 15), tree.scene);
		content = new GUIContent("Load Dialogue");
		if(GUI.Button(new Rect(5, 65, 150, 20), content)) {
			DialogueTree d = DialogueWriter.LoadTree(Path.Combine(tree.GetDirectory(), tree.GetFileName()));
			if (d != null)
			{
				tree.Nodes = d.Nodes;
				tree.Connections = d.Connections;
				needsConnectionFuse = true;
			}
		}
		content = new GUIContent("Save Dialogue");
		if (GUI.Button(new Rect(5, 90, 150, 20), content))
		{
			DialogueWriter.WriteTree(tree, tree.GetFileName(), tree.GetDirectory());
		}
		if (needsConnectionFuse) 
		{
			FuseConnections();
		}
	}


	private void FuseConnections() {
		foreach (DialogueNode n in tree.Nodes)
		{
			if (!n.initialized || !n.inPoint.initialized || !n.outPoint.initialized)
			{
				return;
			}
		}
		foreach (Connection c in tree.Connections)
		{
			if (!c.initialized || !c.inPoint.initialized || !c.outPoint.initialized)
			{
				return;
			}
		}
		foreach (DialogueNode n in tree.Nodes)
		{
			foreach (Connection c in tree.Connections)
			{
				if (n.inPoint.id == c.inPoint.id)
				{
					c.inPoint = n.inPoint;
				}
			}
			foreach (Connection c in tree.Connections)
			{
				if (n.outPoint.id == c.outPoint.id)
				{
					c.outPoint = n.outPoint;
				}
			}
		}
		needsConnectionFuse = false;
	}

	private void DrawNodes()
	{
		if (tree.Nodes != null)
		{
			for (int i = 0; i < tree.Nodes.Count; i++)
			{
				tree.Nodes[i].Draw();
			}
		}
	}

	private void DrawConnections()
	{
		if (tree.Connections != null)
		{
			for (int i = 0; i < tree.Connections.Count; i++)
			{
				tree.Connections[i].Draw();
			}
		}
	}

	private void DrawConnectionLine(Event e)
	{
		if (selectedInPoint != null && selectedOutPoint == null)
		{
			Handles.DrawBezier(
				selectedInPoint.rect.center,
				e.mousePosition,
				selectedInPoint.rect.center + Vector2.down * 50f,
				e.mousePosition - Vector2.down * 50f,
				Color.white,
				null,
				2f
			);

			GUI.changed = true;
		}

		if (selectedOutPoint != null && selectedInPoint == null)
		{
			Handles.DrawBezier(
				selectedOutPoint.rect.center,
				e.mousePosition,
				selectedOutPoint.rect.center - Vector2.down * 50f,
				e.mousePosition + Vector2.down * 50f,
				Color.white,
				null,
				2f
			);

			GUI.changed = true;
		}
	}

	private void ProcessEvents(Event e)
	{
		drag = Vector2.zero;
		switch (e.type)
		{
			case EventType.MouseDown:
				if (e.button == 1)
				{
					ProcessContextMenu(e.mousePosition);
				}
				break;

			case EventType.MouseDrag:
				if (e.button == 0)
				{
					OnDrag(e.delta);
				}
				break;
		}
	}

	private void ProcessNodeEvents(Event e)
	{
		if (tree.Nodes != null)
		{
			for (int i = tree.Nodes.Count - 1; i >= 0; i--)
			{
				bool guiChanged = tree.Nodes[i].ProcessEvents(e);

				if (guiChanged)
				{
					GUI.changed = true;
				}
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		drag = delta;

		if (tree.Nodes != null)
		{
			for (int i = 0; i < tree.Nodes.Count; i++)
			{
				tree.Nodes[i].Drag(delta);
			}
		}

		GUI.changed = true;
	}

	private void ProcessContextMenu(Vector2 mousePosition)
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
		genericMenu.ShowAsContext();
	}

	private void OnClickAddNode(Vector2 mousePosition)
	{
		if (tree.Nodes == null)
		{
			tree.Nodes = new List<DialogueNode>();
		}

		tree.Nodes.Add(new DialogueNode(mousePosition, 200, 120));
	}

	private void OnClickInPoint(ConnectionPoint inPoint)
	{
		selectedInPoint = inPoint;

		if (selectedOutPoint != null)
		{
			if (selectedOutPoint.nodeRect != selectedInPoint.nodeRect)
			{
				CreateConnection();

			}
			ClearConnectionSelection();
		}
	}

	private void OnClickOutPoint(ConnectionPoint outPoint)
	{
		selectedOutPoint = outPoint;

		if (selectedInPoint != null)
		{
			if (selectedOutPoint.nodeRect != selectedInPoint.nodeRect)
			{
				CreateConnection();
			}
			ClearConnectionSelection();
		}
	}

	private void OnClickRemoveConnection(Connection connection)
	{
		tree.Connections.Remove(connection);
	}

	private void CreateConnection()
	{
		if (tree.Connections == null)
		{
			tree.Connections = new List<Connection>();
		}

		tree.Connections.Add(new Connection(selectedInPoint, selectedOutPoint));
	}

	private void ClearConnectionSelection()
	{
		selectedInPoint = null;
		selectedOutPoint = null;
	}

	private void OnClickRemoveNode(DialogueNode node)
	{
		if (tree.Connections != null)
		{
			List<Connection> connectionsToRemove = new List<Connection>();

			for (int i = 0; i < tree.Connections.Count; i++)
			{
				if (tree.Connections[i].inPoint == node.inPoint || tree.Connections[i].outPoint == node.outPoint)
				{
					connectionsToRemove.Add(tree.Connections[i]);
				}
			}

			for (int i = 0; i < connectionsToRemove.Count; i++)
			{
				tree.Connections.Remove(connectionsToRemove[i]);
			}

			connectionsToRemove = null;
		}

		tree.Nodes.Remove(node);
	}
}
