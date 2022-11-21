using System;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.Adapters;

namespace WebFormRail
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class WebFormPageAdapter : PageAdapter
    {
        public override System.Web.UI.PageStatePersister GetStatePersister()
        {
            return new StreamPageStatePersister(Page);
        }
    }

    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class StreamPageStatePersister : System.Web.UI.PageStatePersister
    {
        private readonly Guid GuidViewStateFilePath = Guid.NewGuid();
        private string sViewStateFileName;

        private readonly string sViewStatePersisterPath =
            string.Format(@"{0}\{1}\",
                          Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine).TrimEnd('\\'),
                          "WebFormRailViewStatePersist");

        public StreamPageStatePersister(Page page) : base(page)
        {
        }

        //

        // Load ViewState and ControlState.

        //

        public override void Load()
        {
            sViewStateFileName = Page.Request["__VIEWSTATE_KEY"];

            using (Stream stateStream = GetSecureStream())
            {
                ObjectStateFormatter formatter = (ObjectStateFormatter)StateFormatter;
                //BinaryFormatter formatter = new BinaryFormatter();

                // Deserilize returns the Pair object that is serialized in

                // the Save method.

                Pair statePair = (Pair)formatter.Deserialize(stateStream);

                ViewState = statePair.First;

                ControlState = statePair.Second;

                stateStream.Close();
            }
        }

        //

        // Persist any ViewState and ControlState.

        //

        public override void Save()
        {
            if (ViewState != null || ControlState != null)
            {
                if (Page.Session != null)
                {
                    sViewStateFileName = sViewStatePersisterPath + GuidViewStateFilePath;

                    Page.ClientScript.RegisterHiddenField("__VIEWSTATE_KEY", sViewStateFileName);

                    using (Stream stateStream = GetSecureStream())
                    {
                        ObjectStateFormatter formatter = (ObjectStateFormatter)StateFormatter;
                        //BinaryFormatter formatter = new BinaryFormatter();

                        Pair statePair = new Pair(ViewState, ControlState);

                        // Serialize the statePair object to a string.

                        formatter.Serialize(stateStream, statePair);

                        stateStream.Close();
                    }
                }

                else

                    throw new InvalidOperationException("Session needed for StreamPageStatePersister.");
            }
        }


        // Return a secure Stream for your environment.

        private Stream GetSecureStream()
        {
            if (!Directory.Exists(sViewStatePersisterPath))
                Directory.CreateDirectory(sViewStatePersisterPath);

            FileStream fs = new FileStream(sViewStateFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BufferedStream buffer = new BufferedStream(fs);

            return buffer;
        }
    }
}