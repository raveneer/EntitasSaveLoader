using Entitas;
using Entitas.VisualDebugging.Unity;
using UnitTestProject;
using UnityEditor;
using UnityEngine;

public class EntityStyleWindow : EditorWindow
{
   /* private string _assetNameForSave = "";
    private string _assetNameForLoad = "";
    private string _selectedEntity = "";
    private IEntity _entity;

    [MenuItem("Tools/Entity Style Save Loader")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EntityStyleWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Entity", EditorStyles.boldLabel);
        
        //선택한 엔티티출력- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        EditorGUILayout.TextField("selectedEntity", _selectedEntity);

        //선택한것이 엔티티 게임오브젝트 일 때만.
        if ((Selection.activeGameObject) && (Selection.activeGameObject.GetComponent<EntityBehaviour>()))
        {
            _entity = Selection.activeGameObject.GetComponent<EntityBehaviour>().entity;
            _selectedEntity = _entity.ToString();
        }
        
        //현재 선택된 엔티티를 이름짓고 어셋으로 저장할 수 있다.- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        _assetNameForSave = EditorGUILayout.TextField("Name of save entityStyle:", _assetNameForSave);

        if (GUILayout.Button("Save EntityStyle to scriptableObject"))
        {
            if (_entity != null)
            {
                EntityStyleInfo asset = ScriptableObject.CreateInstance<EntityStyleInfo>();
                asset.EntityStyleName = _assetNameForSave;
                asset.Json = EntityJsonUtility.MakeEntityInfoJson(_entity);
                AssetDatabase.CreateAsset(asset, $"Assets/Resources/EntityStyleAsset/{_assetNameForSave}.asset");
                AssetDatabase.SaveAssets();

                Debug.Log($"{_assetNameForSave} entityStyle asset created!");
            }
        }


        GUILayout.Label("Make new Entity wity style", EditorStyles.boldLabel);

        //스크립터블 오브젝트 어셋을 이용해서 엔티티를 생성할 수 있다.- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        _assetNameForLoad = EditorGUILayout.TextField("entityStyle Name :", _assetNameForLoad);

        if (GUILayout.Button("Make new Entity!"))
        {
            EntityStyleInfo asset = AssetDatabase.LoadAssetAtPath<EntityStyleInfo>($"Assets/Resources/EntityStyleAsset/{_assetNameForLoad}.asset");
            EntityJsonUtility.MakeNewEntity(asset.Json, Contexts.sharedInstance);

            Debug.Log($"{_assetNameForLoad} entity created!");
        }


        GUILayout.Label("Reset", EditorStyles.boldLabel);
        //초기화
        if (GUILayout.Button("Clear"))
        {
            _assetNameForSave = "";
            _assetNameForLoad = "";
            _selectedEntity = "";
        }
    }*/
}