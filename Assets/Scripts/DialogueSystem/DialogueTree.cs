using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DialogueTree : IWriteable, IReadable
{

	public enum Type 
	{
		Dialogue, 
		Snippet
	}

	[XmlIgnore]
	public Type type = Type.Dialogue;

	[XmlIgnore]
	public int scene = -1;

	[XmlIgnore]
	public DialogueManager.Character characterFile = DialogueManager.Character.player;

	[XmlArray]
	public List<DialogueNode> Nodes
	{
		get { return nodes; }
		set { nodes = value; }
	}

	private List<DialogueNode> nodes;

	[XmlArray]
	public List<Connection> Connections
	{
		get { return connections; }
		set { connections = value; }
	}

	private List<Connection> connections;

	public string GetDirectory()
	{
		return Path.Combine(type.ToString(), characterFile.ToString());
	}

	public string GetFileName()
	{
		return type.ToString() + scene;
	}
}
