using UnityEngine;

namespace Client
{
    public abstract class BaseWindow : MonoBehaviour
    {
        public void Open()
        {
            gameObject.SetActive(true);
            OnOpen();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            OnClose();
        }
        
        protected abstract void OnOpen();
        protected abstract void OnClose();
    }
}