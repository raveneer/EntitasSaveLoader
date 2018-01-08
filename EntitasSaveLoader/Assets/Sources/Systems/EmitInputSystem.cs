using Entitas;
using UnityEngine;
public class EmitInputSystem : IInitializeSystem, IExecuteSystem
{
    private InputContext _inputContext;
    private InputEntity _leftMouseEntity;
    private InputEntity _rightMouseEntity;
    private InputEntity _middleMouseEntity;
    private InputEntity _spaceKeyEntity;

    public EmitInputSystem(Contexts contexts)
    {
        _inputContext = contexts.input;
    }

    public void Initialize()
    {
        // initialise the unique entities that will hold the mousee button data
        _inputContext.isLeftMouse = true;
        _leftMouseEntity = _inputContext.leftMouseEntity;

        _inputContext.isRightMouse = true;
        _rightMouseEntity = _inputContext.rightMouseEntity;

        _inputContext.isMiddleMouse = true;
        _middleMouseEntity = _inputContext.middleMouseEntity;

        //스페이스키를 추가.
        _inputContext.SetSpaceKey(false);
        _spaceKeyEntity = _inputContext.spaceKeyEntity;
    }

    //executeSystem은 계속 돌아간다.  (컨택스트에서 Execute가 얼마나 자주 불려지는가에 따라 다르지만...)
    public void Execute()
    {
        //Debug.Log("EmitInputSystem is Execute");

        // mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // left mouse button
        if (Input.GetMouseButtonDown(0))
            _leftMouseEntity.ReplaceMouseDown(mousePosition); //Replace는 값을 바꾸는 것이다. Set같은...

        if (Input.GetMouseButton(0))
            _leftMouseEntity.ReplaceMousePosition(mousePosition);

        if (Input.GetMouseButtonUp(0))
            _leftMouseEntity.ReplaceMouseUp(mousePosition);


        // left mouse button
        if (Input.GetMouseButtonDown(1))
            _rightMouseEntity.ReplaceMouseDown(mousePosition);

        if (Input.GetMouseButton(1))
            _rightMouseEntity.ReplaceMousePosition(mousePosition);

        if (Input.GetMouseButtonUp(1))
            _rightMouseEntity.ReplaceMouseUp(mousePosition);

        // middle mouse button
        if (Input.GetMouseButtonDown(2))
            _middleMouseEntity.ReplaceMouseDown(mousePosition);

        if (Input.GetMouseButton(2))
            _middleMouseEntity.ReplaceMousePosition(mousePosition);

        if (Input.GetMouseButtonUp(2))
            _middleMouseEntity.ReplaceMouseUp(mousePosition);

        if(Input.GetKeyDown("space"))
            _spaceKeyEntity.ReplaceSpaceKey(false);

        if (Input.GetKeyUp("space"))
        {
            Debug.Log("space key up");
            _spaceKeyEntity.ReplaceSpaceKey(true);
        }
    }
}