using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skotstein.app.flowsimulation.lib.common;
using Skotstein.app.flowsimulation.lib.model;

namespace Skotstein.app.flowsimulation.lib.connection
{
    /// <summary>
    /// Connector class having multiple destinations (sucessors) and routing an incoming <see cref="Bundle"/> to one of these destinations. The routing decision is made based on the value of a predefined
    /// attribute (header) of the incoming <see cref="Bundle"/>. The routes (destinations) can be initally defined by using <see cref="AddRoute(string, IUnit)"/> - each route has value name which is used as the routing
    /// condition and the destination (implementation of <see cref="IUnit"/>. The header (attribute) (see <see cref="Bundle.GetHeader(string)"/> which should be analyzed for making this routing descision, can be defined by setting
    /// <see cref="RoutingHeaderName"/>. If the analyzed header (attribute) value does not match any route or the header does not exist, the default route will be chosen, which can defined by setting <see cref="Successor"/> or <see cref="DefaultRoute"/>.
    /// <example>
    /// Example: The <see cref="Router"/> has three routes "A", "B" and "C" (added by calling <see cref="AddRoute(string, IUnit)"/> with "A", "B", "C" as the name and the <see cref="IUnit"/> implementation as the second parameter) and a default route (defined by setting <see cref="DefaultRoute"/>). The header which should be analyzed for routing decision has the name "route" and is initally set with <see cref="RoutingHeaderName"/>).
    /// - An incoming <see cref="Bundle"/> has the attribute "route" = "A" --> the <see cref="Bundle"/> is forwared to route "A"
    /// - An incoming <see cref="Bundle"/> has the attribuite "route" = "D" --> the <see cref="Bundle"/> is forwared to the default route, since "D" is not defined
    /// - An incoming <see cref="Bundle"/> does not have the attribute "route" --> the <see cref="Bundle"/> is forwared to the default route, since it the attribute for analyzing is missing
    /// </example>
    /// Connector class routing an incoming <see cref="Bundle"/> to a specific target (successor)
    /// </summary>
    public class Router : IPipe
    {
        #region Default Route
        /// <summary>
        /// Gets or sets the default route (same as <see cref="DefaultRoute"/>)
        /// </summary>
        public IUnit Successor
        {
            get
            {
                return _defaultRoute;
            }
            set
            {
                _defaultRoute = value;
            }
        }
        /// <summary>
        /// Gets or sets the default route
        /// </summary>
        public IUnit DefaultRoute
        {
            get
            {
                return _defaultRoute;
            }
            set
            {
                _defaultRoute = value;
            }
        }
        

        private IDictionary<string, IUnit> _successors = new Dictionary<string, IUnit>();
        private IUnit _defaultRoute;
        #endregion
        #region Routing
        /// <summary>
        /// Adds a route to the router by specifying the routing value (name of the route) and the destination. An existing entry (same name for the route) will be replaced.
        /// </summary>
        /// <param name="name">name of the route</param>
        /// <param name="successor">destination</param>
        public void AddRoute(string name, IUnit successor)
        {
            if (_successors.ContainsKey(name))
            {
                _successors[name] = successor;
            }
            else
            {
                _successors.Add(name, successor);
            }
        }

        /// <summary>
        /// Removes a route identified by the name of the route if it exists
        /// </summary>
        /// <param name="name">name of the route</param>
        public void RemoveRoute(string name)
        {
            if (_successors.ContainsKey(name))
            {
                _successors.Remove(name);
            }
        }

        private string _routingHeaderName;
        /// <summary>
        /// Gets or sets the name of the header (attribute) of an incoming <see cref="Bundle"/> which is analyzed for making a routing decision.
        /// </summary>
        public string RoutingHeaderName
        {
            get
            {
                return _routingHeaderName;
            }
            set
            {
                _routingHeaderName = value;
            }
        }
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="Router"/>. Specifiec the name of the header (attribute) of an incoming <see cref="Bundle"/> which is analyzed for making a routing decision.
        /// </summary>
        /// <param name="headerName">name of the header</param>
        public Router(string headerName)
        {
            _routingHeaderName = headerName;
        }

        public void In(Bundle bundle)
        {
            string routingDecision = bundle.GetHeader(_routingHeaderName);
            if (_successors.ContainsKey(routingDecision))
            {
                _successors[routingDecision].In(bundle);
            }
            else
            {
                _defaultRoute?.In(bundle);
            }
            
        }
    }
}
