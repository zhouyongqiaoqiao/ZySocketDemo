using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZySocketCore.Client
{
    internal class TaskManager
    {
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        public TaskManager()
        {
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        public void Start()
        {

            backgroundWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            backgroundWorker.CancelAsync();
        }

        public void AddTask(Action action)
        {
            backgroundWorker.RunWorkerAsync(action);
        }

        public void AddTask(Action<object> action, object state)
        {
            backgroundWorker.RunWorkerAsync(new Tuple<Action<object>, object>(action, state));
        }

        public void RemoveTask(Action action)
        {
            
        }

    }
}
