using System;
using System.Threading.Tasks;
using Audio;
using General.SingletonClass;
using UnityEngine;

namespace Core
{
    public class CoreGameManager : SingletonClass<CoreGameManager>
    {
        public override void Awake()
        {
            base.Awake();

            Cursor.visible = false;
        }

        private async void Start()
        {
            await Task.Yield();
            Debug.Log((MusicManager.Instance == null));
            MusicManager.Instance.PlayMusic(MusicManager.MusicType.Theme);
        }
    }
}