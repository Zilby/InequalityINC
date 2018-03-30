using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

[XmlInclude(typeof(DialogueNode))]
public class DialogueNode
{
	private static int idCounter = 0;
	public int id = 0;
	public bool readyToInit = false;
	[XmlIgnore]
	public bool initialized = false;
	public Rect rect;
	public Rect defaultRect;
	public string title;
	public string dialogue = "";
	public DialogueManager.Character character = DialogueManager.Character.player;
	public DialogueManager.Expression expression = DialogueManager.Expression.neutral;
	public bool advancedOptions = false;
	public DialogueManager.Character characterAffected = DialogueManager.Character.player;
	public bool forceLeft = false;
	public bool longOption = false;
	public bool positive = false;
	public bool negative = false;
	public int infoGathered = -1;
	public bool fired = false;
	public bool restrictions = false;
	public DialogueManager.Character characterRestriction = DialogueManager.Character.player;
	public int positiveRestriction = -1;
	public int negativeRestriction = -1;
	public int infoRestriction = -1;
	public bool isDragged;
	public bool isSelected;

	public ConnectionPoint inPoint;
	public ConnectionPoint outPoint;

	private GUIStyle style;
	private GUIStyle defaultNodeStyle;
	private GUIStyle selectedNodeStyle;

	private GUIStyle textfieldStyle;
	private GUIStyle toggleStyle;
	private GUIStyle popupStyle;
	private GUIStyle foldoutStyle;

	private Action<DialogueNode> OnRemoveNode;

	public DialogueNode()
	{
	}

	public DialogueNode(Vector2 position, float width, float height)
	{
		defaultRect = rect = new Rect(position.x, position.y, width, height);
		id = ++idCounter;
		inPoint = new ConnectionPoint(this, ConnectionPoint.Type.In);
		outPoint = new ConnectionPoint(this, ConnectionPoint.Type.Out);
		readyToInit = true;
	}

	public void Init()
	{
		style = new GUIStyle();
		style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
		style.border = new RectOffset(12, 12, 12, 12);

		defaultNodeStyle = style;

		selectedNodeStyle = new GUIStyle();
		selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
		selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

		EditorStyles.textField.wordWrap = false;
		EditorStyles.textField.stretchHeight = false;
		textfieldStyle = new GUIStyle(EditorStyles.textField); 
		textfieldStyle.wordWrap = true;
		textfieldStyle.stretchHeight = true;
		textfieldStyle.stretchWidth = false;
		EditorStyles.label.normal.textColor = Color.black;
		EditorStyles.label.richText = true;
		foldoutStyle = new GUIStyle(EditorStyles.foldout);
		foldoutStyle.richText = true;
		OnRemoveNode = DialogueNodeEditor.RemoveNodeEvent;
		idCounter = Mathf.Max(id, idCounter);
		initialized = true;
	}

	public void Drag(Vector2 delta)
	{
		rect.position += delta;
	}

	public void Draw()
	{
		if (readyToInit && !initialized)
		{
			Init();
		}
		else
		{
			GUIContent content = new GUIContent(dialogue);
			float height = textfieldStyle.CalcHeight(content, rect.width - 20);
			rect.height = defaultRect.height + height;
			float advancedHeight = advancedOptions ? 140 : 0;
			float restrictionsHeight = restrictions ? 80 : 0;
			rect.height += advancedHeight;
			rect.height += restrictionsHeight;
			inPoint.Draw();
			outPoint.Draw();
			GUI.Box(rect, title, style);
			dialogue = EditorGUI.TextField(new Rect(rect.x + 10, rect.y + 70, rect.width - 20, height), dialogue, textfieldStyle);
			character = (DialogueManager.Character)EditorGUI.EnumPopup(
				new Rect(rect.x + 10, rect.y + 25, rect.width - 20, 15), character);
			expression = (DialogueManager.Expression)EditorGUI.EnumPopup(
				new Rect(rect.x + 10, rect.y + 45, rect.width - 20, 15), expression);
			content = new GUIContent("<color=white>Advanced Options</color>");
			advancedOptions = EditorGUI.Foldout(new Rect(rect.x + 10, rect.y + 80 + height, rect.width - 20, 15), advancedOptions, content, foldoutStyle);
			content = new GUIContent("<color=white>Restrictions</color>");
			restrictions = EditorGUI.Foldout(new Rect(rect.x + 10, rect.y + 100 + height + advancedHeight, rect.width - 20, 15), restrictions, content, foldoutStyle);
			if (advancedOptions)
			{
				content = new GUIContent("<color=white>NPC Affected</color>");
				EditorGUI.LabelField(new Rect(rect.x + 15, rect.y + 100 + height, 80, 15), content);
				characterAffected = (DialogueManager.Character)EditorGUI.EnumPopup(
					new Rect(rect.x + 110, rect.y + 100 + height, rect.width - 130, 15), characterAffected);
				content = new GUIContent("<color=white>Force Left</color>");
				forceLeft = EditorGUI.Toggle(new Rect(rect.x + 15, rect.y + 120 + height, rect.width - 20, 15), content, forceLeft);
				content = new GUIContent("<color=white>Long Option</color>");
				longOption = EditorGUI.Toggle(new Rect(rect.x + 15, rect.y + 140 + height, rect.width - 20, 15), content, longOption);
				content = new GUIContent("<color=white>Positive Interaction</color>");
				positive = EditorGUI.Toggle(new Rect(rect.x + 15, rect.y + 160 + height, rect.width - 20, 15), content, positive);
				content = new GUIContent("<color=white>Negative Interaction</color>");
				negative = EditorGUI.Toggle(new Rect(rect.x + 15, rect.y + 180 + height, rect.width - 20, 15), content, negative);
				content = new GUIContent("<color=white>Info Gathered</color>");
				EditorGUI.LabelField(new Rect(rect.x + 15, rect.y + 200 + height, 80, 15), content);
				infoGathered = EditorGUI.IntField(new Rect(rect.x + 150, rect.y + 200 + height, rect.width - 170, 15), infoGathered);
				content = new GUIContent("<color=white>Fired</color>");
				fired = EditorGUI.Toggle(new Rect(rect.x + 15, rect.y + 220 + height, rect.width - 20, 15), content, fired);
			}
			inPoint.nodeRect = rect;
			outPoint.nodeRect = rect;
		}
	}

	public bool ProcessEvents(Event e)
	{
		switch (e.type)
		{
			case EventType.MouseDown:
				if (e.button == 0)
				{
					GUI.changed = true;
					isSelected = rect.Contains(e.mousePosition);
					if (isSelected)
					{
						isDragged = true;
						style = selectedNodeStyle;
					}
					else
					{
						style = defaultNodeStyle;
					}
				}
				if (e.button == 1 && rect.Contains(e.mousePosition))
				{
					ProcessContextMenu();
					e.Use();
				}
				break;

			case EventType.MouseUp:
				isDragged = false;
				break;

			case EventType.MouseDrag:
				if (e.button == 0 && isDragged)
				{
					Drag(e.delta);
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
		genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
		genericMenu.ShowAsContext();
	}

	private void OnClickRemoveNode()
	{
		if (OnRemoveNode != null)
		{
			OnRemoveNode(this);
		}
	}
}
