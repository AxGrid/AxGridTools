using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AxGrid.Model;
using AxGrid.Path;
using AxGrid.Utils;
using AxGrid.Utils.Reflections;
using UnityEngine;
using UnityEngine.Scripting;

namespace AxGrid.Base
{
	public class MonoBehaviourExt : MonoBehaviour
	{
		private List<BaseAttributeInstance> awakeList;
		private List<BaseAttributeInstance> destroyList;
		private List<BaseAttributeInstance> startList;
		private List<BaseAttributeInstance> updateList;
		
		private List<RefreshAttributeInstance> refreshList;
		private List<DelayAttributeInstance> delayList;
		
		private List<BaseAttributeInstance> enableList;
		private List<BaseAttributeInstance> disableList;

		
		/// <summary>
		/// Proxy
		/// </summary>
		protected virtual DynamicModel Model => Settings.Model; 
		
		private class BaseAttributeInstance :  IComparable<BaseAttributeInstance>
		{
			public MethodInfo Method { get; set; }
			public object Target { get; set; }
			public int Priority { get; set; }

			public string GetTargetName()
			{
				return Target == null ? "null" : $"{Target.GetType().FullName}";
			}
			
			public virtual void Invoke()
			{
				try
				{
					Method.Invoke(Target, new object[0]);
				}
				catch (Exception e)
				{
					Exception th = e;
					while (th.InnerException != null)
						th = th.InnerException;
					Debug.LogError($"{th.Message}\nin {Method.Name} target {GetTargetName()}\n{th.StackTrace}");
				}
			}

			public virtual int CompareTo(BaseAttributeInstance other)
			{
				return Priority - other.Priority;
			}
		}

		private class DelayAttributeInstance : BaseAttributeInstance, IComparable<DelayAttributeInstance>
		{
			public float Interval { get; set; }
			public float CurrentInterval { get; set; }
			public bool IsUpdated { get; set; }
			
			public bool Update(float time)
			{
				if (IsUpdated)
					return true;
				CurrentInterval += time;
				if (!(CurrentInterval >= Interval)) return false;
				Invoke();
				IsUpdated = true;
				return true;
			}

			public bool Update()
			{
				return Update(Time.deltaTime);
			}

			public int CompareTo(DelayAttributeInstance other)
			{
				return Priority - other.Priority;
			}
			
			public void Reset()
			{
				CurrentInterval = 0;
			}
		}

		private class RefreshAttributeInstance : BaseAttributeInstance, IComparable<RefreshAttributeInstance>
		{
			public float CurrentInterval { get; set; }
			public float Interval { get; set; }

			public void Update(float time)
			{
				CurrentInterval += time;
				if (!(CurrentInterval >= Interval)) return;
				CurrentInterval -= Interval;
				Invoke();
			}
			
			public void Update()
			{
				Update(Time.deltaTime);
			}
			
			public int CompareTo(RefreshAttributeInstance other)
			{
				return Priority - other.Priority;
			}
			
			public void Reset()
			{
				CurrentInterval = 0;
			}
		}
		
		private MethodInfo disposeMethodInfo;
		private Dictionary<MethodInfo, OnWhen> whenList;

		private CPath path = CPath.Create();
		private List<CPath> pathCollection = new List<CPath>();

		/// <summary>
		/// Основной путь
		/// </summary>
		public CPath Path
		{
			get => path;
			set => path = value;
		}


		/// <summary>
		/// Создать путь в коллекции путей
		/// </summary>
		/// <returns>Путь</returns>
		public CPath CreateNewPath() {
			var newPath = CPath.Create();
			pathCollection.Add(newPath);
			return newPath;
		}

		/// <summary>
		/// Уничтожить путь в коллекции путей
		/// </summary>
		/// <param name="oldPath">Путь</param>
		public void DestroyPath(CPath oldPath) {
			pathCollection.Remove(oldPath);
			oldPath.StopPath();
		}

		
		public bool IsPathAnimated => path != null && path.IsPlaying;

		private Animator _animator;
		public Animator Animator
		{
			get
			{
				if (_animator == null)
					_animator = GetComponent<Animator>();
				return _animator;
			}
			set { _animator = value; } 			
		}
		
		private AudioSource _audioSource;
		public AudioSource AudioSource
		{
			get
			{
				if (_audioSource == null)
					_audioSource = GetComponent<AudioSource>();
				return _audioSource;
			}
			set { _audioSource = value; } 
			
		}


		public void Awake()
		{
			FindAttrebutes();
			foreach (var a in awakeList)
				a.Invoke();

			awakeList.Clear();
			awakeList = null;
			
		}


		public void Start()
		{
			foreach (var a in startList)
				a.Invoke();
			
			startList.Clear();
			startList = null;
		}

		public void OnEnable()
		{
			foreach (var a in enableList)
					a.Invoke();
		}

		public void OnDisable()
		{
			foreach (var a in disableList)
					a.Invoke();
		}

		// Update is called once per frame
		public void Update()
		{
			var dt = Time.deltaTime;
			if (delayList != null && delayList.Count > 0)
			{
				var updated = false;
				foreach (var a in delayList)
					if (a.Update(dt))
						updated = true;

				if (updated)
					delayList.RemoveAll((delay) => { return delay.IsUpdated; });
				if (delayList.Count == 0)
					delayList = null;
			}
			foreach (var a in refreshList)
				a.Update(dt);
			

			foreach (var a in updateList)
				a.Invoke();

			if (IsPathAnimated)
				path.Update(dt);
			pathCollection.Where(item => item.IsPlaying).ForEach(item => item.Update(dt));
			if (pathCollection.Count > 0) pathCollection.RemoveAll(item => !item.IsPlaying);
		}

		public void OnDestroy()
		{
			if (destroyList != null)
				foreach (var a in destroyList)
					a.Invoke();

			if (disposeMethodInfo != null)
				disposeMethodInfo.Invoke(this, new object[0]);
			disposeMethodInfo = null;
			delayList?.Clear();
			refreshList?.Clear();
			updateList.Clear();
			destroyList?.Clear();
			whenList?.Clear();
			Model?.EventManager.Remove(this);
			Path?.Clear();
			Path = null;
			pathCollection.Clear();
		}

		public void ResetRefreshTimer()
		{
			refreshList.ForEach(r => r.Reset());
		}


		
		protected void FindAttrebutes()
		{
			delayList = new List<DelayAttributeInstance>();
			refreshList = new List<RefreshAttributeInstance>();
			awakeList = new List<BaseAttributeInstance>();
			startList = new List<BaseAttributeInstance>();
			updateList = new List<BaseAttributeInstance>();
			destroyList = new List<BaseAttributeInstance>();
			enableList = new List<BaseAttributeInstance>();
			disableList = new List<BaseAttributeInstance>();
			
			whenList = new Dictionary<MethodInfo, OnWhen>();

			if (GetType().GetInterfaces().Contains(typeof(IDisposable)))
				disposeMethodInfo = GetType().GetMethod("Dispose");

			
			//foreach (MethodInfo _mi in GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			foreach (MethodInfo _mi in ReflectionUtils.GetAllMethodsInfo(GetType()))
			{
				var mi = _mi;
				foreach (object _o in mi.GetCustomAttributes(false))
				{
					var o = _o;
					if (o is OnRefresh)
					{
						if (mi.GetGenericArguments().Length > 0)
							throw new ArgumentException("Need method without arguments public void awake()");
						var r = (OnRefresh) o;
						refreshList.Add(new RefreshAttributeInstance
						{
							Method = mi,
							Target = this,
							Priority = r.Priority,
							Interval = r.Interval,
							CurrentInterval = r.currentInterval
						});
					}
					if (o is OnAwake)
					{
						if (mi.GetGenericArguments().Length > 0)
							throw new ArgumentException("Need method without arguments public void awake()");
						var a = (OnAwake) o;
						awakeList.Add(new BaseAttributeInstance{Method = mi,Target = this,Priority = a.Priority});
					}

					if (o is OnStart)
					{
						if (mi.GetGenericArguments().Length > 0)
							throw new ArgumentException("Need method without arguments public void start()");
						var a = (OnStart) o;
						startList.Add(new BaseAttributeInstance{Method = mi,Target = this,Priority = a.Priority});
						
					}

					if (o is OnDestroy)
					{
						if (mi.GetGenericArguments().Length > 0)
							throw new ArgumentException("Need method without arguments public void destroy()");

						var a = (OnDestroy) o;
						destroyList.Add(new BaseAttributeInstance{Method = mi,Target = this,Priority = a.Priority});
					}

					if (o is OnUpdate)
					{
						if (mi.GetGenericArguments().Length > 0)
							throw new ArgumentException("Need method without arguments public void update()");

						var a = (OnUpdate) o;
						updateList.Add(new BaseAttributeInstance{Method = mi,Target = this,Priority = a.Priority});
					}
					
					if (o is OnEnable)
					{
						if (mi.GetGenericArguments().Length > 0)
							throw new ArgumentException("Need method without arguments public void enable()");

						var a = (OnEnable) o;
						enableList.Add(new BaseAttributeInstance{Method = mi,Target = this,Priority = a.Priority});
					}
					
					
					if (o is OnDisable)
					{
						if (mi.GetGenericArguments().Length > 0)
							throw new ArgumentException("Need method without arguments public void disable()");

						var a = (OnDisable) o;
						disableList.Add(new BaseAttributeInstance{Method = mi,Target = this,Priority = a.Priority});
					}


					if (o is OnDelay)
					{
						if (mi.GetGenericArguments().Length > 0)
							throw new ArgumentException("Need method without arguments public void delay()");

						var a = (OnDelay) o;
						delayList.Add(new DelayAttributeInstance
						{
							Method = mi,
							Target = this,
							Priority = a.Priority,
							Interval = a.interval,
							CurrentInterval = a.currentInterval
						});
					}

				}
			}


			awakeList.Sort();
			startList.Sort(); // = startList.OrderBy(item => item.Priority).ToList();
			refreshList.Sort();
			updateList.Sort();


		}
	}

	#region Attributes

	

	public abstract class MonoBehaviourExtAttribute : PreserveAttribute, IComparable<MonoBehaviourExtAttribute>
	{
		public MethodInfo Method { get; set; }
		public object Target { get; set; }
		public int Priority { get; protected set; }
		public Dictionary<MethodInfo,OnWhen> WhenList { get; set; }



		public virtual void Invoke()
		{
			try
			{
				Method.Invoke(Target, new object[0]);
			}
			catch (Exception e)
			{
				Exception th = e;
				while (th.InnerException != null)
					th = th.InnerException; 
				Log.Error(th);
			}
		}



		public virtual int CompareTo(MonoBehaviourExtAttribute other)
		{
			return Priority - other.Priority;
		}

	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnRefresh : MonoBehaviourExtAttribute, IComparable<OnRefresh>
	{

		public float currentInterval = 0.0f;
		public float Interval { get; protected set; }

		public void Update()
		{
			currentInterval += Time.deltaTime;
			if (currentInterval >= Interval)
			{
				currentInterval -= Interval;

				if (WhenList.ContainsKey(Method) && !WhenList[Method].Check())
					return;

				Invoke();
			}
		}

		public void Reset()
		{
			currentInterval = 0;
		}
		
		public int CompareTo(OnRefresh other)
		{
			return Priority - other.Priority;
		}

		public OnRefresh(float interval) : this(interval, 0.0f, 0)
		{
		}

		public OnRefresh(float interval, float startIntervalValue) : this(interval, startIntervalValue, 0)
		{
		}

		public OnRefresh(float interval, float startIntervalValue, RunLevel runLevel) : this(interval, startIntervalValue,
			(int) runLevel)
		{
		}

		public OnRefresh(float interval, float startIntervalValue, int priority)
		{
			Interval = interval;
			currentInterval = startIntervalValue;
			Priority = priority;
		}

		public OnRefresh() : this(1.0f, 0.0f, 0)
		{
		}

		public override string ToString()
		{
			return string.Format("[Refresh: Interval={0}/{1} P={2}]", Interval, currentInterval, Priority);
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnAwake : MonoBehaviourExtAttribute, IComparable<OnAwake>
	{

		public OnAwake(RunLevel runLevel) : this((int) runLevel)
		{
		}

		public OnAwake() : this(0)
		{
		}

		public OnAwake(int priority)
		{
			Priority = priority;
		}

		public override void Invoke()
		{
			if (WhenList.ContainsKey(Method) && !WhenList[Method].Check())
				return;

			base.Invoke();
		}

		public int CompareTo(OnAwake other)
		{
			return Priority - other.Priority;
		}

		public override string ToString()
		{
			return string.Format("[Awake: Priority={0}]", Priority);
		}

	}
	
	
	
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnDestroy : MonoBehaviourExtAttribute, IComparable<OnDestroy>
	{

		public OnDestroy(RunLevel runLevel) : this((int) runLevel)
		{
		}

		public OnDestroy() : this(0)
		{
		}

		public OnDestroy(int priority)
		{
			Priority = priority;
		}

		public override void Invoke()
		{
			if (WhenList.ContainsKey(Method) && !WhenList[Method].Check())
				return;

			base.Invoke();
		}

		public int CompareTo(OnDestroy other)
		{
			return Priority - other.Priority;
		}

		public override string ToString()
		{
			return string.Format("[Destroy: Priority={0}]", Priority);
		}

	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnWhen : MonoBehaviourExtAttribute
	{

		private Func<bool> check = null;

		public OnWhen(Func<bool> check)
		{
			this.check = check;
		}

		public bool Check()
		{
			return check.Invoke();
		}

		public override string ToString()
		{
			return string.Format("[When]");
		}

	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnUpdate : MonoBehaviourExtAttribute, IComparable<OnUpdate>
	{

		public OnUpdate(RunLevel runLevel) : this((int) runLevel)
		{
		}

		public OnUpdate() : this(0)
		{
		}

		public OnUpdate(int priority)
		{
			Priority = priority;
		}

		public override void Invoke()
		{
			if (WhenList.ContainsKey(Method) && !WhenList[Method].Check())
				return;

			base.Invoke();
		}

		public int CompareTo(OnUpdate other)
		{
			return Priority - other.Priority;
		}


		public override string ToString()
		{
			return string.Format("[Update: Priority={0}]", Priority);
		}

	}
	
	
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnEnable : MonoBehaviourExtAttribute, IComparable<OnUpdate>
	{

		public OnEnable(RunLevel runLevel) : this((int) runLevel)
		{
		}

		public OnEnable() : this(0)
		{
		}

		public OnEnable(int priority)
		{
			Priority = priority;
		}

		public override void Invoke()
		{
			if (WhenList.ContainsKey(Method) && !WhenList[Method].Check())
				return;

			base.Invoke();
		}

		public int CompareTo(OnUpdate other)
		{
			return Priority - other.Priority;
		}


		public override string ToString()
		{
			return string.Format("[Enable: Priority={0}]", Priority);
		}

	}
	
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnDisable : MonoBehaviourExtAttribute, IComparable<OnUpdate>
	{

		public OnDisable(RunLevel runLevel) : this((int) runLevel)
		{
		}

		public OnDisable() : this(0)
		{
		}

		public OnDisable(int priority)
		{
			Priority = priority;
		}

		public override void Invoke()
		{
			if (WhenList.ContainsKey(Method) && !WhenList[Method].Check())
				return;

			base.Invoke();
		}

		public int CompareTo(OnUpdate other)
		{
			return Priority - other.Priority;
		}


		public override string ToString()
		{
			return string.Format("[Disable: Priority={0}]", Priority);
		}

	}
	

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnDelay : MonoBehaviourExtAttribute, IComparable<OnDelay>
	{

		public float interval = 0.1f;
		public float currentInterval = 0.0f;
		public bool IsUpdated { get; protected set; }

		public OnDelay() : this(0.1f)
		{
		}

		public OnDelay(float interval) : this(interval, 0)
		{
		}

		public OnDelay(float interval, RunLevel runLevel) : this(interval, (int) runLevel)
		{
		}

		public OnDelay(float interval, int priority)
		{
			Priority = priority;
			this.interval = interval;
			IsUpdated = false;
		}

		public bool Update()
		{
			if (IsUpdated)
				return true;


			if (WhenList.ContainsKey(Method) && !WhenList[Method].Check())
				return false;



			currentInterval += Time.deltaTime;
			if (currentInterval >= interval)
			{
				Invoke();
				IsUpdated = true;
				return true;
			}
			return false;
		}

		public int CompareTo(OnDelay other)
		{
			return Priority - other.Priority;
		}

		public override string ToString()
		{
			return string.Format("[Delay: Priority={0}]", Priority);
		}
	}


	public enum RunLevel
	{
		High = -100,
		Medium = 0,
		Low = 100
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnStart : MonoBehaviourExtAttribute, IComparable<OnStart>
	{

		public OnStart(RunLevel runLevel) : this((int) runLevel)
		{
		}

		public OnStart() : this(0)
		{
		}

		public OnStart(int priority)
		{
			Priority = priority;
		}

		public int CompareTo(OnStart other)
		{
			return Priority - other.Priority;
		}

		public override void Invoke()
		{
			if (WhenList.ContainsKey(Method) && !WhenList[Method].Check())
				return;

			base.Invoke();
		}

		public override string ToString()
		{
			return string.Format("[Start: Priority={0}]", Priority);
		}
	}


	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OnTextUpdate : MonoBehaviourExtAttribute, IComparable<OnStart>
	{

		public OnTextUpdate(RunLevel runLevel) : this((int) runLevel)
		{
		}

		public OnTextUpdate() : this(0)
		{
		}

		public OnTextUpdate(int priority)
		{
			Priority = priority;
		}

		public int CompareTo(OnStart other)
		{
			return Priority - other.Priority;
		}

		public void Invoke(string text, Action<string> callback)
		{
			Method.Invoke(Target, new object[] {text, callback});
		}
	}

	
	
	#endregion

}