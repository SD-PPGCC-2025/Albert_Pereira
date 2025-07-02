using DistributedSnapshot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    private readonly SnapshotService _snapshotService;
    public IndexModel(SnapshotService snapshotService) => _snapshotService = snapshotService;

    public IActionResult OnPostFailNode(int nodeId)
    {
        _snapshotService.FailNode(nodeId);
        return new JsonResult(new { success = true });
    }

    public IActionResult OnPostRecoverNode(int nodeId)
    {
        _snapshotService.RecoverNode(nodeId);
        return new JsonResult(new { success = true });
    }

    public IActionResult OnGetNodeStatus()
    {
        var nodes = _snapshotService.GetAllNodes()
            .Select(n => new { id = n.Id, isAlive = n.IsAlive, isRecovering = n.IsRecovering })
            .ToList();
        return new JsonResult(nodes);
    }
}