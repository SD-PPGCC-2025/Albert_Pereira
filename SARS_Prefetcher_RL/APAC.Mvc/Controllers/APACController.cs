using APAC.Core;
using Microsoft.AspNetCore.Mvc;

namespace APAC.Mvc.Controllers
{
    public class APACController : Controller
    {
        private static APACAgent? agent;
        private static PrefetchEnvironment? env;
        private static List<double> rewards = new();
        public IActionResult Index()
        {
            ViewBag.Rewards = rewards;
            return View();
        }

        [HttpPost]
        public IActionResult Initialize(int stateCount, int actionCount)
        {
            agent = new APACAgent(stateCount, actionCount);
            env = new PrefetchEnvironment(stateCount, actionCount);
            rewards.Clear();
            TempData["Message"] = "Agente e ambiente inicializados.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Train(int episodes)
        {
            if (agent == null || env == null)
            {
                TempData["Error"] = "Inicialize o agente primeiro.";
                return RedirectToAction("Index");
            }

            int state = 0;
            int action = agent.ChooseAction(state);

            for (int episode = 0; episode < episodes; episode++)
            {
                var (nextState, reward) = env.Step(state, action);
                int nextAction = agent.ChooseAction(nextState);

                agent.Update(state, action, reward, nextState, nextAction);

                state = nextState;
                action = nextAction;
                rewards.Add(reward);
            }

            TempData["Message"] = $"Treinamento de {episodes} episódios concluído.";
            return RedirectToAction("Index");
        }
    }
}
