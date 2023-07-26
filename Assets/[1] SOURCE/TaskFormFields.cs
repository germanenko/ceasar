using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



namespace Germanenko.Source
{

    [CreateAssetMenu(fileName = "Task form fields", menuName = "New ceasar/Task form/Fields")]
    public class TaskFormFields : ScriptableObject
    {

        [SerializeField] private GameObject _dropDown;
        [SerializeField] private GameObject _inputField;

        private List<GameObject> _listOfObjects = new();



        public async void CreateFields(Transform parentTransform)
        {

            GameObject newField;
            ClearFields();


            for (int i = 0; i < 10; i++)
            {

                newField = Instantiate(_inputField, parentTransform);
                _listOfObjects.Add(newField);

            }


            for (int i = 0; i < 10; i++)
            {

                newField = Instantiate(_dropDown, parentTransform);
                _listOfObjects.Add(newField);

            }


            await System.Threading.Tasks.Task.Delay(300);

            ActivateField();
           
        }



        public void ClearFields()
        {
            foreach (var item in _listOfObjects) Destroy(item);
            _listOfObjects.Clear();
        }



        public void ActivateNextField(GameObject currentItem)
        {

            for (int i = 0; i < _listOfObjects.Count; i++)
            {
                if(currentItem == _listOfObjects[i])
                {
                    ActivateField(i + 1);
                    break;
                }
            }

        }



        public void ActivateField(int n = 0)
        {

            if (_listOfObjects.Count <= n || n < 0) return;

            //var firstField = _listOfObjects[n].GetComponentInChildren<TMP_InputField>();

            //firstField?.MoveToEndOfLine(false, false);
            //firstField?.ActivateInputField();

        }

    }

}
