using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DialogueTree : IWriteable, IReadable
{

	[XmlIgnore]
	public int scene = -1;

	[XmlIgnore]
	public DialogueManager.Character characterFile = DialogueManager.Character.none;

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
		return Path.Combine("Dialogue", characterFile.ToString());
	}

	public string GetFileName()
	{
		return "Dialogue" + scene;
	}
}
