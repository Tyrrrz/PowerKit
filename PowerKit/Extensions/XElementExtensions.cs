#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class XElementExtensions
{
    extension(XElement element)
    {
        /// <summary>
        /// Returns a copy of the element with all namespace declarations and namespace prefixes
        /// removed from the element and its descendants.
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
                        .Where(a =>
                            a.Name.Namespace != XNamespace.Xml
                            && a.Name.Namespace != XNamespace.Xmlns
                        )
                        .Select(a => new XAttribute(
                            XNamespace.None.GetName(a.Name.LocalName),
                            a.Value
                        ))
                );
            }

            return result;
        }
    }
}
