using System.Collections.Generic;
using UnityEngine;


namespace UISystem
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private RectTransform _elementsParent;

        private Dictionary<IUISpawner, List<Component>> _spawnedElements = new Dictionary<IUISpawner, List<Component>>();

        public T CreateUIElement<T>(T prefab, IUISpawner spawner) where T : Component
        {
            var element = Instantiate(prefab, _elementsParent);

            if (_spawnedElements.ContainsKey(spawner))
                _spawnedElements[spawner].Add(element);
            else
                _spawnedElements[spawner] = new List<Component>() { element };

            return element;
        }

        public bool DestroyElement(Component element, IUISpawner spawner)
        {
            if (_spawnedElements.ContainsKey(spawner) && _spawnedElements[spawner].Remove(element))
            {
                Destroy(element.gameObject);
                return true;
            }

            return false;
        }

        public void ClearComponentsForSpawner(IUISpawner spawner)
        {
            foreach (var element in _spawnedElements[spawner])
            {
                Destroy(element);
            }
        }
    }

    public interface IUISpawner{} 
}
