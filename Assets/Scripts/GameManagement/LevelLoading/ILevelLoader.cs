using System.Threading;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.GameManagement.LevelLoading
{
	public interface ILevelLoader
	{
		public int LevelCount { get; }
		public bool HasLoadedLevel { get; }

		public UniTask LoadLevelAsync(int index, CancellationToken token);
		public void UnloadCurrentLevel();
	}
}