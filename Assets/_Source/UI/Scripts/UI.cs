using System.Collections.Generic;
using UnityEngine;


namespace UISystem
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private RectTransform _levelParent;
        [SerializeField] private RectTransform _mainMenuParent;

        private Dictionary<IUISpawner, List<Component>> _spawnedElements = new Dictionary<IUISpawner, List<Component>>();
        private Dictionary<UIType, RectTransform> _elementsParents = new Dictionary<UIType, RectTransform>();

        private void Awake()
        {
            _elementsParents.Add(UIType.Level, _levelParent);
            _elementsParents.Add(UIType.MainMenu, _mainMenuParent);
        }

        public T CreateUIElement<T>(T prefab, UIType type, IUISpawner spawner) where T : Component
        {
            var element = Instantiate(prefab, _elementsParents[type]);

            if (_spawnedElements.ContainsKey(spawner))
                _spawnedElements[spawner].Add(element);
            else
                _spawnedElements[spawner] = new List<Component>() { element };

            return element;
        }

        public bool DestroyElement(Component element, float offset, IUISpawner spawner)
        {
            if (_spawnedElements.ContainsKey(spawner) && _spawnedElements[spawner].Remove(element))
            {
                Destroy(element.gameObject, offset);
                return true;
            }

            return false;
        }

        public void ClearComponentsForSpawner(IUISpawner spawner)
        {
            foreach (var element in _spawnedElements[spawner])
            {
                Destroy(element.gameObject);
            }
        }
    }

    public interface IUISpawner{}

    public enum UIType
    {
        Level,
        MainMenu
    }
}
