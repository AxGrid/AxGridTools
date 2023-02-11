FSM - Машина состояний
======================

AxGridTools предоставляет реализацию машины состояний (Конечный автомат)
Статичная ссылка на базовый экземпляр fsm находится по пути `Settings.Fsm`

Сосоятоние
----------

Для того чтоб создать состояние нужно пронаследовать ваш класс от обстракции FSMState

```csharp
[State("MyState")]
public class MyState : FSMState
{
    public void Enter()
    {
        Debug.Log("Enter state");
    }

    public void Exit()
    {
        Debug.Log("Exit state");
    }
}
```

Все состояния так-же имеют таймеры, и слушатели событий
Таймеры реализованы атрибтами `[One]` и `[Loop]`
Слушатели атрибутом `[Bind]`

Кажое состояние имеет доступ к самой машине через свойство `Parent`

```csharp
public class MyState : FSMState
{
    public void Enter()
    {
        Debug.Log("Enter state");
    }
    
    [One(1f)]
    public void GoToNextState() {
        Parent.Change("NextMyState");
    }

    public void Exit()
    {
        Debug.Log("Exit state");
    }
}
```

Для отправки сигнала машине состояний существует метод `Settings.Fsm.Invoke("EventName", eventArg1, eventArg2, ...)`

> Для отправки событий рекомендуется использовать общий метод Settings.Invoke("EventName", eventArg1, eventArg2, ...)

> Нужно понимать что событие прийдет только в текущее состояние FSM

```csharp

Создать машину состояний и запустить ее можно через простой класс инициализации

```csharp
// Класс инициализации
public class InitMyFsm : MonoBehaviourExt
{
    [OnAwake]
    private void CreateFsm()
    {
        Settings.Fsm = new FSM();
        Settings.Fsm.Add(new MyInitState()); // Добавим первое состояние
        Settings.Fsm.Add(new MyFirstState()); // Добавим второе состояние
        Settings.Fsm.Add(new MyNextState()); // Добавим третье состояние
    }
    
    [OnStart]
    private void StartFsm()
    {
        Settings.Fsm.Start("MyInitState"); // Запустим машину состояний
    }
    
    [OnUpdate]
    public void UpdateFsm()
    {
        Settings.Fsm.Update(Time.deltaTime); // Необходимо для работы таймеров
    }
}

// Класс первого состояния
public class MyInitState : FSMState
{
    public void Enter()
    {
        Debug.Log("Enter init state");
        Parent.Change("MyFirstState");
    }
}

// Класс второго состояния
public class MyFirstState : FSMState
{
    public void Enter()
    {
        Debug.Log("Enter first state");
    }
    
    [One(1f)]
    public void GoToNextState() {
        Parent.Change("MyNextState");
    }
}

// Класс третьего состояния
public class MyNextState : FSMState
{
    public void Enter()
    {
        Debug.Log("Enter next state");
    }
    
    [One(1f)]
    public void GoToPrevState() {
        Parent.Change("MyFirstState");
    }
}

```