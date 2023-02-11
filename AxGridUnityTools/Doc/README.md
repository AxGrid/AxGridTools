AxGridUnityTools
================

* [Библиотека для расширения функционала стандартного MonoBehaviour](#расширения-функционала-стандартного-monobehaviour)
* [Модель данных в виде Dictionary<string, object>](Model.md)
* [Очередь событий объектов](Model.md#eventmanager)
* [FSM](FSM.md)
* [Как использовать AxGridUnityTools](Use.md)
* [Простой аниматор Path](Path.md)

## Расширения функционала стандартного MonoBehaviour

Как расчишить свой компонент от MonoBehaviour? Наследоваться от MonoBehaviourExt!

```csharp

public MyComponent : MonoBehaviourExt
{
    [OnStart]
    public void Init()
    {
        Debug.Log("Hello world!");
    }
}

```

> Важно понимать что методы Start, Awake, Destroy, Update заменены на атрибуты [OnStart], [OnAwake] итп
> Использовать стандарные нельзя

Какие возможности дает MonoBehaviourExt?

Множественные инициализаторы, в том числе и наследованных компонентов

```csharp
public MyComponent : MonoBehaviourExt
{
    [OnStart]
    public void Init()
    {
        Debug.Log("Hello world!"));
    }
    
    [OnStart(Priority.High)]
    public void Init2()
    {
        Debug.Log("I'm first Hello world!"));
    }
}
```

Таймеры и покадровые лупы

```csharp

public MyComponent : MonoBehaviourExt
{
    [OnDelay(1.0f]
    public void Wait1SecondAndExecute()
    {
        Debug.Log("I'm executed after 1 second!"));
    }
    
    [OnLoop(2f)]
    public void ExecuteEvery2Sec()
    {
        Debug.Log("I'm executed every 2 seconds!"));
    }
}

```

Полный список атребутов

| Атрибут            | Описание                           |
|--------------------|------------------------------------|
| [OnAwake]          | Вызывается в методе Awake          |
| [OnStart]          | Вызывается в методе Start          |
| [OnDestroy]        | Вызывается в методе Destroy        |
| [OnEnable]         | Вызывается в методе OnEnable       |
| [OnDisable]        | Вызывается в методе OnDisable      |
| [OnUpdate]         | Вызывается каждый кадр             |
| [OnRefresh(float)] | Вызывается через каждый интервал   |
| [OnDelay(float)]   | Вызывается через заданный интервал |

Каждый из атрибутов имеет параметр Priority, который позволяет задать приоритет вызова метода

Так-же в каждом MonoBehaviourExt есть доступ 
* к моделе данных через свойство Model
* к системе логирования через Log.Debug Log.Info Log.Warning Log.Error


