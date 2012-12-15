using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Threading;

namespace Enhancer
{
    class WPF2WFBridge : UserControl, ISynchronizeInvoke
    {
        public Dispatcher RemoteDispatcher { get; set; }
        public DispatcherOperation DispatcherOperation { get; set; }

        new public IAsyncResult BeginInvoke(Delegate method, params object[] args)
        {
            return new GenericAsyncResult(null, (DispatcherOperation = RemoteDispatcher.BeginInvoke(method, args)).Result);
        }

        new public object EndInvoke(IAsyncResult result)
        {
            return base.EndInvoke(result);
        }

        new public object Invoke(Delegate method, params object[] args)
        {
            return RemoteDispatcher.Invoke(method, args);
        }

        new public bool InvokeRequired
        {
            get { return RemoteDispatcher.CheckAccess(); }
        }
    }
}
