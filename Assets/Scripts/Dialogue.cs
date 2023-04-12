using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
	public Sprite characterSprite;

	public string characterName;

	[TextArea(3, 10)]
	public string[] sentences;
}
