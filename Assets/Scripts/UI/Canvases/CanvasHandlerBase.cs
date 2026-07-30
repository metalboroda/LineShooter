using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Canvases
{
	[RequireComponent(typeof(Canvas))]
	public abstract class CanvasHandlerBase : MonoBehaviour, ICanvasHandler
	{
		private Canvas _canvas;
		private GraphicRaycaster _raycaster;

		private Canvas CanvasComponent => _canvas ? _canvas : _canvas = GetComponent<Canvas>();
		private GraphicRaycaster RaycasterComponent => _raycaster ? _raycaster : _raycaster = GetComponent<GraphicRaycaster>();

		public bool IsVisible => CanvasComponent.enabled;

		public void Show()
		{
			if (IsVisible) return;
			
			OnShown();
			SetCanvasEnabled(true);
		}

		public void Hide()
		{
			if (!IsVisible) return;

			SetCanvasEnabled(false);
			OnHidden();
		}

		private void SetCanvasEnabled(bool isEnabled)
		{
			CanvasComponent.enabled = isEnabled;

			if (RaycasterComponent)
				RaycasterComponent.enabled = isEnabled;
		}

		protected virtual void OnShown() { }

		protected virtual void OnHidden() { }
	}
}