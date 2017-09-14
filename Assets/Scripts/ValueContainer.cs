using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueContainer
{
	private List<float> values = new List<float>();
	private List<string> valueLookup = new List<string>();

	public ValueContainer(string[] valueNames)
	{
		int len = valueNames.Length;

		for (int i = 0; i < len; i++)
		{
			set(valueNames[i], 0);
		}
	}

	public void set(string lookup, float val)
	{
		valueLookup.Add(lookup);
		values.Add(val);
	}

	public float? get(string lookup)
	{
		if(!valueLookup.Contains(lookup))
		{
			Debug.LogError("'" + lookup + "' doesn't exist as a value, check to see if your string is valid.");

			return null;
		}

		return values[valueLookup.IndexOf(lookup)];
	}

	public List<float> getAll()
	{
		return values;
	}
}




