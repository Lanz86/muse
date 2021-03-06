﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using System.IO;
using YamlDotNet.Serialization;

namespace Muse.Web.Services
{
	public static class YamlExtensions
	{
		static readonly Regex r = new Regex(@"^---([\d\D\w\W\s\S]+)---", RegexOptions.Multiline);
		public static IDictionary<string, object> YamlHeader(this string text)
		{
			var results = new Dictionary<string, object>();
			var m = r.Matches(text);
			if (m.Count == 0)
				return results;

			var input = new StringReader(m[0].Groups[1].Value);

			var yaml = new YamlStream();
			yaml.Load(input);

			var root = yaml.Documents[0].RootNode;

			var collection = root as YamlMappingNode;
			if (collection != null) {
				foreach (var entry in collection.Children) {
					var node = entry.Key as YamlScalarNode;
					if (node != null) {
						results.Add(node.Value, entry.Value.ToString());
					}
				}
			}

			return results;
		}

		//public static string ToYaml<T>(this T model)
		//{
		//	var serializer = new Serializer();
		//	var stringWriter = new StringWriter();

		//	serializer.Serialize(stringWriter, model);

		//	return stringWriter.ToString();
		//}

		public static string ExcludeHeader(this string text)
		{
			var m = r.Matches(text);
			if (m.Count == 0)
				return text;

			return text.Replace(m[0].Groups[0].Value, "").Trim();
		}
	}
}