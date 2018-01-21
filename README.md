# EntitasSaveLoader v0.4
can save/load entity info with Json file.

https://www.youtube.com/watch?v=TzMSpb3g7_A&feature=youtu.be

https://github.com/sschmid/Entitas-CSharp

## version v0.4
added  [IgnoreSave] class attribute. with this, component will not be saved.

some class reference can be serialized, some not. checking this can be difficult. so, add attribute hint for SaveLoader please.

ex) Unity.Vector3 can be save by this SaveLoader, but GameObject not. so we need to add [IgnoreSave] attribute to. (else, you will have null value component)

```
[Game]
public class SomeVector3Component : IComponent
{
    public Vector3 Value;
}

[IgnoreSave]
[Game]
public class SomeGameObjectRefTypeComponent : IComponent
{
    public GameObject Ref;
}
```

## version v0.3
stop support scriptable oject.

can save/load all entities with 'SaveDataComponent'

fix not group added bug.

## version v0.2
can save/load by Json file.

## install
1. go to asset store, install Json.Net.
2. copy to Assets\Sources\Utility to your project
3. done!

## save entity
1. make an entity in runtime.
2. select an entity gameobject from hiarachy
3. open tool/entity templete save loader
4. enter name of templete
5. save!
6. you can find Json asset in asAssets\Resources\EntityTemplete

## load and make entity
1. play game
2. open tool/entity templete save loader
3. type name of templete
4. make it!

## save/load all entity
1. add 'savingDataCompontnt' flag to your desired entities.
2. open tool/entity templete save loader
3. save all!
4. you got json file.
5. load all!
6. you got new entities. 

## what is "Entity Templete"?
entity templete is entity's context info + components info. it have values too.

## next?
clear entities before load?

merge to blueprint?

support scriptable object again?
