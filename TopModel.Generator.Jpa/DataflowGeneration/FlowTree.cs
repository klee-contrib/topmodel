using TopModel.Core.Model;

namespace TopModel.Generator.Jpa.DataflowGeneration;

public class FlowTree
{
    public FlowTree(IList<DataFlow> flows)
    {
        var hasIndependantFlow = Graps(flows).Count > 1;
        RootFlows = hasIndependantFlow ? [] : flows.Where(f => !flows.Intersect(f.DependsOn).Any()).ToList();
        while (flows.Any(f => !Flows.Contains(f)))
        {
            var f = flows.First(f => !Flows.Contains(f));
            var subFlowDataFlows = new List<DataFlow>();
            var stack = new Queue<DataFlow>();
            stack.Enqueue(f);
            while (stack.TryDequeue(out var s))
            {
                subFlowDataFlows.Add(s);
                foreach (
                    var depFlow in flows.Where(f =>
                        !Flows.Concat(subFlowDataFlows).Contains(f)
                        && (s.DependsOn.Contains(f) || f.DependsOn.Contains(s))
                    )
                )
                {
                    stack.Enqueue(depFlow);
                }
            }

            Subflows.Add(new FlowTree(subFlowDataFlows));
        }
    }

    // Flows dont toutes les dépendances sont déjà passées
    public IList<DataFlow> RootFlows { get; set; } = [];

    // Arbres de dépendances
    public IList<FlowTree> Subflows { get; set; } = [];

    // Tous les flows de l'arbre
    public IList<DataFlow> Flows => RootFlows.Concat(Subflows.SelectMany(s => s.Flows)).Distinct().ToList();

    public string ToFlow(int indentLevel)
    {
        var baseIndent = "  ";
        var indent = baseIndent;
        for (int i = 0; i < indentLevel; i++)
        {
            indent += baseIndent;
        }

        if (Flows.Count == 1)
        {
            return $"{Flows[0].Name.ToCamelCase()}Flow";
        }

        var result = $" //\n{indent}new FlowBuilder<Flow>(\"{string.Join('-', Flows.Select(r => r.Name))}\")";
        var next = "start";
        if (RootFlows.Count == 1)
        {
            result += $" //\n{indent}{baseIndent}.start({RootFlows[0].Name.ToCamelCase()}Flow)";
            next = "next";
        }
        else if (RootFlows.Count > 1)
        {
            result +=
                $" //\n{indent}{baseIndent}.start(new FlowBuilder<Flow>(\"{string.Join('-', RootFlows.Select(r => r.Name))}\")";
            next = "next";
            result += $" //\n{indent}{baseIndent}.split(taskExecutor)";
            result += $" //\n{indent}{baseIndent}.add(";
            foreach (var s in RootFlows)
            {
                result += $" //\n{indent}{baseIndent}{baseIndent}{s.Name.ToCamelCase()}Flow";
                if (RootFlows.IndexOf(s) != RootFlows.Count - 1)
                {
                    result += ",";
                }
            }

            result += $" //\n{indent}{baseIndent})";
            result += $" //\n{indent}.end()";
            result += $" //\n{indent})";
        }

        if (Subflows.Count == 1)
        {
            result += $"//\n{indent}{baseIndent}.{next}({Subflows[0].ToFlow(indentLevel + 2)})";
        }
        else
        {
            if (RootFlows.Any())
            {
                result += $" //\n{indent}.{next}(";
                result +=
                    $" //\n{indent}{baseIndent}new FlowBuilder<Flow>(\"{string.Join('-', Subflows.SelectMany(s => s.Flows).Select(r => r.Name))}\")";
            }

            result += $" //\n{indent}{baseIndent}.split(taskExecutor)";
            result += $" //\n{indent}{baseIndent}.add(";
            foreach (var subflow in Subflows)
            {
                result += $" //\n{indent}{baseIndent}{baseIndent}{subflow.ToFlow(indentLevel + 2)}";
                if (Subflows.IndexOf(subflow) != Subflows.Count - 1)
                {
                    result += ",";
                }
            }

            if (RootFlows.Any())
            {
                result += $" //\n{indent}{baseIndent})";
                result += $" //\n{indent}{baseIndent}.end()";
            }

            result += $" //\n{indent})";
        }

        result += $" //\n{indent}.build()";
        return result;
    }

    protected virtual IList<List<DataFlow>> Graps(IList<DataFlow> flows)
    {
        var graps = new List<List<DataFlow>>();
        while (flows.Any(f => !graps.SelectMany(t => t).Contains(f)))
        {
            var f = flows.First(f => !graps.SelectMany(t => t).Contains(f));
            var subFlowDataFlows = new List<DataFlow>();
            var stack = new Queue<DataFlow>();
            stack.Enqueue(f);
            while (stack.TryDequeue(out var s))
            {
                subFlowDataFlows.Add(s);
                foreach (
                    var depFlow in flows.Where(f =>
                        !graps.SelectMany(t => t).Concat(subFlowDataFlows).Contains(f)
                        && (s.DependsOn.Contains(f) || f.DependsOn.Contains(s))
                    )
                )
                {
                    stack.Enqueue(depFlow);
                }
            }

            graps.Add(subFlowDataFlows);
        }

        return graps;
    }
}
