using System;
using UnityEngine;

public enum ConnectionPointType
{
	In,
	Out
}

public class ConnectionPoint
{
	public Rect rect;

	public ConnectionPointType type;

	public DialogueNode node;

	public GUIStyle style;

	public Action<ConnectionPoint> OnClickConnectionPoint;

	public ConnectionPoint(DialogueNode node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
	{
		this.node = node;
		this.type = type;
		this.style = style;
		this.OnClickConnectionPoint = OnClickConnectionPoint;
		rect = new Rect(0, 0, 20f, 10f);
	}

	public void Draw()
	{
		rect.x = node.rect.x + (node.rect.width * 0.5f) - rect.width * 0.5f;

		switch (type)
		{
			case ConnectionPointType.In:
				rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f - 22;
				break;

			case ConnectionPointType.Out:
				rect.y = node.rect.y + (node.rect.height * 0.5f) - (rect.height * 0.5f) + 20;
				break;
		}

		if (GUI.Button(rect, "", style))
		{
			if (OnClickConnectionPoint != null)
			{
				OnClickConnectionPoint(this);
			}
		}
	}
}