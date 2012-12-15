using System.Windows.Forms;

namespace Enhancer.Forms
{
    public struct InputResult
    {
        public DialogResult DialogResult { get; internal set; }
        public object[] Values { get; internal set; }

        //public override string ToString()
        //{
        //    return string.Format("Result: {{{0}}}, Values: {{{1}}}", DialogResult.ToString(), Values.ToString());
        //}
    }
}
