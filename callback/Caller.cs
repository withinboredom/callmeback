using System;
using System.Collections;
using System.Text;
using SKYPE4COMLib;

/* Caller Class
 * Description: Connects to Skype instance.  Handles outgoing calls and recording.
 *  One Instance per runtime.
 */

namespace callback
{
    class Caller
    {
        Skype m_skype = new Skype();
        ArrayList m_callers = null;
        ArrayList m_recordings = null;
        bool m_attached;

        public bool Attached
        {
            get { return m_attached; }
        }

        public bool callList(ArrayList numbers)
        {
            if (m_callers == null)
                return false;

            m_callers = numbers;

            return true;
        }

        public bool addCaller(string number)
        {
            m_callers.Add(number);

            return true;
        }

        public Caller()
        {
            m_skype.CallStatus += CallStatus;
            m_skype.Attach(5, true);
        }

        private bool call()
        {
            if (m_skype.ActiveCalls.Count > 0)
            {
            }
            else
            {
                m_skype.PlaceCall(m_callers[0].ToString(), m_callers[1].ToString(), m_callers[2].ToString(), m_callers[3].ToString());
            }

            return true;
        }

        private void CallStatus(Call call, TCallStatus status)
        {
        }
    }
}
