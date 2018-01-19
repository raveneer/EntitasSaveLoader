//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public SomeFloatComponent someFloat { get { return (SomeFloatComponent)GetComponent(GameComponentsLookup.SomeFloat); } }
    public bool hasSomeFloat { get { return HasComponent(GameComponentsLookup.SomeFloat); } }

    public void AddSomeFloat(float newValue) {
        var index = GameComponentsLookup.SomeFloat;
        var component = CreateComponent<SomeFloatComponent>(index);
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceSomeFloat(float newValue) {
        var index = GameComponentsLookup.SomeFloat;
        var component = CreateComponent<SomeFloatComponent>(index);
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveSomeFloat() {
        RemoveComponent(GameComponentsLookup.SomeFloat);
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

    static Entitas.IMatcher<GameEntity> _matcherSomeFloat;

    public static Entitas.IMatcher<GameEntity> SomeFloat {
        get {
            if (_matcherSomeFloat == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.SomeFloat);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherSomeFloat = matcher;
            }

            return _matcherSomeFloat;
        }
    }
}