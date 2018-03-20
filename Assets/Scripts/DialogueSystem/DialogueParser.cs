﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Parses dialogue from data files. 
/// </summary>
public class DialogueParser
{
	/// <summary>
	/// A single line of dialogue. 
	/// </summary>
	public struct DialogueLine
	{
		/// <summary>
		/// The character saying the dialogue. 
		/// </summary>
		public DialogueManager.Character character;

		/// <summary>
		/// The character receiving the dialogue. 
		/// </summary>
		public DialogueManager.Character receiving;

		/// <summary>
		/// The dialogue being spoken. 
		/// </summary>
		public string content;

		/// <summary>
		/// The expression of the character. 
		/// </summary>
		public DialogueManager.Expression expression;

		/// <summary>
		/// The expression of the character receiving the dialogue. 
		/// </summary>
		public DialogueManager.Expression rExpression;

		/// <summary>
		/// The position of the character (left or right). 
		/// </summary>
		public string position;

		/// <summary>
		/// The options for the player to choose from (if any). 
		/// </summary>
		public string[] options;

		public DialogueLine(DialogueManager.Character ch, string ct, DialogueManager.Expression e, string p,
							DialogueManager.Character cr = DialogueManager.Character.end,
							DialogueManager.Expression re = DialogueManager.Expression.neutral)
		{
			character = ch;
			content = ct;
			expression = e;
			position = p;
			options = new string[1];
			receiving = cr;
			rExpression = re;
		}
	}

	/// <summary>
	/// The lines of dialogue for this scene. 
	/// </summary>
	private List<DialogueLine> lines;

	/// <summary>
	/// The lines of dialogue for this scene. 
	/// </summary>
	public List<DialogueLine> Lines
	{
		get { return lines; }
	}

	/// <summary>
	/// Loads a dialogue scene file into dialogue line structs. 
	/// </summary>
	/// <param name="scene">The scene file to be loaded</param>
	public void LoadDialogue(string scene)
	{
		lines = new List<DialogueLine>();
		TextAsset textFile = (TextAsset)Resources.Load(scene);
		string line;

		using (StringReader r = new StringReader(textFile.text))
		{
			line = r.ReadLine();
			while (line != null)
			{
				{
					string[] lineData = line.Split(';');
					DialogueLine lineEntry;
					if (lineData[0] == "options")
					{
						lineEntry = new DialogueLine(
							(DialogueManager.Character)Enum.Parse(typeof(DialogueManager.Character), lineData[0]), "", 0, "");
						lineEntry.options = new string[lineData.Length - 1];
						for (int i = 1; i < lineData.Length; i++)
						{
							lineEntry.options[i - 1] = lineData[i];
						}
					}
					else if (lineData[0] == "end")
					{
						lineEntry = new DialogueLine(
							(DialogueManager.Character)Enum.Parse(typeof(DialogueManager.Character), lineData[0]), "", 0, "");
						if (lineData.Length > 1)
						{
							lineEntry.content = lineData[1];
						}
					}
					else
					{
						lineEntry = new DialogueLine(
							(DialogueManager.Character)Enum.Parse(typeof(DialogueManager.Character), lineData[0]), lineData[1],
							(DialogueManager.Expression)Enum.Parse(typeof(DialogueManager.Expression), lineData[2]), lineData[3]);
						if (lineData.Length > 5)
						{
							lineEntry.receiving = (DialogueManager.Character)Enum.Parse(typeof(DialogueManager.Character), lineData[4]);
							lineEntry.rExpression = (DialogueManager.Expression)Enum.Parse(typeof(DialogueManager.Expression), lineData[5]);
							if (lineData.Length > 6)
							{
								lineEntry.options[0] = lineData[6];
							}
						}
						else if (lineData.Length > 4)
						{
							lineEntry.options[0] = lineData[4];
						}
					}
					lines.Add(lineEntry);
					line = r.ReadLine();
				}
			}
			r.Close();
		}
	}
}