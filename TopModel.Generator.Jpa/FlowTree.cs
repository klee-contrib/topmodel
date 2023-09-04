using TopModel.Core;

namespace TopModel.Generator.Jpa;

public class FlowTree
{
    private readonly bool _hasIndependantFlows = false;

    public FlowTree(List<DataFlow> flows)
    {
        RootFlows = flows.Where(f => !flows.Intersect(f.DependsOn).Any()).ToList();
        if (RootFlows.Count > 1)
        {
            var independantTrees = new List<List<DataFlow>>();
            while (RootFlows.Any(f => !independantTrees.SelectMany(t => t).Contains(f)))
            {
                var f = RootFlows.Where(f => !independantTrees.SelectMany(t => t).Contains(f)).First();
                var subFlowDataFlows = new List<DataFlow>();
                var stack = new Queue<DataFlow>();
                stack.Enqueue(f);
                var flowsToAdd = new List<DataFlow>();
                while (stack.TryDequeue(out var s))
                {
                    subFlowDataFlows.Add(s);
                    foreach (var depFlow in flows.Where(f => !Subflows.SelectMany(s => s.Flows).Concat(subFlowDataFlows).Contains(f)).Where(f => s.DependsOn.Contains(f) || f.DependsOn.Contains(s)))
                    {
                        stack.Enqueue(depFlow);
                    }
                }

                if (RootFlows.TrueForAll(r => subFlowDataFlows.Contains(r)))
                {
                    break;
                }
                else
                {
                    _hasIndependantFlows = true;
                    independantTrees.Add(subFlowDataFlows);
                    Subflows.Add(new FlowTree(subFlowDataFlows));
                }
            }
        }

        while (flows.Any(f => !Flows.Contains(f)))
        {
            var f = flows.Where(f => !Flows.Contains(f)).First();
            var subFlowDataFlows = new List<DataFlow>();
            var stack = new Queue<DataFlow>();
            stack.Enqueue(f);
            var flowsToAdd = new List<DataFlow>();
            while (stack.TryDequeue(out var s))
            {
                subFlowDataFlows.Add(s);
                foreach (var depFlow in flows.Where(f => !Flows.Concat(subFlowDataFlows).Contains(f)).Where(f => s.DependsOn.Contains(f) || f.DependsOn.Contains(s)))
                {
                    stack.Enqueue(depFlow);
                }
            }

            Subflows.Add(new FlowTree(subFlowDataFlows));
        }
    }

    public List<DataFlow> RootFlows { get; set; } = new();

    public List<FlowTree> Subflows { get; set; } = new();

    public List<DataFlow> Flows => RootFlows.Concat(Subflows.SelectMany(s => s.Flows)).Distinct().ToList();

    public string ToFlow(int indentLevel)
    {
        var indent = "	";
        for (int i = 0; i < indentLevel; i++)
        {
            indent += "	";
        }

        if (Flows.Count == 1)
        {
            return $"{Flows.First().Name.ToCamelCase()}Flow";
        }

        if (_hasIndependantFlows)
        {
            var result = $" //\n{indent}new FlowBuilder<Flow>(\"{string.Join('-', Flows.Select(r => r.Name))}\")";
            result += $" //\n{indent}.split(taskExecutor)";
            result += $" //\n{indent}.add( //\n{indent}	{string.Join($", ", Subflows.Select(s => $"{s.ToFlow(indentLevel + 1)}"))})";
            result += $" //\n{indent}.end()";

            return result;
        }
        else
        {
            var result = $" //\n{indent}new FlowBuilder<Flow>(\"{string.Join('-', Flows.Select(r => r.Name))}\")";
            if (RootFlows.Count == 1)
            {
                result += $" //\n{indent} .start({RootFlows.First().Name.ToCamelCase()}Flow)";
            }
            else
            {
                result += $" //\n{indent} .start(new FlowBuilder<Flow>(\"{string.Join('-', RootFlows.Select(r => r.Name))}\")";
                result += $" //\n{indent}   .split(taskExecutor)";
                result += $" //\n{indent}   .add({string.Join(", ", RootFlows.Select(s => $"{s.Name.ToCamelCase()}Flow"))})";
                result += $" //\n{indent}   .end())";
            }

            if (Subflows.Count == 1)
            {
                result += $"//\n{indent} .next({Subflows.First().ToFlow(indentLevel + 1)})";
            }
            else
            {
                result += $" //\n{indent} .next(";
                result += $" //\n{indent}  new FlowBuilder<Flow>(\"{string.Join('-', Subflows.SelectMany(s => s.Flows).Select(r => r.Name))}\")";
                result += $" //\n{indent}   .split(taskExecutor)";
                result += $" //\n{indent}   .add({string.Join(", ", Subflows.Select(s => s.ToFlow(indentLevel + 1)))})";
                result += $" //\n{indent} .end())";
            }

            result += $"  //\n{indent} .build()";
            return result;
        }
    }
}