# TopModel Language Service for VSCode

![Features demo](https://raw.githubusercontent.com/klee-contrib/topmodel/develop/TopModel.VSCode/demo.gif "Features demonstration")

## Available features

This extension greatly enhances the development experience for TopModel projects (files with `*.tmd` extensions).

Main features:

- Semantic highlighting (class references, properties and domains, imports)
- Auto-completion for domains, classes and alias properties
- Validation and display of errors and warnings
- Commands `TopModel: Start model generation` and `TopModel: Start model generation (watch mode)` with automatic detection of configuration file(s)
- Automatic imports
- Symbol search (classes, domains and endpoints) with `Ctrl + T`
- List references for classes and domains (via `Shift + F12` and CodeLens)
- Formatting:
  - Oranize imports (`Alt + Shift + O`)
- Input assistance:
  - Add missing import
  - Add missing class to current file
  - Add missing domain to domains file
  - Rename classes and domains (`F2`)
- UML diagram preview
- `Outline` panel
- Warnings
  - Unused imports
  - Duplicate trigrams
  - Duplicate alias properties
