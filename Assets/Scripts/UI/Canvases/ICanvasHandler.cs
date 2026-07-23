namespace Assets.Scripts.UI.Canvases
{
	/// <summary>
	/// Реалізується кожним скриптом екрану (наприклад GameCanvasHandler).
	/// Перемикання відбувається шляхом вмикання/вимикання компонента Canvas
	/// (та GraphicRaycaster) на об'єкті, а НЕ через GameObject.SetActive.
	/// Завдяки цьому сам GameObject екрану залишається активним весь час,
	/// а MonoBehaviour-логіка (Update, корутини, підписки на дані тощо)
	/// не перериваються під час перемикання канвасів.
	/// </summary>
	public interface ICanvasHandler
	{
		/// <summary>Чи видимий (і інтерактивний) зараз цей канвас.</summary>
		bool IsVisible { get; }

		/// <summary>Показати канвас (увімкнути Canvas/Raycaster) і викликати логіку появи.</summary>
		void Show();

		/// <summary>Сховати канвас (вимкнути Canvas/Raycaster) і викликати логіку приховування.</summary>
		void Hide();
	}
}
