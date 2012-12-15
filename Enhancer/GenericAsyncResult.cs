using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enhancer
{
    public class GenericAsyncResult : AsyncResult<Object>
    {
        public GenericAsyncResult(AsyncCallback cb, object state) : base(cb, state) { }
        internal GenericAsyncResult(AsyncCallback cb, object state, bool completed)
            : base(cb, state, completed) { }
    }
}
