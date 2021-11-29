using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Slider
{
	[Serializable]
	public class Banner
	{
		[Tooltip("A prefab implementing the IBanner interface")]
		public GameObject prefab;
		public Button button;
		public CashFlowTypes cashflowType;
		public bool useCashflowType;
	}
}
