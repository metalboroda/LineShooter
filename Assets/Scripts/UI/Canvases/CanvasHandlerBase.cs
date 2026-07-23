using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Canvases
{
	/// <summary>
	/// Базовий клас для всіх скриптів екранів (GameCanvasHandler, ShopCanvasHandler і т.д.).
	/// Інкапсулює спільну реалізацію ICanvasHandler: перемикає видимість екрану через
	/// Canvas.enabled (+ GraphicRaycaster.enabled), а не через GameObject.SetActive.
	///
	/// Нащадки замість OnEnable/OnDisable мають перевизначати OnShown()/OnHidden() —
	/// вони викликаються лише під час РЕАЛЬНОЇ зміни видимості (як і колишні OnEnable/OnDisable),
	/// але не залежать від активності GameObject.
	/// </summary>
	[RequireComponent(typeof(Canvas))]
	public abstract class CanvasHandlerBase : MonoBehaviour, ICanvasHandler
	{
		private Canvas _canvas;
		private GraphicRaycaster _raycaster;

		private Canvas CanvasComponent => _canvas != null ? _canvas : _canvas = GetComponent<Canvas>();
		private GraphicRaycaster RaycasterComponent => _raycaster != null ? _raycaster : _raycaster = GetComponent<GraphicRaycaster>();

		public bool IsVisible => CanvasComponent.enabled;

		public void Show()
		{
			if (IsVisible) return;

			SetCanvasEnabled(true);

			OnShown();
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

			if (RaycasterComponent != null)
				RaycasterComponent.enabled = isEnabled;
		}

		/// <summary>Виконується одразу після показу канвасу. Заміна колишнього OnEnable.</summary>
		protected virtual void OnShown() { }

		/// <summary>Виконується одразу після приховування канвасу. Заміна колишнього OnDisable.</summary>
		protected virtual void OnHidden() { }
	}
}
