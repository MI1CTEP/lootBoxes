using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Video;
using MyGame.Cards;

namespace MyGame
{
    public static class AsyncContent
    {
        public static void LoadCardSprite(Card card, UnityAction<Sprite> onEndLoad)
        {
            int groupId = card.groupId + 1;
            int id = card.id + 1;
            int rank = card.rank + 1;
            if (rank > 3) rank = 3;
            string key = $"card_{groupId}_{id}_{rank}";
            Addressables.LoadAssetAsync<Sprite>(key).Completed += (asyncOperation) =>
            {
                onEndLoad?.Invoke(asyncOperation.Result);
            };
        }

        public static void LoadMiniCardSprite(Card card, UnityAction<Sprite> onEndLoad)
        {
            int groupId = card.groupId + 1;
            int id = card.id + 1;
            int rank = card.rank + 1;
            if (rank > 3) rank = 3;
            string key = $"mini_card_{groupId}_{id}_{rank}";
            Addressables.LoadAssetAsync<Sprite>(key).Completed += (asyncOperation) =>
            {
                onEndLoad?.Invoke(asyncOperation.Result);
            };
        }

        public static void ReleaseSprite(Sprite sprite)
        {
            Addressables.Release(sprite);
        }

        public static void LoadVideo(Card card, UnityAction<VideoClip> onEndLoad)
        {
            int groupId = card.groupId + 1;
            int id = card.id + 1;
            string key = $"video_{groupId}_{id}";
            Addressables.LoadAssetAsync<VideoClip>(key).Completed += (asyncOperation) =>
            {
                onEndLoad?.Invoke(asyncOperation.Result);
            };
        }

        public static void ReleaseVideo(VideoClip videoClip)
        {
            Addressables.Release(videoClip);
        }
    }
}