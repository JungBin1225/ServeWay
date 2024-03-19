using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class IngredientDataSet : ScriptableObject
{
	public List<Ingredient> IngredientList; // Replace 'EntityType' to an actual type that is serializable.
}
