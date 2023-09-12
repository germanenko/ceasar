using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace Germanenko.Source
{

    [CreateAssetMenu(fileName = "Task form fields", menuName = "New ceasar/Task form/Fields")]
    public class TaskFormFields : ScriptableObject
    {

        [SerializeField] private Dropdown _dropDown;
        [SerializeField] private InputFieldWithHits _inputField;

        [SerializeField] private List<Dropdown> _listOfDropdown = new();
        [SerializeField] private List<InputFieldWithHits> _listOfInputFields = new();

        public List<Dropdown> ListOfDropdown => _listOfDropdown;
        public List<InputFieldWithHits> ListOfInputFields => _listOfInputFields;

        [SerializeField] private List<GameObject> _listOfObjects = new();

        public UnityEvent AllInputsSpawned;

        public async void CreateFields(Transform parentTransform)
        {

            InputFieldWithHits newInputField;
            Dropdown newDropdown;

            ClearFields();


            for (int i = 0; i < 10; i++)
            {

                newInputField = Instantiate(_inputField, parentTransform);
                _listOfInputFields.Add(newInputField);
                _listOfObjects.Add(newInputField.gameObject);
            }


            for (int i = 0; i < 10; i++)
            {

                newDropdown = Instantiate(_dropDown, parentTransform);
                _listOfDropdown.Add(newDropdown);
                _listOfObjects.Add(newDropdown.gameObject);
            }

            AllInputsSpawned?.Invoke();

            await System.Threading.Tasks.Task.Delay(300);

            ActivateField();
           
        }



        public void ClearFields()
        {
            foreach (var item in _listOfObjects) Destroy(item);
            _listOfObjects.Clear();

            _listOfInputFields.Clear();
            _listOfDropdown.Clear();
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
