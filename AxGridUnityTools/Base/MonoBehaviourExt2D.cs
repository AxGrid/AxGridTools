using System;
using AxGrid.Path;
using AxGrid.Utils;
using UnityEngine;

namespace AxGrid.Base {
    public abstract class MonoBehaviourExt2D : MonoBehaviourExt {
        
        protected SpriteRenderer SpriteRenderer { get; set; }

        public bool Visible {
            get => SpriteRenderer != null && SpriteRenderer.enabled;
            set {
                if (SpriteRenderer == null) return;
                SpriteRenderer.enabled = value;
            }
        }
        
        [OnAwake]
        protected virtual void __Create2DAdds() {
            SpriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public float AlphaColor {
            get => SpriteRenderer != null ? SpriteRenderer.color.a : 0f;
            set {
                if (SpriteRenderer == null) return;
                SpriteRenderer.color = SpriteRenderer.color.SetA(value);
            }
        }
        
        public float Rotation {
            get => transform.rotation.eulerAngles.z;
            set => transform.rotation = Quaternion.Euler(0, 0, value);
        }

        private void FlipActionHorizontal(float time, DEasingMethodF easingInMethod, DEasingMethodF easingOutMethod, Action completeAction, Action<float> action) {
            if (easingInMethod == null) easingInMethod = EasingTo.Linear;
            if (easingOutMethod == null) easingOutMethod = EasingTo.Linear;
            var startScaleX = this.transform.localScale.x;
            var tail = 0f;
            Path = new CPath().Add(dt => {
                var f = easingInMethod.Invoke(dt.DeltaF, startScaleX, 0.0f, time);
                if (dt.DeltaF > time) f = 0;
                action(f);
                if (!(dt.DeltaF > time)) return Status.Continue;
                tail = dt.DeltaF - time;
                return Status.OK;
            }).Add(dt => {
                var f = easingOutMethod.Invoke(dt.DeltaF, 0.0f, startScaleX, time);
                if (dt.DeltaF > time) f = startScaleX;
                action(f);
                if (!(dt.DeltaF > time)) return Status.Continue;
                tail = dt.DeltaF - time;
                return Status.Now;
            }).Action(() => completeAction?.Invoke());
        }
        
        public void FlipVertical(float time, DEasingMethodF easingInMethod = null, DEasingMethodF easingOutMethod = null, Action completeAction = null) {
            FlipActionHorizontal(time, easingInMethod, easingOutMethod, completeAction,
                f => { transform.localScale = transform.localScale.SetX(f); });
        }
        
        public void FlipHorizontal(float time, DEasingMethodF easingInMethod = null, DEasingMethodF easingOutMethod = null, Action completeAction = null) {
            FlipActionHorizontal(time, easingInMethod, easingOutMethod, completeAction,
                f => { transform.localScale = transform.localScale.SetY(f); });
        }

    }
}