﻿using System.Globalization;
using TopModel.Core.FileModel;
using TopModel.Core.Types;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class DomainLoader : ILoader<Domain>
{
    private readonly FileChecker _fileChecker;

    public DomainLoader(FileChecker fileChecker)
    {
        _fileChecker = fileChecker;
    }

    public Domain Load(Parser parser)
    {
        var domain = new Domain();

        parser.ConsumeMapping(() =>
        {
            var prop = parser.Consume<Scalar>().Value;
            parser.TryConsume<Scalar>(out var value);

            switch (prop)
            {
                case "name":
                    domain.Name = new LocatedString(value);
                    break;
                case "label":
                    domain.Label = value!.Value;
                    break;
                case "length":
                    domain.Length = Convert.ToInt32(decimal.Parse(value!.Value, CultureInfo.InvariantCulture));
                    break;
                case "scale":
                    domain.Scale = Convert.ToInt32(decimal.Parse(value!.Value, CultureInfo.InvariantCulture));
                    break;
                case "autoGeneratedValue":
                    domain.AutoGeneratedValue = value!.Value == "true";
                    break;
                case "bodyParam":
                    domain.BodyParam = value!.Value == "true";
                    break;
                case "listDomain":
                    domain.ListDomainReference = new DomainReference(value!);
                    break;
                case "csharp":
                    domain.CSharp = _fileChecker.Deserialize<CSharpType>(parser);
                    break;
                case "ts":
                    domain.TS = _fileChecker.Deserialize<TSType>(parser);
                    break;
                case "java":
                    domain.Java = _fileChecker.Deserialize<JavaType>(parser);
                    break;
                case "sqlType":
                    domain.SqlType = value!.Value;
                    break;
                case "mediaType":
                    domain.MediaType = value!.Value;
                    break;
            }
        });

        return domain;
    }
}