using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueContainer
{
	/* 0 = Economy, 1 = Food, 2 = Iron, 3 = Coal, 4 = Happiness, 5 = Population*/
	private IDictionary<string, float> values = new Dictionary<string, float>();
	
	public void addValue(string name, float value)
	{
		values.Add(name, value);
	}

	public float? getValue(string key)
	{
		if (!values.ContainsKey(key))
		{
			return null;
		}
		
		return values[key];
	}
}

