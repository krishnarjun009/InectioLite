# InectioLite

Inectio is a lite weight dependency framework for small and upto mid range projects. If your project is more than mid range I would prefer to choose Zenject or StrangeIoc.

Inectio is designed from the experience of StrangeIoc framework (MVCS). Inectio doesn't have any inbuilt design such as MVCS or MVVM etc. So you can your own design pattern on top that you can apply dependency injection using Inectio.

Core Features:

 - Core Binding
 - Injection (Property, Field, Method) (Doesn't support for constructor injection).
 - Signal
  - Listen Attribute (where you can add listeners to singals using one line)
  - Commands
  
  Terminology

  - InjectionBinder
  - CommandBinder
  - RootContext
  - InectioBootStrap
  - Signal
  - Command
  - Attributes
 
InjectionBinder where you can Map all your dependecy objects like data models, signals or etc. This is Core binder and controlls other binders. for EX: CommandBinder.

  - Syntax: 
  ``` C#
         injectionBinder.Map<SampleData>();
         injectionBinder.Map<TestSignal>();
         injectionBinder.Map<JumpInputSignal>();
         injectionBinder.Map<OnPlayerDiedSignal>();
         injectionBinder.Map<ISample, SampleData>();
  ```
         

CommandBinder where you can Map Signal to Command. When signal will dispatch Command Execute method will run. Commands are generic type, so you can specify the data types in generic way. Since Generics will support max 4 parameters inside. So you can use class or struct object.

Signal and Command mapping is One - One Mapping means one signal binds with one command of the whole game life time. You can't bind more than one command to the same signal.

  - Syntax:
  ``` C#
          commandBinder.Map<TestSignal, TestCommand>();
          commandBinder.Map<UpdatePlayerDataSignal, UpdatePlayerDataCommand>().Pooled();
          commandBinder.Map<TestSignal, AnotherTestCommand>(); //(this will gives an exception ).
  ```
         
If you are using CommandFrequently in Update() or any loop, Make it to Pooled(). Otherwise this will give GC as per object size. Pooled() Commands are GC Free.

RootContext will give override method where all dependency bindings are mentioned. Inectio Lite will support only single context means you must have only on context of entire game life like Singleton. So if your game have multiple scenes in additive, add bootstrap inside common scene or use Unity's DontDestroyOnLoad() method.

InectioBootStrap is required for initalize RootContext and it is Monobehaviour. So you have to attach this to Global gameobject.

# Singal
Signals are event delegates. Signals are core system in any DI framework. It will communicate multiple objects when someting is happen in the game.

   - Syntax:
   ``` C#
         public class TestSignal : Signal { }
         public class UpdatePlayerDataSignal : Signal<int, float, string> { }
   ```
         
# Commands
Commands are useful to update game data or Make Server Api's calls from command to service. I recommended use commands for data updation or api calls. You have to override the execute method. Command Parameters should match with signal parameters. Otherwise it will an exception says, "method arguments are Invalid".

   - Syntax:
   ``` C#
          public class TestCommand : Command
          {
              public override void Execute()
              {

              }
          }

         public class UpdatePlayerDataCommand : Command<int, float, string>
         {
             public override void Execute(int value, float flt, string str)
             {

             }
         }
   ```
   
   # Attributes
   ###### Inject
    this attribute will work on property, field or any method to inject required dependencies.
    - Syntax:
    ``` C#
    
          [Inject] private TestSignal testSignalP { get; set; } // property injection
          [Inject] private TestSignal testSignalF; // field injection
          private TestSignal _testSignal;
          
          [Inject]
          private void InjectDependencies(TestSignal testSignal) // method injection
          {
            _testSignal = testSignal;
          }
          
          [Inject]
          private void InjectDependencies([Inject("another")] TestSignal testSignal) // method injection
          {
            _testSignal = testSignal;
          }
          
    ```
      You can provide inject attribute to method parameters if there are multiple bindings with different names.
      
   ###### Listen
   This attribute is useful for adding listeners to signals in single line.
   - Syntax:
   ``` C#
        
        [Listen(typeof(TestSignal)]
        private void Test()
        {
         Debug.Log("Testing Listen attribute");
        }
        
        Listen(typeof(TestSignal), Listen.ListenType.AUTO)]
        private void TestAuto()
        {
         Debug.Log("TOnly listens at OnEnable");
        }
        
   ```
    
    All Listen attributes ListenTypes are MANUAL by default means at OnDestroy time listeners will remove. If it is AUTO means listers will remove at OnDisable and will add at OnEnable. So if the gameobject is enable Method will listen otherwise listener will remove.

Check the Demo Project for Better understanding. It is lightweight IOC Framework for smaller or below mid range projects. Add if you find any issues pls mention in Issues Section.

😃THANKS
