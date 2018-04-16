using System;

namespace R2.Routing.AspNetCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RouteAttribute : Attribute
    {
        private const string _IS_NULL_OR_EMPTY = "{0} is null or empty.";

        private string _prefix = string.Empty;
        private string _template;

        public RouteAttribute(string template)
        {
            Template = template;
        }

        public RouteAttribute(string prefix, string template)
        {
            Prefix = prefix;
            Template = template;
        }

        public string Prefix
        {
            get => _prefix;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _prefix = value;
            }
        }

        public string Template
        {
            get => _template;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(string.Format(_IS_NULL_OR_EMPTY, nameof(value)));
                }

                _template = value;
            }
        }
    }
}