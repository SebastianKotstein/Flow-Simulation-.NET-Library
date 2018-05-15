using Skotstein.app.flowsimulation.lib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.model
{
    /// <summary>
    /// Represents a container for one or muliple <see cref="Entity"/> objects
    /// </summary>
    public class Bundle
    {
        #region Entity Management
        private IList<Entity> _entities = new List<Entity>();

        /// <summary>
        /// Returns the number of <see cref="Entity"/> object
        /// </summary>
        public int Count
        {
            get
            {
                return _entities.Count;
            }
        }

        /// <summary>
        /// Adds an <see cref="Entity"/> object to this bundle
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> object to be added</param>
        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Returns the <see cref="Entity"/> object at the specified index position
        /// </summary>
        /// <param name="index">index</param>
        /// <returns><see cref="Entity"/> object at index</returns>
        public Entity GetEntity(int index)
        {
            if(index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return _entities[index];
            }
        }

        /// <summary>
        /// Returns and removes the <see cref="Entity"/> object at the specified index position
        /// </summary>
        /// <param name="index">index</param>
        /// <returns></returns>
        public Entity RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                Entity e = _entities[index];
                _entities.RemoveAt(index);
                return e;
            }
        }

        /// <summary>
        /// Returns the <see cref="Entity"/> object having the passed ID or null, if no such <see cref="Entity"/> object exists
        /// </summary>
        /// <param name="id">id</param>
        /// <returns><see cref="Entity"/> having the ID or null</returns>
        public Entity GetEntityById(string id)
        {
            foreach(Entity e in _entities)
            {
                if (e.Id.CompareTo(id) == 0)
                {
                    return e;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns and removes the <see cref="Entity"/> object having the passed ID or returns null, if no such <see cref="Entity"/> object exists
        /// </summary>
        /// <param name="id">id</param>
        /// <returns><see cref="Entity"/> having the ID or null</returns>
        public Entity RemoveById(string id)
        {
            Entity e = null;
            for (int i = 0; i < Count; i++)
            {
                if (_entities[i].Id.CompareTo(id) == 0)
                {
                    e = _entities[i];
                    _entities.RemoveAt(i);
                    return e;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks whether an <see cref="Entity"/> object having the passed ID exists or not
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>true if the <see cref="Entity"/> object with the passed ID exists, else false</returns>
        public bool HasEntity(string id)
        {
            return GetEntityById(id) != null;
        }

        /// <summary>
        /// Gets a list with contained <see cref="Entity"/> objects
        /// </summary>
        public IList<Entity> Entities
        {
            get
            {
                return _entities;
            }
        }
        #endregion
        #region Header Management
        private IDictionary<string, string> _header = new Dictionary<string, string>();

        /// <summary>
        /// Checks whether the header with the passed name exists
        /// </summary>
        /// <param name="name">name of the header</param>
        /// <returns>true or false</returns>
        public bool HasHeader(string name)
        {
            return _header.ContainsKey(name);
        }

        /// <summary>
        /// Returns the header value having the passed name
        /// </summary>
        /// <param name="name">name of the header</param>
        /// <returns>header value or null</returns>
        public string GetHeader(string name)
        {
            if (HasHeader(name))
            {
                return _header[name];
            }
            else
            {
                throw new HeaderNotFoundException(this);
            }
        }

        /// <summary>
        /// Sets the value of the header having the passed name
        /// </summary>
        /// <param name="name">header name</param>
        /// <param name="value">header value</param>
        public void SetHeader(string name, string value)
        {
            if (HasHeader(name))
            {
                _header[name] = value;
            }
            else
            {
                _header.Add(name, value);
            }

        }

        /// <summary>
        /// Removes the header having the passed name and return its value or null if no such header exists
        /// </summary>
        /// <param name="name">header name</param>
        /// <returns>header value or null</returns>
        public string RemoveHeader(string name)
        {
            if (HasHeader(name))
            {
                return _header[name];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the available header names
        /// </summary>
        public ICollection<string> HeaderNames
        {
            get
            {
                return _header.Keys;
            }
        }
        #endregion
    }
}
