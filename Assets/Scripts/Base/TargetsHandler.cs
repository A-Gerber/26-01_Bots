using System.Collections.Generic;

public class TargetsHandler
{
    private Queue<Resource> _targets;
    private List<Resource> _performedTasks;

    public int Count => _targets.Count;

    public TargetsHandler()
    {
        _targets = new Queue<Resource>();
        _performedTasks = new List<Resource>();
    }

    public void TransferTargets(List<Resource> resources)
    {
        if (_targets.Count > 0 || _performedTasks.Count > 0)
        {
            foreach (var resource in resources)
            {
                if (_targets.Contains(resource) == false)
                    _targets.Enqueue(resource);
            }
        }
        else
        {
            foreach (var resource in resources)
            {
                _targets.Enqueue(resource);
            }
        }
    }

    public Resource SetTarget()
    {
        Resource resource = _targets.Dequeue();
        resource.SetStatusFinded();
        _performedTasks.Add(resource);

        return resource;
    }

    public void DeletePerformedTasks(Resource resource)
    {
        _performedTasks.Remove(resource);
    }
}