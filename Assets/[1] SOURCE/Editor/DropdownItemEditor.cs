//�������� � ���������� ������������ ������������ ���
using Germanenko.Source;
using UnityEditor;

//���� ��������� �� ��������� ����� ��������� ������������ ��������������
[CustomEditor(typeof(DropDownItem))]
[CanEditMultipleObjects]

public class DropdownItemEditor : Editor
{
    DropDownItem subject;
    SerializedProperty dropdownItemType;

    SerializedProperty taskType;
    SerializedProperty nameText;
    SerializedProperty image;

    //������� ����� ������� ��������� � ����������� � ��������� ����
    void OnEnable()
    {
        subject = target as DropDownItem;

        dropdownItemType = serializedObject.FindProperty("DropdownItemType");

        taskType = serializedObject.FindProperty("TaskType");
        nameText = serializedObject.FindProperty("_nameText");

        image = serializedObject.FindProperty("_image");
    }

    //�������������� ������� ��������� ����������
    public override void OnInspectorGUI()
    {
        //����� ���������� � ������. ����� ���� �������� ���������� ������ ������ �
        //����� �� � ���� ������������ ��� ���������.
        serializedObject.Update();

        //����� � �������� ����������� ����
        EditorGUILayout.PropertyField(dropdownItemType);

        EditorGUILayout.PropertyField(image);

        //�������� ���������� ������ � ���������� ����, 
        if (subject.DropdownItemType == DropdownItemType.Task)
        {
            EditorGUILayout.PropertyField(taskType);

            EditorGUILayout.PropertyField(nameText);
        }

        //����� ���������� � �����
        serializedObject.ApplyModifiedProperties();
    }
}