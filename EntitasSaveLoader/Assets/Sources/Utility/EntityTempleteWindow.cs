using Entitas;
using Entitas.VisualDebugging.Unity;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class EntityTemplateSaveLoadWindow : EditorWindow
{
    private string _assetNameForSave = "";
    private string _assetNameForLoad = "";
    private string _selectedEntity = "";
    private string _saveFileName = "";
    private string _loadFileName = "";
    private EntitySaveLoader _entitySaveLoader;
    private IEntity _entity;

    [MenuItem("Tools/Entity template Save Loader")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EntityTemplateSaveLoadWindow));
    }

    private void OnGUI()
    {
        CheckInit();

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

        GUILayout.Label("Save Entity To single asset", EditorStyles.boldLabel);

        //can save to json file
        _assetNameForSave = EditorGUILayout.TextField("Name of template to save:", _assetNameForSave);

        if (GUILayout.Button("Save entity!"))
        {
            if (_entity != null)
            {
                var asset = CreateInstance<EntityTemplate>();
                asset.templateName = _assetNameForSave;
                _entitySaveLoader.GenerateEntityTemplate(_entity, _assetNameForSave);
                AssetDatabase.Refresh();
                _entitySaveLoader.ReLoadTemplets();
                Debug.Log($"{_assetNameForSave} Entity template file created!");
            }
        }

        #endregion

        #region load - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        GUILayout.Label("Make new Entity from text asset", EditorStyles.boldLabel);

        _assetNameForLoad = EditorGUILayout.TextField("Name of template to save:", _assetNameForLoad);

        if (GUILayout.Button("Make new entity!"))
        {
            _entitySaveLoader.MakeEntityFromtemplate(_assetNameForLoad, Contexts.sharedInstance);
        }

        #endregion

        GUILayout.Label("Save Game", EditorStyles.boldLabel);
        _saveFileName = EditorGUILayout.TextField("saveFileName:", _saveFileName);
        if (GUILayout.Button("Save all"))
        {
            _entitySaveLoader.SaveAllEntitiesInScene(Contexts.sharedInstance, _saveFileName);
            AssetDatabase.Refresh();
        }

        GUILayout.Label("Load Game ", EditorStyles.boldLabel);
        _loadFileName = EditorGUILayout.TextField("loadFileName:", _loadFileName);
        if (GUILayout.Button("Load"))
        {
            _entitySaveLoader.LoadEntitiesFromSaveFile(Contexts.sharedInstance, _loadFileName);
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

    private void CheckInit()
    {
        if (_entitySaveLoader == null)
        {
            _entitySaveLoader = new EntitySaveLoader();
            _entitySaveLoader.ReLoadTemplets();
        }
    }
}