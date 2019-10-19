using System;
using UnityEngine;

namespace AxGrid.Base
{
    /// <inheritdoc />
    /// <summary>
    /// Абстрактный класс связывания событий
    /// </summary>
    public class MonoBehaviourExtBind : MonoBehaviourExt
    {
        [OnStart(-100)]
        protected void __Bind()
        {
            try
            {
                Settings.Model.EventManager.Add(this);
            }
            catch (NullReferenceException e)
            {
                Log.Error(e, "Bind Exception");
            }
        }

        [OnDestroy(int.MaxValue)]
        protected void __UnBind()
        {
            if (Settings.Model == null)
                return;
            try
            {
                Settings.Model.EventManager.Remove(this);
            }
            catch (NullReferenceException e)
            {
                Log.Error(e, "UnBind Exception");
            }
        }
        
        /// <summary>
        /// Методы контроля аниматоров через модель
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="animatorName"></param>
        public void BindAnimatorBool(string fieldName, string animatorName=null)
        {
            if (string.IsNullOrEmpty(animatorName))
                animatorName = fieldName;
            Model.EventManager.AddAction(fieldName, () => Animator.SetBool(animatorName, Model.GetBool(fieldName)));    
        } 

        /// <summary>
        /// Методы контроля аниматоров через модель
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="animatorName"></param>
        public void BindAnimatorFloat(string fieldName, string animatorName=null)
        {
            if (string.IsNullOrEmpty(animatorName))
                animatorName = fieldName;
            Model.EventManager.AddAction(fieldName, () => Animator.SetFloat(animatorName, Model.GetFloat(fieldName)));    
        } 
        /// <summary>
        /// Методы контроля аниматоров через модель
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="animatorName"></param>
        public void BindAnimatorInteger(string fieldName, string animatorName=null)
        {
            if (string.IsNullOrEmpty(animatorName))
                animatorName = fieldName;
            Model.EventManager.AddAction(fieldName, () => Animator.SetInteger(animatorName, Model.GetInt(fieldName)));    
        } 

        /// <summary>
        /// Методы контроля аниматоров через модель
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="animatorName"></param>
        public void BindAnimatorTrigger(string fieldName, string animatorName=null)
        {
            if (string.IsNullOrEmpty(animatorName))
                animatorName = fieldName;
            Model.EventManager.AddAction(fieldName, () => Animator.SetTrigger(animatorName));
            Model.EventManager.AddAction($"{fieldName}Reset", () => Animator.ResetTrigger(animatorName));
        } 

    }
}