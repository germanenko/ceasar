//Начинаем с добавления необходимого пространства имён
using Germanenko.Source;
using UnityEditor;

//Этим атрибутом мы объявляем какой компонент подвергнется редактированию
[CustomEditor(typeof(DropDownItem))]
[CanEditMultipleObjects]

public class DropdownItemEditor : Editor
{
    DropDownItem subject;
    SerializedProperty dropdownItemType;

    SerializedProperty taskType;
    SerializedProperty nameText;
    SerializedProperty image;

    //Передаём этому скрипту компонент и необходимые в редакторе поля
    void OnEnable()
    {
        subject = target as DropDownItem;

        dropdownItemType = serializedObject.FindProperty("DropdownItemType");

        taskType = serializedObject.FindProperty("TaskType");
        nameText = serializedObject.FindProperty("_nameText");

        image = serializedObject.FindProperty("_image");
    }

    //Переопределяем событие отрисовки компонента
    public override void OnInspectorGUI()
    {
        //Метод обязателен в начале. После него редактор компонента станет пустым и
        //далее мы с нуля отрисовываем его интерфейс.
        serializedObject.Update();

        //Вывод в редактор выпадающего меню
        EditorGUILayout.PropertyField(dropdownItemType);

        EditorGUILayout.PropertyField(image);

        //Проверка выбранного пункта в выпадающем меню, 
        if (subject.DropdownItemType == DropdownItemType.Task)
        {
            EditorGUILayout.PropertyField(taskType);

            EditorGUILayout.PropertyField(nameText);
        }

        //Метод обязателен в конце
        serializedObject.ApplyModifiedProperties();
    }
}