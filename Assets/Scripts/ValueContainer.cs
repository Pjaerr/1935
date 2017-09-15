using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Two lists exist, one containing a string acting as a key, and one being the float values corresponding to those keys.
This mimics the functionality of a dictionary, albeit not as efficient, but this does allow for efficient iteration of the values
if and when it is needed.*/
public class ValueContainer
{
	private List<float> values = new List<float>();
	private List<string> valueLookup = new List<string>();

	/*Initial keys with corresponding values of 0 can be added when the object is initialized so to avoid
	a bunch of .set() calls just to create the values*/
	public ValueContainer(string[] valueNames)
	{
		int len = valueNames.Length;

		for (int i = 0; i < len; i++)
		{
			set(valueNames[i], 0);
		}
	}

	public List<string> getValueLookup()
	{
		return valueLookup;
	}

	public void set(string lookup, float val)
	{
		
		if (valueLookup.Contains(lookup))	//If the lookup string already exists, set its corresponding value to the value passed into the function.
		{
			values[valueLookup.IndexOf(lookup)] = val;
		}
		else	//If not, add the string as a key and the value as a value in the same position.
		{
			valueLookup.Add(lookup);
			values.Add(val);
		}
	}

	public void setAll(float[] newValues)
	{
		int len = values.Count;
		for (int i = 0; i < len; i++)
		{
			values[i] = newValues[i];
		}
	}

	/*Returns the value matching the lookup string passed in, if string doesn't exist, throws an error and returns 0*/
	public float get(string lookup)
	{
		if(!valueLookup.Contains(lookup))
		{
			Debug.LogError("'" + lookup + "' doesn't exist as a value, check to see if your lookup string is valid.");

			return 0;
		}

		return values[valueLookup.IndexOf(lookup)];
	}

	public List<float> getAll()
	{
		return values;
	}
}




