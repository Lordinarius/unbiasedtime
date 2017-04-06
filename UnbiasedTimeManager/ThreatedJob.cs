//Copyright(c) 2017 �mer Faruk Say�l�r

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using UnityEngine;
using System.Threading;

namespace UnbiasedTimeManager
{

    public class GetNetworkTime : CustomYieldInstruction
    {
        Thread thread;
        float abordTime;
        float endTime;
        bool started = false;
        public bool isSuccess = false;
        public ulong time;

        public override bool keepWaiting
        {
            get
            {
                if (!started)
                {
                    endTime = Time.unscaledTime + abordTime;
                    started = true;
                    thread.Start();
                }
                bool isTimeCompleted = Time.unscaledTime >= endTime;
                if (isTimeCompleted)
                {
                    started = false;
                    isSuccess = false;
                    thread.Abort();
                    return false;
                }
                return thread.IsAlive;
            }
        }

        public GetNetworkTime(UnbiasedTime timeManager, float abordTime = 3)
        {
            this.abordTime = abordTime;
            ThreadStart threadStart = new ThreadStart(delegate
            {
                time = timeManager.TryToGetTime(out isSuccess);
            });
            thread = new Thread(threadStart);
        }

    }

    public class ThreatedJob<T> : CustomYieldInstruction
    {
        Thread thread;
        float abordTime;
        float endTime;
        bool started = false;
        public bool isSuccess = true;
        public T data;

        public override bool keepWaiting
        {
            get
            {
                if (!started)
                {
                    endTime = Time.unscaledTime + abordTime;
                    started = true;
                    thread.Start();
                }
                bool isTimeCompleted = Time.unscaledTime >= endTime;
                if (isTimeCompleted)
                {
                    started = false;
                    isSuccess = false;
                    thread.Abort();
                    return false;
                }
                return thread.IsAlive;
            }
        }

        public ThreatedJob(ThreatedEvent<T> job, float abordTime = 2)
        {
            this.abordTime = abordTime;
            ThreadStart thre = new ThreadStart(delegate
            {
                data = job.Invoke(out isSuccess);
            });
            thread = new Thread(thre);
        }

    }


    public class ThreatedJob : CustomYieldInstruction
    {
        Thread thread;
        float abordTime;
        float endTime;
        bool started = false;
        public bool isSuccess = true;

        public override bool keepWaiting
        {
            get
            {
                if (!started)
                {
                    endTime = Time.unscaledTime + abordTime;
                    started = true;
                    thread.Start();
                }
                bool isTimeCompleted = Time.unscaledTime >= endTime;
                if (isTimeCompleted)
                {
                    started = false;
                    isSuccess = false;
                    thread.Abort();
                    return false;
                }
                return thread.IsAlive;
            }
        }

        public ThreatedJob(ThreatedEvent job, float abordTime = 2)
        {
            this.abordTime = abordTime;
            ThreadStart thre = new ThreadStart(delegate
            {
                job.Invoke(out isSuccess);
            });
            thread = new Thread(thre);
        }

    }

    public delegate T ThreatedEvent<T>(out bool success);
    public delegate void ThreatedEvent(out bool success);

}