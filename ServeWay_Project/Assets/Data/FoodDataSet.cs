using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(LogOnImport =true)]
public class FoodDataSet : ScriptableObject
{
	public List<FoodData> FoodDatas; // Replace 'EntityType' to an actual type that is serializable.
}
