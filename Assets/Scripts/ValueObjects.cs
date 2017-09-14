using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueContainer
{
	/*These are the data values. value[0] is the actual value and value[1] is the amount
	by which the first value will be changed every so often where:
	0 = Economy, 1 = Food, 2 = Iron, 3 = Coal, 4 = Happiness, 5 = Population*/
	private int[] values;
	public enum type{};

	public int[] getAll()
	{
		return values;
	}

	public int get(type value)
	{
		return values[(int)value];
	}

	public void set(type value, int newValue)
	{
		values[(int)value] = newValue;
	}
}
//UNFINISHED
public class ProvinceValues : ValueContainer
{
	new enum type{Economy, Food, Iron, Coal, Happiness, Population};
	

	public ProvinceValues()
	{
		values = new int[] {0, 0, 0, 0, 0, 0};
	}
}




