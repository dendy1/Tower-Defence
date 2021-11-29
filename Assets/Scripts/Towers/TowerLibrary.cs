using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense.Towers.Data
{
    [CreateAssetMenu(fileName = "TowerLibrary.asset", menuName = "TowerDefense/Tower Library", order = 1)]
    public class TowerLibrary : ScriptableObject, IList<Tower>, IDictionary<string, Tower>
    {
	    /// <summary>
		/// The list of all the towers
		/// </summary>
		public List<Tower> towerConfigurations;

		/// <summary>
		/// The internal reference to the dictionary made from the list of towers
		/// with the name of tower as the key
		/// </summary>
		Dictionary<string, Tower> _configurationDictionary;

		/// <summary>
		/// The accessor to the towers by index
		/// </summary>
		/// <param name="index"></param>
		public Tower this[int index]
		{
			get { return towerConfigurations[index]; }
		}

		public void OnBeforeSerialize()
		{
		}

		/// <summary>
		/// Convert the list (m_Configurations) to a dictionary for access via name
		/// </summary>
		public void OnAfterDeserialize()
		{
			if (towerConfigurations == null)
			{
				return;
			}
			_configurationDictionary = towerConfigurations.ToDictionary(t => t.towerName);
		}

		public bool ContainsKey(string key)
		{
			return _configurationDictionary.ContainsKey(key);
		}

		public void Add(string key, Tower value)
		{
			_configurationDictionary.Add(key, value);
		}

		public bool Remove(string key)
		{
			return _configurationDictionary.Remove(key);
		}

		public bool TryGetValue(string key, out Tower value)
		{
			return _configurationDictionary.TryGetValue(key, out value);
		}

		Tower IDictionary<string, Tower>.this[string key]
		{
			get { return _configurationDictionary[key]; }
			set { _configurationDictionary[key] = value; }
		}

		public ICollection<string> Keys
		{
			get { return ((IDictionary<string, Tower>) _configurationDictionary).Keys; }
		}

		ICollection<Tower> IDictionary<string, Tower>.Values
		{
			get { return _configurationDictionary.Values; }
		}

		IEnumerator<KeyValuePair<string, Tower>> IEnumerable<KeyValuePair<string, Tower>>.GetEnumerator()
		{
			return _configurationDictionary.GetEnumerator();
		}

		public void Add(KeyValuePair<string, Tower> item)
		{
			_configurationDictionary.Add(item.Key, item.Value);
		}

		public bool Remove(KeyValuePair<string, Tower> item)
		{
			return _configurationDictionary.Remove(item.Key);
		}

		public bool Contains(KeyValuePair<string, Tower> item)
		{
			return _configurationDictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, Tower>[] array, int arrayIndex)
		{
			int count = array.Length;
			for (int i = arrayIndex; i < count; i++)
			{
				Tower config = towerConfigurations[i - arrayIndex];
				KeyValuePair<string, Tower> current = new KeyValuePair<string, Tower>(config.towerName, config);
				array[i] = current;
			}
		}

		public int IndexOf(Tower item)
		{
			return towerConfigurations.IndexOf(item);
		}

		public void Insert(int index, Tower item)
		{
			towerConfigurations.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			towerConfigurations.RemoveAt(index);
		}

		Tower IList<Tower>.this[int index]
		{
			get { return towerConfigurations[index]; }
			set { towerConfigurations[index] = value; }
		}

		public IEnumerator<Tower> GetEnumerator()
		{
			return towerConfigurations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) towerConfigurations).GetEnumerator();
		}

		public void Add(Tower item)
		{
			towerConfigurations.Add(item);
		}

		public void Clear()
		{
			towerConfigurations.Clear();
		}

		public bool Contains(Tower item)
		{
			return towerConfigurations.Contains(item);
		}

		public void CopyTo(Tower[] array, int arrayIndex)
		{
			towerConfigurations.CopyTo(array, arrayIndex);
		}

		public bool Remove(Tower item)
		{
			return towerConfigurations.Remove(item);
		}

		public int Count
		{
			get { return towerConfigurations.Count; }
		}

		public bool IsReadOnly
		{
			get { return ((ICollection<Tower>) towerConfigurations).IsReadOnly; }
		}
    }
}
