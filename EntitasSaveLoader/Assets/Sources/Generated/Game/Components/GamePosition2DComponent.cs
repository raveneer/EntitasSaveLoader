//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Position2D position2D { get { return (Position2D)GetComponent(GameComponentsLookup.Position2D); } }
    public bool hasPosition2D { get { return HasComponent(GameComponentsLookup.Position2D); } }

    public void AddPosition2D(float newX, float newY) {
        var index = GameComponentsLookup.Position2D;
        var component = CreateComponent<Position2D>(index);
        component.X = newX;
        component.Y = newY;
        AddComponent(index, component);
    }

    public void ReplacePosition2D(float newX, float newY) {
        var index = GameComponentsLookup.Position2D;
        var component = CreateComponent<Position2D>(index);
        component.X = newX;
        component.Y = newY;
        ReplaceComponent(index, component);
    }

    public void RemovePosition2D() {
        RemoveComponent(GameComponentsLookup.Position2D);
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

    static Entitas.IMatcher<GameEntity> _matcherPosition2D;

    public static Entitas.IMatcher<GameEntity> Position2D {
        get {
            if (_matcherPosition2D == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Position2D);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherPosition2D = matcher;
            }

            return _matcherPosition2D;
        }
    }
}
