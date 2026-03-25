using System.Xml;
using System.Xml.Linq;

using XsltEditor.Sdk;
using XsltEditor.Sdk.Abstractions;

namespace Formatter;

public sealed class FormatterPlugin : PluginBase
{
	public override string Name => "XML Formatter";
	public override string Description => "Formats XML and XSL documents with indentation";

	public override void Execute(IDocumentContext context)
	{
		foreach (var doc in context.Documents)
		{
			doc.Content = Format(doc.Content);
		}
	}

	private static string? Format(string? content)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return content;
		}

		try
		{
			var doc = XDocument.Parse(content);

			using var writer = new StringWriter();
			using var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
			{
				Indent = true,
				IndentChars = "  ",
				OmitXmlDeclaration = doc.Declaration is null
			});

			doc.WriteTo(xmlWriter);
			xmlWriter.Flush();

			return writer.ToString();
		}
		catch
		{
			return content;
		}
	}
}