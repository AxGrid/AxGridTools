Path
====

Path - один из инструментов AxGridUnityTools. Он позволяет создавать простые пошаговые действия.

Например вам нужно переместить объект из точки (0,0) в точку (1,1) за 2 секунды, потом проиграть звук завершения, подождать 1 секунда и уничтожить его, при этом еще и сообщив событие что действие выполнено.

```csharp
public class MyComponent : MonoBehaviourExt
{
    [OnStart]
    public void Init()
    {
        this.path = new Path()
            .EasingLinear(2f, 0, 1, (f) => transform.position = Lerps.Vector2(Vector2.zero, Vector2.one, f))
            .Action(Settings.Invoke("OnSoundManadgerPlay", "Ding"));
            .Wait1f);
            .Action(() => {
                Settings.Invoke("OnMyEvent");
                Destroy(gameObject);
            });
    }
}
```

Этот путь исполнится на старте и отработает последовательно все действия.

Путь основан на очень простом принципе. 
Есть цепь методов, которая должна вызыватся каждый кадр, пока в ней есть хотябы один метод. 
В каждый такой метод будет передаватся контекст пути, а на выходе он должен вернуть один из 4х вариантов:

* `Status.OK` - перейти к следующему методу в пути, в следующем кадре
* `Status.Immediately` - перейти к следующему методу в пути, в текущем кадре
* `Status.Continue` - оставаться на текущем методе в пути, в следующем кадре
* `Status.Error` - перевести путь в состояние ошибки и исполнять его методы добавленные через .Error()
* `Status.Stop` - остановить путь

*Контекст* пути содержит время от начала исполнения данного метода (DeltaF), всего пути (PathStartTimeF) 

```csharp
public class MyTestPath : MonoBehaviourExt
{
    [OnStart]
    public void Init()
    {
        this.path = new Path()
            .Add((context) => {
                Debug.Log("1"); // Будем показывать в течении 1 секунды
                if (context.DeltaF > 1f) { // Проверим что прошло 1 секунда от начала метода
                    return Status.OK;
                }
                return Status.Continue;
            })
            .Add((context) => {
                Debug.Log("2"); // Покажем один раз
                return Status.OK;
            })
    }
}
```

Все остальные методы пути просто надстройки над текущей реализацией.

Например простой метод ожидания на указанное количество секунд 

```csharp
public static class MyPathHelpers {
    public static CPath MyWait(this CPath path, float time) {
        return path.Add((context) => {
            if (context.DeltaF > time) {
                return Status.OK;
            }
            return Status.Continue;
        });
    }
}
```

В текущей реализации добавлено много методов Easing для упрощения работы с путями.

EasingLinear, EasingQuadIn, EasingQuadOut, EasingQuadInOut, EasingCubicIn, EasingCubicOut, EasingCubicInOut, EasingQuartIn, EasingQuartOut, EasingQuartInOut, EasingQuintIn, EasingQuintOut, EasingQuintInOut, EasingSineIn, EasingSineOut, EasingSineInOut, EasingExpoIn, EasingExpoOut, EasingExpoInOut, EasingCircIn, EasingCircOut, EasingCircInOut, EasingElasticIn, EasingElasticOut, EasingElasticInOut, EasingBackIn, EasingBackOut, EasingBackInOut, EasingBounceIn, EasingBounceOut, EasingBounceInOut

Сигнатура у них всегда одинакова

`.EasingLinear(float time, float from, float to, (f) => { ... })`

* time - вермя в секундах, которое будет занимать анимация
* from - начальное значение
* to - конечное значение
* (f) => { ... } - метод, который будет вызыватся каждый кадр с текущим значением

например простой счетчик, который за 5 секунд увеличит значение от 0 до 100 с замедлением

```csharp
public class MyTestPath : MonoBehaviourExt
{
    [OnStart]
    public void Calculate()
    {
        this.path = new Path()
            .EasingCircEaseOut(5f, 0f, 100f, (f) => {
                Debug.Log(f);
            })
    }
}
```

