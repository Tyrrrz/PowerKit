using System.Linq;
using System.Xml.Linq;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="XElement" />.
/// </summary>
public static class XElementExtensions
{
    extension(XElement element)
    {
        /// <summary>
        /// Returns a copy of the element with element namespaces, namespace declarations, and
        /// non-reserved attribute namespace prefixes removed from the element and its descendants.
        /// Attributes in the reserved <c>xml</c> namespace are preserved.
        /// </summary>
        public XElement StripNamespaces()
        {
            // Adapted from http://stackoverflow.com/a/1147012

            var result = new XElement(element);

            foreach (var descendantElement in result.DescendantsAndSelf())
            {
                descendantElement.Name = XNamespace.None.GetName(descendantElement.Name.LocalName);

                descendantElement.ReplaceAttributes(
                    descendantElement
                        .Attributes()
                        .Where(a => !a.IsNamespaceDeclaration)
                        .Select(a =>
                            a.Name.Namespace == XNamespace.Xml
                                ? new XAttribute(a.Name, a.Value)
                                : new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value)
                        )
                );
            }

            return result;
        }
    }
}
