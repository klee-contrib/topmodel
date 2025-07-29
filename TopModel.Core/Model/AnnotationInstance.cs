namespace TopModel.Core.Model;

public record AnnotationInstance(Annotation Annotation, IDictionary<string, string> Parameters);
