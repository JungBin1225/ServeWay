using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(LogOnImport = true)]
public class StartFoodDataSet : ScriptableObject
{
	public List<FoodData> StartFoodDatas; // Replace 'EntityType' to an actual type that is serializable.
}
