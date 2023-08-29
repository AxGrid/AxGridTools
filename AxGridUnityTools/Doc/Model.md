Модель данных
=============

Модель данных в AxGridTools это управляемое KeyValue хранилище с очередью событий

Статичный экземпляр этого хранилища находится в `Settings.Model`. Так-же в каждом `MonoBehaviourExt` есть доступ к модели данных через свойство `Model`

Управление данными модели осуществляется через свойства Get / Set

```csharp
public class MyComponent : MonoBehaviourExt
{
    [OnStart]
    public void Init()
    {
        Model.Set("Hello", "World");
        Debug.Log(Model.Get<string>("Hello")); // World
    }
}
```

Для облегчения разименовывания данных реализовано множество методов получения данных нужного типа
`GetString`, `GetInt`, `GetFloat`, `GetBool`. Эти методы будут пытатся приводить тип к нужному, вне зависимости от данных которые там находятся. 

```csharp
public class MyComponent : MonoBehaviourExt
{
    [OnStart]
    public void Init()
    {
        Model.Set("Hello", "5");
        Debug.Log("{0}", Model.GetInt("Hello") + 4); // 9
    }
}
```

Каждое изменение вызванное методом `Set` генерирует событие `ModelChanged` в котором передается ключ измененного значения и новое значение.
Так-же происходит вызов именованного события On{FieldName}Changed, где FieldName - имя поля которое было изменено.

Подписаться на эти события можно через методы EventManager, Model.EventManager.AddAction("FieldName", ActionMethod)

```csharp
public class MyComponent : MonoBehaviourExt
{
    [OnStart]
    public void Init()
    {
        Model.EventManager.AddAction("Hello", OnHelloChanged);
    }
    
    [OnDelay(1f)]
    public void ChangeHello()
    {
        Model.Set("Hello", "World");
    }

    private void OnHelloChanged()
    {
        var value = Model.Get<string>("Hello");
        Debug.Log($"Hello changed to {value}");
    }
}
```

Так-же есть способ взаимодеятсвия с событиями через атрибуты. 
Для этого нужно наследоватся от MonoBehaviourExtBind и пометить методы которые должны быть вызваны при изменении данных атрибутом `[Bind("FieldName")]`

```csharp
public class MyComponent : MonoBehaviourExtBind
{
    [OnStart]
    public void Init()
    {
        Model.Set("Hello", "World");
    }
    
    [Bind("Hello")]
    public void OnHelloChanged(string value)
    {
        Debug.Log("Hello changed to {0}", value);
    }
}
```

Атрибут `[Bind]` может брать имя из названия метода.
Так-же можно подписатся на несколько событий сразу.

> Не забывайте про то что наследоваться нужно от MonoBehaviourExtBind, а не MonoBehaviourExt

```csharp
public class MyComponent : MonoBehaviourExtBind
{
    [OnStart]
    public void Init()
    {
        Model.Set("A", 5);
        Model.Set("B", 10);
    }
    
    //Увеличиваем значение на 2 каждую секунду
    [OnRepeat(1f)]
    private void ChangeA()
    {   
        Model.Inc("A", 2);
    }

    //Уменьшаем значение на 1 каждые 2 секунды
    [OnRepeat(2f)]
    private void ChangeB()
    {    
        Model.Dec("B", 1);
    }
    
    [Bind("OnAChanged")]
    [Bind("OnBChanged")]
    public void OnAOrBChanged()
    {
        var a = Model.Get<int>("A");
        var b = Model.Get<int>("B");
        Debug.Log("Sum {0}", a + b);
    }
}
```

Так-же атрибут бинд может содержать не явное задание поля, а вычисление его из сериализуемых полей класса.
Это позволяет создавать гибкие настраиваемые компоненты.

```csharp
public class MyComponent : MonoBehaviourExtBind
{
    [SerializeField]
    private string _fieldName = "Hello";

    [OnStart]
    public void Init()
    {
        Model.Set("Hello", "World");
    }
    
    [Bind("On{_fieldName}Changed")]
    public void OnCustomFieldNameChanged(string value)
    {
        Debug.Log("{0} changed to {1}", _fieldName, value);
    }
}
```

EventManager
------------

В каждой моделе есть EventManager, он отвечает за отправку очереде событий, подписанным на них методам.
Cамый простой способ получить доступ к нему - это через ссылку вызвать метод `Settings.Model.EventManager`

```csharp
public class MyComponent : MonoBehaviourExt
{
    [OnStart]
    public void Init()
    {
        Settings.Model.EventManager.AddAction<string,int>("OnMyEvent", OnMyEvent);
    }
    
    // Отправим событие через секунду
    [OnDelay(1f)]
    public void ChangeHello()
    {
        Settings.Model.EventManager.Invoke("OnMyEvent", "Hello", 5);
        // Лучше использовать прямой метод, он так-же отправит события и в текущую FSM
        Settings.Invoke("OnMyEvent", "Hello", 5);
    }

    private void OnMyEvent(string eventStringArgs, int eventIntArgs)
    {
        Debug.Log($"OnMyEvent {eventStringArgs} {eventIntArgs}");
    }
    
    [OnDestroy]
    public void OnDestroy()
    {
        // А вот так можно отписатся от события
        Settings.Model.EventManager.RemoveAction<string,int>("OnMyEvent", OnMyEvent);
    }
   
}
```

Так-же для подписи на события можно использовать конструкцию через атрибут `[Bind]` и компонент `MonoBehaviourExtBind`

```csharp
public class MyComponent : MonoBehaviourExtBind
{
    [OnDelay(1f)]
    public void ChangeHello()
    {
        Settings.Invoke("OnMyEvent", "Hello", 5);
    }

    [Bind("OnMyEvent")]
    private void OnMyEvent(string eventStringArgs, int eventIntArgs)
    {
        Debug.Log($"OnMyEvent {eventStringArgs} {eventIntArgs}");
    }
   
}
```

> Все события подписываются именно в момент `[OnStart]` вызовы событий в `[OnAwake]` не будут обработаны.
