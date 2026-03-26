using System.Xml;
using System.Xml.Linq;

namespace XPathValidator.Services;

internal sealed class XPathNamespaceResolver
{
	private readonly XmlNamespaceManager _manager;
	private readonly Dictionary<string, string> _namespaceToPrefix = [];

	public XPathNamespaceResolver(XDocument document)
	{
		_manager = new XmlNamespaceManager(new NameTable());
		CollectNamespaces(document);
	}

	public XmlNamespaceManager Manager => _manager;

	public string? GetPrefix(XNamespace ns)
	{
		return ns == XNamespace.None
			? null
			: _namespaceToPrefix.GetValueOrDefault(ns.NamespaceName);
	}

	private void CollectNamespaces(XDocument document)
	{
		foreach (var element in document.Descendants())
		{
			foreach (var attr in element.Attributes())
			{
				if (attr.Name.Namespace == XNamespace.Xmlns)
				{
					RegisterNamespace(attr.Name.LocalName, attr.Value);
				}
				else if (attr.Name.LocalName == "xmlns" && attr.Name.Namespace == XNamespace.None)
				{
					RegisterNamespace("ns", attr.Value);
				}
			}
		}
	}

	private void RegisterNamespace(string prefix, string uri)
	{
		if (!_namespaceToPrefix.ContainsKey(uri))
		{
			_namespaceToPrefix[uri] = prefix;
			_manager.AddNamespace(prefix, uri);
		}
	}
}