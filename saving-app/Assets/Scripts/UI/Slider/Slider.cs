using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Slider
{
	public class Slider : MonoBehaviour
	{
		[Header("Settings")]
		public List<Banner> Banners;
		public bool Elastic = true;

		[Header("UI")]
		public Transform BannerGrid;
		public HorizontalScrollSnap HorizontalScrollSnap;

		public IEnumerator Start()
		{
			foreach (Transform child in BannerGrid)
			{
				Destroy(child.gameObject);
			}

			foreach (var banner in Banners)
			{
				var instance = Instantiate(banner.prefab, BannerGrid);

				instance.TryGetComponent<IBanner>(out IBanner component);

				if (component == null)
					throw new System.Exception("Banner prefab should implement the IBanner interface");

				// setup prefab
				if (banner.useCashflowType)
				{
					component.Setup(banner.cashflowType);
				}
				else
					component.Setup(null);
			}

			yield return null;

			HorizontalScrollSnap.Initialize();
			HorizontalScrollSnap.GetComponent<ScrollRect>().movementType = Elastic ? ScrollRect.MovementType.Elastic : ScrollRect.MovementType.Clamped;
		}
	}
}