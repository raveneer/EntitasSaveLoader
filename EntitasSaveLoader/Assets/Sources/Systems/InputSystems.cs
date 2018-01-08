using Entitas;

public class InputSystems : Entitas.Systems
{
    public InputSystems(Contexts contexts)
    {
        Add(new EmitInputSystem(contexts));
    }
}