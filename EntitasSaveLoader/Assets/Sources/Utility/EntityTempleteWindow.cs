using Entitas;
using Entitas.VisualDebugging.Unity;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class EntityTempleteSaveLoadWindow : EditorWindow
{
    private string _assetNameForSave = "";
    private string _assetNameForLoad = "";
    private string _selectedEntity = "";
    private string _saveFileName = "";
    private string _loadFileName = "";

    private IEntity _entity;

    [MenuItem("Tools/Entity Templete Save Loader")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EntityTempleteSaveLoadWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Entity", EditorStyles.boldLabel);

        //선택한 엔티티출력- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        EditorGUILayout.TextField("selectedEntity", _selectedEntity);

        //선택한것이 엔티티 게임오브젝트 일 때만.
        if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<EntityBehaviour>())
        {
            _entity = Selection.activeGameObject.GetComponent<EntityBehaviour>().entity;
            _selectedEntity = _entity.ToString();
        }

        #region save - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        GUILayout.Label("Save Entity To asset", EditorStyles.boldLabel);

        //can save to json file
        _assetNameForSave = EditorGUILayout.TextField("Name of templete to save:", _assetNameForSave);

        if (GUILayout.Button("Save entity!"))
        {
            if (_entity != null)
            {
                var asset = CreateInstance<EntityTemplete>();
                asset.TempleteName = _assetNameForSave;
                EntitySaveLoader.GenerateEntityTemplete(_entity, _assetNameForSave);
                AssetDatabase.Refresh();
                EntitySaveLoader.ReloadTempletesFromResource();
                Debug.Log($"{_assetNameForSave} EntityTemplete text file created!");
            }
        }

        #endregion

        #region load - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        GUILayout.Label("Make new Entity from text asset", EditorStyles.boldLabel);

        _assetNameForLoad = EditorGUILayout.TextField("Name of templete to save:", _assetNameForLoad);

        if (GUILayout.Button("Make new entity!"))
        {
            EntitySaveLoader.MakeEntityFromTempleteName(_assetNameForLoad, Contexts.sharedInstance);
        }

        #endregion

        GUILayout.Label("Save All Entities (have savingData) ", EditorStyles.boldLabel);
        _saveFileName = EditorGUILayout.TextField("saveFileName:", _saveFileName);
        if (GUILayout.Button("Save all"))
        {
            EntitySaveLoader.SaveAllEntitiesInScene(Contexts.sharedInstance, _saveFileName);
            AssetDatabase.Refresh();
        }

        GUILayout.Label("Load Save Entities (have savingData) ", EditorStyles.boldLabel);
        _loadFileName = EditorGUILayout.TextField("loadFileName:", _loadFileName);
        if (GUILayout.Button("Load all"))
        {
            EntitySaveLoader.LoadEntitiesFromSaveFile(Contexts.sharedInstance, _loadFileName);
        }


        //load templete groups- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        if (GUILayout.Button("Load Templete Groups"))
        {
            EntitySaveLoader.ReloadTempleteGroupFromResource();
        }

        //reset- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        GUILayout.Label("Reset", EditorStyles.boldLabel);

        if (GUILayout.Button("Clear"))
        {
            _assetNameForSave = "";
            _assetNameForLoad = "";
            _selectedEntity = "";
        }
    }
}