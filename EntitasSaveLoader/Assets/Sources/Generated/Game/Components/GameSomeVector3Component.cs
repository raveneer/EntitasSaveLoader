//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public SomeVector3Component someVector3 { get { return (SomeVector3Component)GetComponent(GameComponentsLookup.SomeVector3); } }
    public bool hasSomeVector3 { get { return HasComponent(GameComponentsLookup.SomeVector3); } }

    public void AddSomeVector3(UnityEngine.Vector3 newValue) {
        var index = GameComponentsLookup.SomeVector3;
        var component = CreateComponent<SomeVector3Component>(index);
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceSomeVector3(UnityEngine.Vector3 newValue) {
        var index = GameComponentsLookup.SomeVector3;
        var component = CreateComponent<SomeVector3Component>(index);
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveSomeVector3() {
        RemoveComponent(GameComponentsLookup.SomeVector3);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherSomeVector3;

    public static Entitas.IMatcher<GameEntity> SomeVector3 {
        get {
            if (_matcherSomeVector3 == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.SomeVector3);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherSomeVector3 = matcher;
            }

            return _matcherSomeVector3;
        }
    }
}