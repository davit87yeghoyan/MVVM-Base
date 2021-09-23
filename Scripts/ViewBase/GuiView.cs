using System;
using MVVMBase.DataBinding;
using UnityEngine;

namespace MVVMBase.ViewBase
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class GuiView<T>:MonoBehaviour,IView<T> where T:ViewModelBase
    {
        private bool _isInitialized;
        public bool destroyOnHide;
        protected readonly PropertyBinder<T> Binder=new PropertyBinder<T>();
        public readonly BindableProperty<T> ViewModelProperty = new BindableProperty<T>();
      
        public Action RevealedAction { get; set; }
       
        public Action HiddenAction { get; set; }

        public T BindingContext
        {
            get => ViewModelProperty.Value;
            set
            {
                if (!_isInitialized)
                {
                    OnInitialize();
                    _isInitialized = true;
                }
                //触发OnValueChanged事件
                ViewModelProperty.Value = value;
            }
        }

        public void Reveal(bool immediate = false, Action action = null)
        {
            if (action!=null)
            {
                RevealedAction += action;
            }
            OnAppear();
            OnReveal(immediate);
            OnRevealed();
        }

        public void Hide(bool immediate = false, Action action = null)
        {
            if (action!=null)
            {
                HiddenAction += action;
            }
            OnHide(immediate);
            OnHidden();
            OnDisappear();
        }

     
        protected virtual void OnInitialize()
        {
            ViewModelProperty.OnValueChanged += OnBindingContextChanged;
        }

       
        public virtual void OnAppear()
        {
            gameObject.SetActive(true);
            BindingContext.OnStartReveal();
        }
       
        
        private void OnReveal(bool immediate)
        {
            if (immediate)
            {
                transform.localScale = Vector3.one;
                GetComponent<CanvasGroup>().alpha = 1;
            }
          
        }
      
        public virtual void OnRevealed()
        {
            BindingContext.OnFinishReveal();
            //回掉函数
            RevealedAction?.Invoke();
        }
      
        private void OnHide(bool immediate)
        {
            BindingContext.OnStartHide();
            if (!immediate) return;
            //立即隐藏
            transform.localScale = Vector3.zero;
            GetComponent<CanvasGroup>().alpha = 0;

        }
        /// <summary>
        /// alpha 1->0
        /// </summary>
        public virtual void OnHidden()
        {
            HiddenAction?.Invoke();
        }
        /// <summary>
        /// Enable->Disable
        /// </summary>
        public virtual void OnDisappear()
        {
            gameObject.SetActive(false);
            BindingContext.OnFinishHide();
            if (destroyOnHide)
            {
                Destroy(this.gameObject);
            }

        }

        public virtual void OnDestroy()
        {
            ViewModelProperty.OnValueChanged = null;
            if (BindingContext == null) return;
            if (BindingContext.IsRevealed) Hide(true);
            Binder.Unbind(BindingContext);
            BindingContext.OnDestory();
            BindingContext = null;
        }

     
      
        public virtual void OnBindingContextChanged(T oldValue, T newValue)
        {
            Binder.Unbind(oldValue);
            Binder.Bind(newValue);
        }

        public virtual void Unbind(T newValue)
        {
            Binder.Unbind(newValue);
        }
    }
}
