using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class LBMSrcThread
    {
        private int _threadId;
        private int _numThreads;
        private int _numSrcs;
        private int _nmsgs;
        private int _pause;
        private byte[] _message;
        private int _msglen;
        private LBMSource[] _sources;
        private Thread myThread;

        public LBMSrcThread(int threadId, int numThreads, byte[] message, int msglen, int nmsgs, LBMSource[] sources, int numSrcs, int pause)
        {
            _threadId = threadId;
            _numThreads = numThreads;
            _numSrcs = numSrcs;
            _nmsgs = nmsgs;
            _sources = sources;
            _message = message;
            _msglen = msglen;
            _pause = pause;
        }

        public void start()
        {
            myThread = new Thread(new ThreadStart(run));
            myThread.Start();
        }

        public void join()
        {
            if (myThread != null)
            {
                myThread.Join();
            }
        }

        public void run()
        {
            while (_nmsgs > 0)
            {
                for (int i = _threadId; i < _numSrcs; i += _numThreads)
                {
                    _sources[i].send(_message, _msglen, 0);
                    if (--_nmsgs == 0)
                        break;
                    if (_pause > 0)
                    {
                        Thread.Sleep(_pause);
                    }
                }
            }
        }
    }
}
