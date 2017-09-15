using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueContainer
{
	public List<float> values = new List<float>();
	public List<string> names = new List<string>();

	public ValueContainer(string[] initialNames, float[] initialValues)
	{
		int len = initialNames.Length;
		for (int i = 0; i < len; i++)
		{
			names.Add(initialNames[i]);
			values.Add(initialValues[i]);
		}
	}

	public float get(string lookup)
	{
		if (!names.Contains(lookup))
		{
			Debug.LogError("'" + lookup + "' does not match the name of a value on this object.");
			return 0;
		}

		return values[names.IndexOf(lookup)];
	}

	public void addValue(string name, float value)
	{
		if (names.Contains(name))
		{
			Debug.Log("This object already contains a value named '" + name + "'");
			return;
		}

		names.Add(name);
		values.Add(value);
	}
}




