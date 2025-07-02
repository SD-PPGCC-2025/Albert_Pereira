using DistributedSnapshot.Models;
using DistributedSnapshot.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSnapshot.Controllers
{
    public class SnapshotController : Controller
    {
        private readonly SnapshotService _snapshotService;

        public SnapshotController(SnapshotService svc) => _snapshotService = svc;

        public IActionResult Index() => View();

        [HttpPost]
        public JsonResult StartSnapshot(int initiatorId = 1)
        {
            SnapshotResult result = _snapshotService.StartSnapshot(initiatorId);
            return Json(result);
        }

        [HttpPost]
        public JsonResult FailNode(int nodeId)
        {
            _snapshotService.FailNode(nodeId);
            return new JsonResult(new { success = true });
        }

        [HttpPost]
        public JsonResult RecoverNode(int nodeId)
        {
            _snapshotService.RecoverNode(nodeId);
            return new JsonResult(new { success = true });
        }

        [HttpGet]
        public JsonResult NodeStatus()
        {
            var nodes = _snapshotService.GetAllNodes()
                .Select(n => new { id = n.Id, isAlive = n.IsAlive, isRecovering = n.IsRecovering })
                .ToList();
            return new JsonResult(nodes);
        }
    }
}
