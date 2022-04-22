/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "test_property_holder", menuName = "FF/Test/Property Holder" ) ]
public class Test_PropertyHolder : ScriptableObject
{
	public Durability durability;
	public SharedBoolNotifier isNutOnBolt;
	public Velocity velocity;
	public Currency currency;
}
