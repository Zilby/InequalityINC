using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode
{
	public Rect rect;
	public Rect defaultRect;
	public string title;
	public string dialogue = "";
	public DialogueManager.Character character = DialogueManager.Character.player;
	public DialogueManager.Expression expression = DialogueManager.Expression.neutral;
	public bool advancedOptions = false; 
	public bool longOption = false;
	public bool positive = false;
	public bool negative = false;
	public int infoGathered = -1;
	public DialogueManager.Character fired = DialogueManager.Character.none;
	public bool isDragged;
	public bool isSelected;

	public ConnectionPoint inPoint;
	public ConnectionPoint outPoint;

	public GUIStyle style;
	public GUIStyle defaultNodeStyle;
	public GUIStyle selectedNodeStyle;
	public GUIStyle textfieldStyle;
	public GUIStyle toggleStyle;
	public GUIStyle popupStyle;
	public GUIStyle foldoutStyle;

	public Action<DialogueNode> OnRemoveNode;

	public DialogueNode(Vector2 position, float width, float height,
						GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
						Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint,
						Action<DialogueNode> OnClickRemoveNode)
	{
		defaultRect = rect = new Rect(position.x, position.y, width, height);
		style = nodeStyle;
		inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
		outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
		defaultNodeStyle = nodeStyle;
		selectedNodeStyle = selectedStyle;
		OnRemoveNode = OnClickRemoveNode;
		textfieldStyle = EditorStyles.textField;
		textfieldStyle.wordWrap = true;
		textfieldStyle.stretchHeight = true;
		textfieldStyle.stretchWidth = false;
		toggleStyle = EditorStyles.toggle;
		toggleStyle.richText = true;
		popupStyle = EditorStyles.popup;
		foldoutStyle = EditorStyles.foldout;
		foldoutStyle.richText = true;
	}

	public void Drag(Vector2 delta)
	{
		rect.position += delta;
	}

	public void Draw()
	{
		GUIContent content = new GUIContent(dialogue);
		float height = textfieldStyle.CalcHeight(content, rect.width - 20);
		rect.height = defaultRect.height + height;
		if (character == DialogueManager.Character.options && advancedOptions)
		{
			rect.height += 100;
		}
		inPoint.Draw();
		outPoint.Draw();
		GUI.Box(rect, title, style);
		dialogue = EditorGUI.TextField(new Rect(rect.x + 10, rect.y + 70, rect.width - 20, height), dialogue, textfieldStyle);
		character = (DialogueManager.Character)EditorGUI.EnumPopup(
			new Rect(rect.x + 10, rect.y + 25, rect.width - 20, 15), character);
		if (character != DialogueManager.Character.options)
		{
			expression = (DialogueManager.Expression)EditorGUI.EnumPopup(
				new Rect(rect.x + 10, rect.y + 45, rect.width - 20, 15), expression);
		}
		else
		{
			content = new GUIContent("<color=white>Advanced Options</color>");
			advancedOptions = EditorGUI.Foldout(new Rect(rect.x + 10, rect.y + 45, rect.width - 20, 15), advancedOptions, content);
			if (advancedOptions)
			{
				content = new GUIContent("<color=white>Long Option</color>");
				longOption = EditorGUI.Toggle(new Rect(rect.x + 15, rect.y + rect.height - 120, rect.width - 20, 15), content, longOption, toggleStyle);
				content = new GUIContent("<color=white>Positive Interaction</color>");
				positive = EditorGUI.Toggle(new Rect(rect.x + 15, rect.y + rect.height - 100, rect.width - 20, 15), content, positive, toggleStyle);
				content = new GUIContent("<color=white>Negative Interaction</color>");
				negative = EditorGUI.Toggle(new Rect(rect.x + 15, rect.y + rect.height - 80, rect.width - 20, 15), content, negative, toggleStyle);
				content = new GUIContent("<color=white>Info Gathered</color>");
				EditorGUI.LabelField(new Rect(rect.x + 15, rect.y + rect.height - 60, 80, 15), content);
				infoGathered = EditorGUI.IntField(new Rect(rect.x + 150, rect.y + rect.height - 60, rect.width - 170, 15), infoGathered);
				content = new GUIContent("<color=white>Fired</color>");
				EditorGUI.LabelField(new Rect(rect.x + 15, rect.y + rect.height - 40, 30, 15), content);
				fired = (DialogueManager.Character)EditorGUI.EnumPopup(
					new Rect(rect.x + 60, rect.y + rect.height - 40, rect.width - 80, 15), fired, popupStyle);
			}
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
