using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skotstein.app.flowsimulation.lib.model;

namespace Skotstein.app.flowsimulation.lib.connection
{
    /// <summary>
    /// Connector class for assigning a list of stored attributes to a <see cref="Bundle"/>. The attributes are added as header of the incoming <see cref="Bundle"/> object.
    /// Note that for every incoming <see cref="Bundle"/> the same set of attributes is assigend. If a dynamic assignemnt is required (e.g. depending on the underlying <see cref="Entity"/> type), the super class<see cref="AttributeSetter"/> can be used by
    /// imlementing the <see cref="In(Bundle)"/> method.
    /// </summary>
    public class StaticAttributeSetter : AttributeSetter
    {
        #region Attributes
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();
        /// <summary>
        /// Clears the set of attributes which should be assigned to an incoming <see cref="Bundle"/>
        /// </summary>
        public void ClearAttributes()
        {
            _attributes.Clear();
        }
        /// <summary>
        /// Adds an attribute to the set which should be assigned to an incoming <see cref="Bundle"/>
        /// </summary>
        /// <param name="name">name of the attribute</param>
        /// <param name="value">value of the attribute</param>
        public void SetAttribute(string name, string value)
        {
            if (_attributes.ContainsKey(name))
            {
                _attributes[name] = value;
            }
            else
            {
                _attributes.Add(name, value);
            }
        }
        /// <summary>
        /// Removes an attribute from the set which should be assigned to an incoming <see cref="Bundle"/>
        /// </summary>
        /// <param name="name">name of the attribute</param>
        public void RemoveAttribute(string name)
        {
            if (_attributes.ContainsKey(name))
            {
                _attributes.Remove(name);
            }
        }

        /// <summary>
        /// Checks whether an attribute is contained within the set which should be assigned to an incoming <see cref="Bundle"/>
        /// </summary>
        /// <param name="name">name of the attribute</param>
        /// <returns></returns>
        public bool HasAttribute(string name)
        {
            return _attributes.ContainsKey(name);
        }
        #endregion

        public override void In(Bundle bundle)
        {
            foreach(string name in _attributes.Keys)
            {
                bundle.SetHeader(name, _attributes[name]);
            }
            Successor?.In(bundle);
        }
    }
}
