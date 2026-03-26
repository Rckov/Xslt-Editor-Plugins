## XSLT Editor Plugins

Plugin collection for [XSLT Editor](https://github.com/Rckov/Xslt-Editor). Built with [XsltEditor.Sdk](https://github.com/Rckov/Xslt-Editor-Sdk).

### Available Plugins

|Plugin|Description|
|-|-|
|XML Formatter|Formats XML and XSL documents with indentation|
|XPath Validator|Validates XPath expressions in XML document|

### Build \& Install

```
dotnet build -c Release
```

Copy the output dll to `plugins/{plugin-name.dll}/` next to the editor executable.

## License
[MIT License](LICENSE) | [Report an Issue](https://github.com/Rckov/Xslt-Editor-Plugins/issues)

