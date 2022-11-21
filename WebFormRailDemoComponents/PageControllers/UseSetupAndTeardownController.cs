using System;
using System.Collections.Generic;
using System.Text;
using WebFormRail;

namespace WebFormRailDemoComponents
{
    class UseSetupAndTeardownController : SetupAndTeardownController
    {
        public void Index()
        {
        }

        public void Look()
        {
        }
    }
    public class SetupAndTeardownController : WebFormController
    {
        [Setup]
        private void SetupData()
        {
            ViewData["Setup"] = DateTime.Now;
        }
        [Teardown]
        private void TeardownData()
        {
            ViewData["Teardown"] = "Teardown data";
        }
    }
}
