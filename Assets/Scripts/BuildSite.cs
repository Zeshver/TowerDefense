using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TowerDefense
{
    public class BuildSite : MonoBehaviour, IPointerDownHandler
    {
        public TowerAsset[] buildableTowers;
        public void SetBuildableTowers(TowerAsset[] towers) 
        {
            if (towers == null || towers.Length == 0)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                buildableTowers = towers;
            }
        }

        public static event Action<BuildSite> OnClickEvent;
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnClickEvent(this);
        }

        public static void HideControls()
        {
            OnClickEvent(null);
        }
    }
}