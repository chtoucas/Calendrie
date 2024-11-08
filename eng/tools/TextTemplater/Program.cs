// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

// Adapted from https://github.com/unoplatform/uno/tree/master/src/T4Generator
// See also https://github.com/mono/t4/blob/main/TextTransform/TextTransform.cs

using System;
using System.CodeDom.Compiler;
using System.IO;

if (args.Length == 0) throw new ArgumentException("The template file path was missing.");

string? templateFile = args[0] ?? throw new ArgumentException("The specified template file path was null.");
if (File.Exists(templateFile) == false) throw new FileNotFoundException("The specified template file does not exist.");

var host = new TextTemplater.ConsoleHost(templateFile);

string output = ProcessTemplate(templateFile, host);

if (host.Errors.HasErrors)
{
    foreach (CompilerError err in host.Errors) Console.WriteLine(err);
}

string outputFile =
    Path.Combine(Path.GetDirectoryName(templateFile), Path.GetFileNameWithoutExtension(templateFile))
    + host.FileExtension;

File.WriteAllText(outputFile, output, host.OutputEncoding);

static string ProcessTemplate(string templateFile, TextTemplater.ConsoleHost host)
{
    using var tt = new Microsoft.VisualStudio.TextTemplating.Engine();

    string input = File.ReadAllText(templateFile);
    return tt.ProcessTemplate(input, host);
}
